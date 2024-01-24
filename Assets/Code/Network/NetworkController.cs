using System;
using UnityEngine;
using GONet;
using System.Collections.Generic;
using DATS;

public class NetworkController : GONetParticipantCompanionBehaviour
{
    [SerializeField] private GONetParticipant _playerBipedPrefab;
    private GONetParticipant _myPlayerBipedInstance = default;
    private readonly Dictionary<ushort, PlayerController> _bipedControllersByAuthorityId = new Dictionary<ushort, PlayerController>();

    [SerializeField] private GONetParticipant _playerDronePrefab;
    private GONetParticipant _myPlayerDroneInstance = default;
    private readonly Dictionary<ushort, DroneController> _droneControllersByAuthorityId = new Dictionary<ushort, DroneController>();

    public event Action OnLocalDisconnect;

    bool _isForceBypassToggleese;

    [SerializeField] private GONetParticipant _obstaclePrefab;
    [SerializeField] private ColorTeamConfiguration[] _colorTeamConfigurations;

    //Client and Server Components
    private GONetRPCHandler _networkRPCHandler;
    private ConnectionComponent _connectionComponent;
    private DroneAndBipedSwitcher _droneAndBipedSwitcher;

    //Client Only Components
    private SpawnPointPicker _spawnPointPicker;
    private PlayerSpawnCountdown _playerSpawnCountdown;

    //Server Only Components
    private ServerSimulator _serverSimulator;
    private ServerHitRegistrator _serverHitRegistrator;
    private ServerEntityEventsHolder _entityEventsHolder;
    private DroneLifetimeHandler _droneLifetimeHandler;

    private ColorTeamAssigner _colorTeamAssigner;
    public ColorTeamAssigner ColorTeamAssigner => _colorTeamAssigner;

    private readonly List<(GONetParticipant tempNetworkEntity, LocalContextEntityEvent entityEvent)> tempEntitiesAwaitingEventSend_remove =
        new List<(GONetParticipant tempNetworkEntity, LocalContextEntityEvent entityEvent)>();
    private readonly List<(GONetParticipant tempNetworkEntity, LocalContextEntityEvent entityEvent)> tempEntitiesAwaitingEventSend =
        new List<(GONetParticipant tempNetworkEntity, LocalContextEntityEvent entityEvent)>();

    public const byte TICK_RATE_HZ = 32;
    public const float TICK_RATE = 1f / TICK_RATE_HZ;

    private const float DRONE_LIFETIME = 23f;
    private const float PLAYER_SPAWN_COUNTDOWN = 3f;

    private IPlayerNetworkEntity _playerNetworkEntityToReplace;

    private uint _tick;
    private uint _clientTick = 0;

    private bool _isSpawned = false;

    public uint ClientTick => _clientTick;
    public void SetClientTick(uint newValue)
    {
        _clientTick = newValue;
    }

    protected override void Awake()
    {
        base.Awake();

        if (!_playerBipedPrefab || !_playerDronePrefab)
        {
            throw new InvalidOperationException("Need all the prefabs set!");
        }

        _colorTeamAssigner = new ColorTeamAssigner(_colorTeamConfigurations);

        // NOTE: this is instantiated early here to ensure all GNPs registered:
        _entityEventsHolder = new ServerEntityEventsHolder();

        _serverSimulator = new ServerSimulator();
        _connectionComponent = new ConnectionComponent();

        { 
            var sub = EventBus.Subscribe<LocalContextEntityEvent>(OnKilledEntity_Toggleese,(e) => e.Event.type == EntityEventType.EV_KILLED);
            sub.SetSubscriptionPriority(short.MaxValue); // process this last since it may destroy the associated GO and want to allow others to process first
        }
        {
            var sub = EventBus.Subscribe<LocalContextEntityEvent>(OnKilledEntity_Toggleese, (e) => e.Event.type == EntityEventType.EV_ALMOST_KILLED_FORCE_CHANGE_TEAM);
            sub.SetSubscriptionPriority(short.MaxValue); // process this last since it may destroy the associated GO and want to allow others to process first
        }
        EventBus.Subscribe(SyncEvent_GeneratedTypes.SyncEvent_PlayerLocalContext__currentPlayerType, OnPlayerTypeChanged);
        EventBus.Subscribe(SyncEvent_GeneratedTypes.SyncEvent_PlayerLocalContext__isBipedInRagdollState, OnBipedRagdollStateChanged);
        {
            var sub = EventBus.Subscribe(SyncEvent_GeneratedTypes.SyncEvent_GONetParticipant_GONetId, _serverSimulator.OnGONetIdChanged);
            sub.SetSubscriptionPriority(short.MinValue); // process this first since it does data management that others might rely upon
        }
        //GONetMain.EventBus.Subscribe<SyncEvent_ValueChangeProcessed>(OnSomethingSyncd);
    }

    private void OnSomethingSyncd(GONetEventEnvelope<SyncEvent_ValueChangeProcessed> eventEnvelope)
    {
        GONetLog.Debug($"something sync'd.  val: {eventEnvelope.Event.ValueNew}, explanation: {eventEnvelope.Event.Explanation}");
    }

    protected override void OnEnable()
    {
        base.OnEnable();
     
        _connectionComponent.OnLocalDisconnect += InvokeLocalDisconnectEvent;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        _connectionComponent.OnLocalDisconnect -= InvokeLocalDisconnectEvent;
    }

    public override void OnGONetClientVsServerStatusKnown(bool isClient, bool isServer, ushort myAuthorityId)
    {
        base.OnGONetClientVsServerStatusKnown(isClient, isServer, myAuthorityId);

        _networkRPCHandler = new GONetRPCHandler();
        _entityEventsHolder.Initialize();

        if (isServer)
        {
            _serverHitRegistrator = new ServerHitRegistrator(FindObjectOfType<RaycastShooter>(), this);

            GONetMain.gonetServer.ClientDisconnected += Server_OnClientDisconnected;

            GONetMain.EventBus.Subscribe(SyncEvent_GeneratedTypes.SyncEvent_GONetParticipant_GONetId, Server_OnGONetParticipantIDChanged);

            _droneLifetimeHandler = new DroneLifetimeHandler(this, DRONE_LIFETIME);
            _droneAndBipedSwitcher = new DroneAndBipedSwitcher(_serverSimulator, _droneLifetimeHandler);
        }

        if (isClient)
        {
            _playerSpawnCountdown = new PlayerSpawnCountdown(this, PLAYER_SPAWN_COUNTDOWN);
            _playerSpawnCountdown.DisableCountdown();

            _spawnPointPicker = FindAnyObjectByType<SpawnPointPicker>();

            // spawn both of the things the player will control (well, alternate between controlling each...only controlling one directly at a time)
            IPlayerNetworkEntity spawnedPlayerEntity = default;
            _isForceBypassToggleese = true;
            Client_SwitchToControlPlayerType(PlayableEntityType.Drone, _playerDronePrefab, ref _myPlayerDroneInstance, ref spawnedPlayerEntity);
            Client_SwitchToControlPlayerType(PlayableEntityType.Biped, _playerBipedPrefab, ref _myPlayerBipedInstance, ref spawnedPlayerEntity);
            _isForceBypassToggleese = false;
            Toggleese(shouldRagdollBiped: PlayerLocalContext.LookupByAuthorityId(myAuthorityId)._isBipedInRagdollState);

            _droneAndBipedSwitcher = new DroneAndBipedSwitcher(this);
        }

        _isSpawned = true;
        IsTickReceiver = true;
    }

    private bool FindAssosiatedBipedEntity(uint bipedEntityId, out IPlayerNetworkEntity bipedEntity)
    {
        bipedEntity = null;
        bool foundSuccesfully = false;

        GONetParticipant[] gnps = FindObjectsOfType<GONetParticipant>();
        for (int i = 0; i < gnps.Length; ++i)
        {
            if (gnps[i].IsMine && gnps[i].GONetId == bipedEntityId)
            {
                if (gnps[i].gameObject.TryGetComponent(out bipedEntity))
                {
                    foundSuccesfully = true;
                }

                break;
            }
        }

        return foundSuccesfully;
    }

    private void Server_OnGONetParticipantIDChanged(GONetEventEnvelope<SyncEvent_ValueChangeProcessed> eventEnvelope)
    {
        foreach (INetworkEntity entityWithUnsetGONetId in _serverSimulator.networkEntitiesWithUnsetGONetId)
        {
            if (entityWithUnsetGONetId.GetNetworkEntityId() == eventEnvelope.Event.ValueNew.System_UInt32)
            {
                _serverSimulator.networkEntitiesWithUnsetGONetId.Remove(entityWithUnsetGONetId);
                AddNetworkEntityWithUnsetGONetId(entityWithUnsetGONetId, eventEnvelope.Event.ValuePrevious.System_UInt32);

                /*
                if (unsetEntity.DoReceiveEntityStates())
                {
                    CreateEntityStateSynchronizer(unsetEntity.GetNetworkEntityId());
                }
                */

                break;
            }
        }
    }

    private void AddNetworkEntityWithUnsetGONetId(INetworkEntity entity, uint previousId)
    {
        _serverSimulator.AddNetworkEntity(entity);

        if (_entityEventsHolder.entityEventsWithUnsetGONetId.TryGetValue(previousId, out List<LocalContextEntityEvent> entityEvents))
        {
            foreach (LocalContextEntityEvent entityEvent in entityEvents)
            {
                entityEvent.GONetParticipantId = entity.GetNetworkEntityId();
                _entityEventsHolder.AddEvent(entity.GetNetworkEntityId(), entityEvent);
            }
            _entityEventsHolder.entityEventsWithUnsetGONetId.Remove(previousId);
        }
    }

    private void Server_OnClientDisconnected(GONetConnection_ServerToClient gonetConnection_ServerToClient)
    {
        ushort clientAuthorityId = gonetConnection_ServerToClient.OwnerAuthorityId;

        DisconnectClientConnection(clientAuthorityId);
        RemoveAllNetworkContentForAuthority(clientAuthorityId);

        void RemoveAllNetworkContentForAuthority(ushort clientAuthorityId)
        {
            // Remove and despawn its local player entity from the simulation
            if (_serverSimulator.ActiveNetworkEntitiesByAuthorityId.TryGetValue(clientAuthorityId, out HashSet<INetworkEntity> entitiesToDespawn))
            {
                foreach (INetworkEntity entityToDespawn in entitiesToDespawn)
                {
                    uint networkEntityId = entityToDespawn.GetNetworkEntityId();
                    GONetParticipant gnpToDespawn = entityToDespawn.GetNetworkObject();
                    if (gnpToDespawn && gnpToDespawn.gameObject)
                    {
                        Destroy(gnpToDespawn.gameObject);
                    }
                    _serverSimulator.RemoveNetworkEntity(networkEntityId, shouldDestroyFinally: true);
                }
            }
        }
    }

    /// <summary>
    /// TODO call this on client too
    /// </summary>
    private void DisconnectClientConnection(ushort clientAuthorityId)
    {
        _connectionComponent.DisconnectClient(clientAuthorityId); 
    }

    public override void OnGONetParticipantEnabled(GONetParticipant gonetParticipant)
    {
        base.OnGONetParticipantEnabled(gonetParticipant);

        if (gonetParticipant.gameObject.TryGetComponent<INetworkEntity>(out INetworkEntity networkEntity))
        {
            if (networkEntity is IPlayerNetworkEntity)
            {
                IPlayerNetworkEntity playerNetworkEntity = networkEntity as IPlayerNetworkEntity;

                if (IsServer)
                {
                    _serverSimulator.AddNetworkEntity(playerNetworkEntity);
                }

                if (playerNetworkEntity is PlayerController)
                {
                    PlayerController playerController = playerNetworkEntity as PlayerController;
                    _bipedControllersByAuthorityId[playerController.GONetParticipant.OwnerAuthorityId] = playerController;
                }
                if (playerNetworkEntity is DroneController)
                {
                    DroneController droneController = playerNetworkEntity as DroneController;
                    _droneControllersByAuthorityId[droneController.GONetParticipant.OwnerAuthorityId] = droneController;
                }

                OnPlayerTypeChanged_AttemptProcess(playerNetworkEntity.GetNetworkObject().OwnerAuthorityId);
            }

            if (IsServer)
            { 
                if (networkEntity is IColorTeamMember)
                {
                    IColorTeamMember colorTeamMember = networkEntity as IColorTeamMember;
                    byte assignedColor = _colorTeamAssigner.AssignMemberToTeam(networkEntity);
                    colorTeamMember.Server_SetColorTeam(assignedColor);
                }

                if (networkEntity is ObstacleController)
                {
                    Server_AddToAutoDestroyAfterSeconds(gonetParticipant, 60 * 2.5f);
                }
            }
        }
    }

    private readonly List<GONetParticipant> _server_autoDestroyAfterElapsedSeconds_remove = new List<GONetParticipant>();
    private readonly Dictionary<GONetParticipant, double> _server_autoDestroyAfterElapsedSeconds = new Dictionary<GONetParticipant, double>();
    private void Server_AddToAutoDestroyAfterSeconds(GONetParticipant gonetParticipant, float seconds)
    {
        _server_autoDestroyAfterElapsedSeconds[gonetParticipant] = GONetMain.Time.ElapsedSeconds + seconds;
    }

    public override void OnGONetParticipantDisabled(GONetParticipant gonetParticipant) // TODO really need to chnage this for OnGNPDestroyed
    {
        if (IsClient)
        {
            if (gonetParticipant.IsMine)
            {
                if (gonetParticipant.gameObject.TryGetComponent<INetworkEntity>(out INetworkEntity networkEntity))
                {
                    if (networkEntity is IPlayerNetworkEntity)
                    {
                        if (networkEntity is PlayerController)
                        {
                            IPlayerNetworkEntity playerController = networkEntity as IPlayerNetworkEntity;

                            if(playerController.IsActivated)
                            {
                                _playerSpawnCountdown.EnableCountdown(PlayableEntityType.Biped);
                            }
                        }
                    }
                }
            }
        }

        if (IsServer)
        {
            if (GONetMain.Server_IsClientOwnerConnected(gonetParticipant))
            {
                Server_FinishKillPlayerProcess(gonetParticipant.GONetId);
            }
            else
            {
                Server_KillEntity(gonetParticipant.GONetId);
            }

            _server_autoDestroyAfterElapsedSeconds.Remove(gonetParticipant);
        }

        {
            if (gonetParticipant.gameObject.TryGetComponent<INetworkEntity>(out INetworkEntity networkEntity))
            {
                if (networkEntity is IPlayerNetworkEntity)
                {
                    if (networkEntity is PlayerController)
                    {
                        PlayerController playerController = networkEntity as PlayerController;
                        _bipedControllersByAuthorityId.Remove(playerController.GONetParticipant.OwnerAuthorityId);
                    }
                    if (networkEntity is DroneController)
                    {
                        DroneController droneController = networkEntity as DroneController;
                        _droneControllersByAuthorityId.Remove(droneController.GONetParticipant.OwnerAuthorityId);
                    }
                }
            }
        }

        base.OnGONetParticipantDisabled(gonetParticipant);
    }

    public void DisconnectLocal()
    {
        StartCoroutine(_connectionComponent.DisconnectLocal());
    }

    private void InvokeLocalDisconnectEvent()
    {
        StopAllCoroutines();
        OnLocalDisconnect?.Invoke();
    }

    public IPlayerNetworkEntity Client_SwitchToControlPlayerType(PlayableEntityType entityType)
    {
        IPlayerNetworkEntity spawnedPlayerEntity = default;

        switch (entityType)
        {
            case PlayableEntityType.Drone:
                Client_SwitchToControlPlayerType(entityType, _playerDronePrefab, ref _myPlayerDroneInstance, ref spawnedPlayerEntity);
                break;

            default:
            case PlayableEntityType.Biped:
                Client_SwitchToControlPlayerType(entityType, _playerBipedPrefab, ref _myPlayerBipedInstance, ref spawnedPlayerEntity);
                break;
        }

        return spawnedPlayerEntity;
    }

    /// <summary>
    /// IMPORTANT: This will spawn a networked object on the first call only, thereafter it just toggles things so 
    ///            the selected entity type is the one being controlled instead of the other(s)
    /// </summary>
    private void Client_SwitchToControlPlayerType(PlayableEntityType entityType, GONetParticipant originatingPrefab, ref GONetParticipant spawnedGNP, ref IPlayerNetworkEntity spawnedPlayerEntity)
    {
        if (!spawnedGNP)
        {
            _spawnPointPicker.GetSpawnPoint(out Vector3 spawnPosition, out Quaternion spawnRotation);
            spawnedGNP = Instantiate(originatingPrefab, spawnPosition, spawnRotation);
        }

        spawnedPlayerEntity = spawnedGNP.gameObject.GetComponent<IPlayerNetworkEntity>();

        if (!_isForceBypassToggleese && IsMine)
        {
            Toggleese();
        }
    }

    /// <summary>
    /// IMPORTANT: Some processing will occur if <paramref name="authorityId"/> is not mine, 
    /// BUT NOT the actual changing of the <see cref="PlayerLocalContext._currentPlayerType"/> (i.e., that is only set here when is mine).
    /// </summary>
    private void Toggleese(ushort authorityId = default, bool shouldRagdollBiped = false)
    {
        PlayerController playerController = default;
        DroneController droneController = default;
        if (authorityId == default)
        {
            authorityId = GONetMain.MyAuthorityId;
            playerController = _myPlayerBipedInstance.GetComponent<PlayerController>();
            droneController = _myPlayerDroneInstance.GetComponent<DroneController>();
        }
        else
        {
            playerController = _bipedControllersByAuthorityId[authorityId];
            droneController = _droneControllersByAuthorityId[authorityId];
        }

        playerController.HasUnprocessedRagdoll = shouldRagdollBiped;

        //GONetLog.Debug($"toggleese..playerController(id:{playerController.GetNetworkEntityId()}).IsActivated: {playerController.IsActivated}, isMine? {authorityId == GONetMain.MyAuthorityId}");

        if (authorityId == GONetMain.MyAuthorityId)
        {
            if (playerController.IsActivated)
            {
                PlayerLocalContext.Mine.CurrentPlayerType = PlayableEntityType.Drone;
                
                // move drone just above its player to be like taking over the drone that was already above it
                droneController.ResetAbove(playerController.transform);
            }
            else // ASSuME droneController.IsActivated OR neither were and we want to activate player!
            {
                PlayerLocalContext.Mine.CurrentPlayerType = PlayableEntityType.Biped;

                _spawnPointPicker.GetSpawnPoint(out Vector3 spawnPosition, out Quaternion spawnRotation);
                playerController.TrySetTransform(spawnPosition, spawnRotation);
            }
        }
    }

    public override void OnGONetParticipantDeserializeInitAllCompleted(GONetParticipant gonetParticipant)
    {
        base.OnGONetParticipantDeserializeInitAllCompleted(gonetParticipant);

        PlayerLocalContext playerLocalContext = gonetParticipant.GetComponent<PlayerLocalContext>();
        if ((object)playerLocalContext != default)
        {
            // Since gonet does not fire the value changed event when first loading up, we need to do some things manually (TODO get gonet to fire the event after initial deserialize init)
            OnPlayerTypeChanged_ReadyToProcess(playerLocalContext.CurrentPlayerType, playerLocalContext.GetClientOwnerId());
        }
    }

    private void OnBipedRagdollStateChanged(GONetEventEnvelope<SyncEvent_ValueChangeProcessed> eventEnvelope)
    {
        ushort authorityId = GONetMain.gonetParticipantByGONetIdMap[eventEnvelope.Event.GONetId].OwnerAuthorityId;

        //PlayerLocalContext.LookupByAuthorityId(gonetParticipant.OwnerAuthorityId)._isBipedInRagdollState
    }

    private readonly Dictionary<ushort, PlayableEntityType> awaitingReadyToProcess_playerTypeChange = new Dictionary<ushort, PlayableEntityType>();

    private void OnPlayerTypeChanged(GONetEventEnvelope<SyncEvent_ValueChangeProcessed> eventEnvelope)
    {
        PlayableEntityType newValue = (PlayableEntityType)eventEnvelope.Event.ValueNew.System_Byte;
        ushort authorityId = GONetMain.gonetParticipantByGONetIdMap[eventEnvelope.Event.GONetId].OwnerAuthorityId;

        OnPlayerTypeChanged_AttemptProcess(newValue, authorityId);
    }

    private void OnPlayerTypeChanged_AttemptProcess(ushort authorityId)
    {
        if (awaitingReadyToProcess_playerTypeChange.TryGetValue(authorityId, out PlayableEntityType newValue))
        {
            OnPlayerTypeChanged_AttemptProcess(newValue, authorityId);
        }
    }

    private void OnPlayerTypeChanged_AttemptProcess(PlayableEntityType newValue, ushort authorityId)
    {
        // due to order of processing a newly connecting client, the initial values are all processed prior to other necessary actions (like onGNPenables to populate needed data structures)
        if (!_bipedControllersByAuthorityId.TryGetValue(authorityId, out PlayerController playerController) ||
            !_droneControllersByAuthorityId.TryGetValue(authorityId, out DroneController droneController))
        {
            awaitingReadyToProcess_playerTypeChange[authorityId] = newValue;
        }
        else
        {
            OnPlayerTypeChanged_ReadyToProcess(newValue, authorityId);
        }
    }

    private void OnPlayerTypeChanged_ReadyToProcess(PlayableEntityType newValue, ushort authorityId)
    {
        PlayerController playerController = _bipedControllersByAuthorityId[authorityId];
        DroneController droneController = _droneControllersByAuthorityId[authorityId];
        awaitingReadyToProcess_playerTypeChange.Remove(authorityId);

        //Debug.Log($"[DREETS] player type changed to: {newValue} (playerController.id:{playerController.GetNetworkEntityId()}).IsActivated: {playerController.IsActivated} isMine? {authorityId == GONetMain.MyAuthorityId}");

        switch (newValue)
        {
            case PlayableEntityType.Drone:
                playerController.DeactivateEntityControl();
                droneController.ActivateEntityControl();
                break;

            case PlayableEntityType.Biped:
                droneController.DeactivateEntityControl();
                playerController.ActivateEntityControl();
                break;
        }
    }

    private void NetworkTick(float deltaTime)
    {
        if (IsServer)
        {
            _serverHitRegistrator.ProcessShots(_serverSimulator.ActiveNetworkEntitiesByGONetId);
        }

        _entityEventsHolder.PublishCurrentTickEvents();

        if (IsServer)
        {
            _droneLifetimeHandler.Server_Update(deltaTime);
            ++_tick;
        }
        else
        {
            _playerSpawnCountdown.Update(deltaTime);
        }
    }

    public bool Client_IsConnected()
    {
        if (!_isSpawned || !IsClient) return true;

        return GONetMain.GONetClient != default && GONetMain.GONetClient.IsConnectedToServer;
    }

    public ulong GetLocalClientRTT()
    {
        if (!_isSpawned || !IsClient) return 0;

        return (ulong)(1000 * GONetMain.GONetClient.connectionToServer.RTT_Latest);
    }

    public float GetClientRTT(ushort clientId)
    {
        if (!_isSpawned || !IsServer)
        {
            return 0;
        }

        return GONetMain.gonetServer.GetRemoteClientByAuthorityId(clientId).ConnectionToClient.RTTMilliseconds;
    }

    /// <summary>
    /// Adds an entity shot to be processed in the Hit Registration process.
    /// </summary>
    /// <param name="shotConfig">The shooter's shot relevant configuration for the HitReg algorithm</param>
    public void Server_RegisterEntityShot(ShotConfiguration shotConfig)
    {
        _serverHitRegistrator.AddShooter(shotConfig);
    }

    /// <summary>
    /// Creates an entity event associated to a network entity
    /// </summary>
    /// <param name="networkEntityId">The network entity owner of this event</param>
    /// <param name="eventType">The eventType Id of this event</param>
    public void CreateEntityEvent(uint networkEntityId, LocalContextEntityEvent eventType, bool addToEventsWithUnsetGONetId = false)
    {
        if (addToEventsWithUnsetGONetId)
        {
            if (!_entityEventsHolder.entityEventsWithUnsetGONetId.TryGetValue(networkEntityId, out List<LocalContextEntityEvent> entityEvents))
            {
                _entityEventsHolder.entityEventsWithUnsetGONetId[networkEntityId] = entityEvents = new List<LocalContextEntityEvent>();
            }
            entityEvents.Add(eventType);
        }
        else
        {
            _entityEventsHolder.AddEvent(networkEntityId, eventType);
        }
    }

    /// <summary>
    /// Spawns a temporary entity with the purpose of transmitting an event.
    /// </summary>
    /// <param name="position">The world position of the temporary entity</param>
    /// <param name="rotation">The world rotation of the temporary entity</param>
    /// <param name="entityEvent">The entity event associated to this temporary entity</param>
    public void Server_PublishShotEvent(Vector3 position, Quaternion rotation, LocalContextEntityEvent entityEvent)
    {
        if (!IsServer) return;

        entityEvent.vector3 = position;
        entityEvent.quaternion = rotation;

        _entityEventsHolder.AddEvent(entityEvent.GONetParticipantId, entityEvent);
    }

    private void Update()
    {
        if (IsServer)
        {
            Server_ProcessTempEntitiesAwaitingSend();
            Server_AutoDestroyAnyReady();
        }
    }

    private void Server_AutoDestroyAnyReady()
    {
        _server_autoDestroyAfterElapsedSeconds_remove.Clear();
        foreach (var kvp in _server_autoDestroyAfterElapsedSeconds)
        {
            if (GONetMain.Time.ElapsedSeconds > kvp.Value)
            {
                // avoid destroying here since it will cause the removal of the GNP from the dictionary and throw exception whil enumerating here
                _server_autoDestroyAfterElapsedSeconds_remove.Add(kvp.Key);
            }
        }
        
        foreach (GONetParticipant gnp in _server_autoDestroyAfterElapsedSeconds_remove)
        {
            Destroy(gnp.gameObject); // this will auto propogate to all clients
        }
    }

    private void Server_ProcessTempEntitiesAwaitingSend()
    {
        tempEntitiesAwaitingEventSend_remove.Clear();

        foreach ((GONetParticipant tempNetworkEntity, LocalContextEntityEvent entityEvent) tempEntityEvent in tempEntitiesAwaitingEventSend)
        {
            if (!tempEntityEvent.tempNetworkEntity.DoesGONetIdContainAllComponents()) continue;

            tempEntityEvent.entityEvent.GONetParticipantId = tempEntityEvent.tempNetworkEntity.GONetId;
            CreateEntityEvent(tempEntityEvent.tempNetworkEntity.GONetId, tempEntityEvent.entityEvent);

            tempEntitiesAwaitingEventSend_remove.Add(tempEntityEvent);
        }

        tempEntitiesAwaitingEventSend_remove.ForEach(((GONetParticipant gnp, LocalContextEntityEvent @event) x) => {
            tempEntitiesAwaitingEventSend.Remove(x);

            // we CANNOT afford to keep temp GNPs around and they really serve no good purpose...at least anymore...kill em..immediately apparently is not safe..so wait a little!
            Server_AddToAutoDestroyAfterSeconds(x.gnp, 5f);
        });
    }

    public void Server_SpawnObstacleEntity(Vector3 position, Quaternion rotation)
    {
        //GONetLog.Debug($"instantiate at position: {position}");

        GONetParticipant obstacleGONetParticipant = Instantiate(_obstaclePrefab, position, rotation);
        _serverSimulator.networkEntitiesWithUnsetGONetId.Add(obstacleGONetParticipant.gameObject.GetComponent<INetworkEntity>());
    }

    private void OnKilledEntity_Toggleese(GONetEventEnvelope<LocalContextEntityEvent> eventEnvelope)
    {
        ushort authorityId = GONetMain.gonetParticipantByGONetIdMap[eventEnvelope.Event.GONetParticipantId].OwnerAuthorityId;


        PlayerController killedPlayerController = _bipedControllersByAuthorityId[authorityId];
        DroneController killedDroneController = _droneControllersByAuthorityId[authorityId];
        if (killedPlayerController.IsActivated || 
            (eventEnvelope.Event.GONetParticipantId == killedDroneController.GetNetworkEntityId() && killedDroneController.IsActivated))
        {
            Toggleese(authorityId, shouldRagdollBiped: eventEnvelope.Event.type == EntityEventType.EV_KILLED);
        }
        else
        {
            // if the biped was not active, this means the death of the biped should not cause the toggleese, but rather just ragdoll it
            killedPlayerController.ForceActivateRagdoll();
        }
    }

    public void Server_KillEntity(uint networkEntityId)
    {
        if (!IsServer) return;

        INetworkEntity entityToKill;
        bool foundSuccesfully = _serverSimulator.ActiveNetworkEntitiesByGONetId.TryGetValue(networkEntityId, out entityToKill);

        if (foundSuccesfully)
        {
            _serverSimulator.RemoveNetworkEntity(entityToKill.GetNetworkEntityId());
        }
    }

    public void Server_InitiateKillPlayerProcess(uint networkEntityId)
    {
        if (!IsServer) return;

        INetworkEntity entityToKill;
        bool foundSuccesfully = _serverSimulator.ActiveNetworkEntitiesByGONetId.TryGetValue(networkEntityId, out entityToKill);

        if (foundSuccesfully)
        {
            // have to send owning client the kill event to destroy in order for propogation to occur properly

            // we need to tell the player if it will actually die here, or only change color!!!

            bool assUmeIsBipedPlayer = entityToKill is IColorTeamMember;
            if (assUmeIsBipedPlayer) 
            {
                IColorTeamMember colorTeamMember = entityToKill as IColorTeamMember;
                if (_colorTeamAssigner.TryGetColorTeamZoneOfPosition(colorTeamMember.GetPositon(), out byte diedInZoneColor))
                {
                    if (diedInZoneColor == colorTeamMember.GetColorTeam())
                    {
                        _colorTeamAssigner.RemoveMemberFromTeam(entityToKill);
                        byte newColor = _colorTeamAssigner.AssignMemberToTeam(entityToKill, diedInZoneColor);
                        colorTeamMember.Server_SetColorTeam(newColor);

                        INetworkEntity shooterToSwapIntoDrone = _serverHitRegistrator.LastShooterEntityProcessed;
                        if (shooterToSwapIntoDrone is IPlayerNetworkEntity)
                        {
                            //Debug.Log($"Biped killed. isActive? {((IPlayerNetworkEntity)shooterToSwapIntoDrone).IsActivated}");

                            if (_serverSimulator.TryGetPlayerEntityForAuthorityId(shooterToSwapIntoDrone.GetClientOwnerId(), out DroneController droneToSwapInto))
                            {
                                _droneAndBipedSwitcher.Server_StartTrackingDroneIfPossible(droneToSwapInto);
                            }
                            else GONetLog.Error($"Cannot find drone for authority id: {shooterToSwapIntoDrone.GetClientOwnerId()} in order to switch from biped control to drone control!");

                            // send this to all clients!
                            GONetMain.EventBus.Publish(
                                new LocalContextEntityEvent(EntityEventType.EV_ALMOST_KILLED_FORCE_CHANGE_TEAM, shooterToSwapIntoDrone.GetNetworkEntityId()),
                                shouldPublishReliably: true);
                        }

                        if (entityToKill is IDamageable)
                        {
                            IDamageable damageable = entityToKill as IDamageable;
                            //damageable.Server_ResetToMaxHealth();

                            // It looks like the color change will cause PlayerController.OnMyCurrentColorTeamChanged() to fire
                            // and that will reset to max health!
                        }

                        /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        return; // you are only lucki sum bich..live another day my dude//carpe diem!
                        /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    }
                    else
                    { // must have died outside your safe zone fella, now you have to transition to drone control since your biped will die/ragdoll after below action/event
                        if (entityToKill is IPlayerNetworkEntity playerToKill)
                        {
                            //Debug.Log($"Biped killed. isActive? {playerToKill.IsActivated}"); // _bipedControllersByAuthorityId[entityToKill.GetClientOwnerId()].IsActivated

                            // killing the biped entity will cause a switch to the drone, so need to track that for auto switch back to biped
                            if (_serverSimulator.TryGetPlayerEntityForAuthorityId(playerToKill.GetClientOwnerId(), out DroneController droneToSwapInto))
                            {
                                _droneAndBipedSwitcher.Server_StartTrackingDroneIfPossible(droneToSwapInto);
                            }
                            else GONetLog.Error($"Cannot find drone for authority id: {playerToKill.GetClientOwnerId()} in order to switch from biped control to drone control!");
                        }
                    }
                }
            }
            else if (entityToKill is DroneController)
            {
                //_droneAndBipedSwitcher.swit
            }

            // send this to all clients!
            GONetMain.EventBus.Publish(
                new LocalContextEntityEvent(EntityEventType.EV_KILLED, networkEntityId),
                shouldPublishReliably: true);

            // IMPORTANT: the rest of the kill player process (i.e., Server_FinishKillPlayerProcess) will occur when this server
            //            notices that the owning client destroyed the player GNP (i.e., OnGNPDisabled)
            //  WEL this is really not the case after switching to send the above event to all clients and not just the owner
        }
        else GONetLog.Warning($"shot to queue.  entity not found to initiate kill");
    }

    public void Server_FinishKillPlayerProcess(uint networkEntityId)
    {
        if (!IsServer) return;

        INetworkEntity entityToKill;
        bool foundSuccesfully = _serverSimulator.ActiveNetworkEntitiesByGONetId.TryGetValue(networkEntityId, out entityToKill);

        if (foundSuccesfully)
        {
            IPlayerNetworkEntity associatedBiped = null;
            if (entityToKill is DroneController)
            {
                DroneController droneController = entityToKill as DroneController;
                associatedBiped = droneController.AssociatedBipedEntity;
            }

            Server_KillEntity(networkEntityId);

            if (entityToKill is IColorTeamMember)
            {
                _colorTeamAssigner.RemoveMemberFromTeam(entityToKill);
            }
        }
    }

    public GONetRPCHandler GetRPCHandler()
    {
        return _networkRPCHandler;
    }

    internal override void Tick(short uniqueTickHz, double elapsedSeconds, double deltaTime)
    {
        base.Tick(uniqueTickHz, elapsedSeconds, deltaTime);

        if (uniqueTickHz == TICK_RATE_HZ)
        {
            if (!_isSpawned)
            {
                return;
            }

            NetworkTick(TICK_RATE);
        }
    }
}