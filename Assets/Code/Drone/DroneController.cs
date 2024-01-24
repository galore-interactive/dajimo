using GONet;
using System;
using System.Collections;
using Unity.Jobs;
using UnityEngine;

public class DroneController : GONetParticipantCompanionBehaviour, IPlayerNetworkEntity, IDamageable
{
    [SerializeField] private DroneConfigurationSO _configuration;
    [SerializeField] private PlayerClientNoInterpolationGetter _noInterpolationGetterPrefab;
    private PlayerClientNoInterpolationGetter _noInterpolatedComponents;

    private DamageIndicatorController _damageIndicatorController;

    private DroneClientInterpolator _clientInterpolator;

    private Transform _transform;
    private DroneMovement _droneMovement;
    private PlayerCameraMovement _droneCameraMovement;

    private PlayerInputsController _inputsController;
    private DroneInputStateFactory _inputStateFactory;

    private PlayerStateVariables _stateVariables;
    private DronePlayerStateFactory _playerStateFactory;

    [SerializeField] private GameObject _objectBlueprintPrefabToPlace;
    [SerializeField] private float _placeObjectDuration = 2f;
    private ObstaclesCreator _obstaclesCreator;

    [SerializeField] private Transform _cameraTransform;
    private PlayerCameraController _playerCameraController;

    private NetworkController _networkController;
    private GONetRPCHandler _networkRPCHandler;

    private WeakReference<IPlayerNetworkEntity> _associatedBipedEntity;
    public IPlayerNetworkEntity AssociatedBipedEntity => _associatedBipedEntity.TryGetTarget(out var target) ? target : null;
    private bool _server_isPlacingObject;
    private float _server_placingObjectForwardY;
    private Subscription<LocalContextEntityEvent> _server_isPlacingObject_subscription;

    private bool _isActivated;
    public bool IsActivated => _isActivated;
    
    private Renderer[] _renderers;
    private Collider[] _colliders;

    private EntityState _mostRecentEntityState;

    private bool _isSpawned = false;
    public bool IsSpawned => _isSpawned;

    protected override void Awake()
    {
        base.Awake();

        _renderers = GetComponentsInChildren<Renderer>();
        _colliders = GetComponentsInChildren<Collider>();
    }

    /// <summary>
    /// This method is necessary to call it whenever we want to go back to a biped entity once this drone entity despawns.
    /// </summary>
    /// <param name="bipedPlayableNetworkEntity"></param>
    public void SetAssociatedBipedEntity(IPlayerNetworkEntity bipedPlayableNetworkEntity)
    {
        if (_associatedBipedEntity == null)
        {
            _associatedBipedEntity = new WeakReference<IPlayerNetworkEntity>(bipedPlayableNetworkEntity, false);
        }
        else
        {
            _associatedBipedEntity.SetTarget(bipedPlayableNetworkEntity);
        }
    }

    public override void OnGONetParticipantStarted()
    {
        base.OnGONetParticipantStarted();

        _networkController = FindObjectOfType<NetworkController>();
        _stateVariables = new PlayerStateVariables(_networkController, IsServer);
        _transform = transform;
        _noInterpolatedComponents = Instantiate(_noInterpolationGetterPrefab, _transform.position, _transform.rotation);

        _clientInterpolator = new DroneClientInterpolator(_noInterpolatedComponents, _transform, _cameraTransform);

        if (IsServer || gonetParticipant.IsMine)
        {
            _droneMovement = new DroneMovement(_configuration.MovementConfiguration);
            _droneCameraMovement = new PlayerCameraMovement(_configuration.CameraMovementConfiguration);

            _obstaclesCreator = new ObstaclesCreator(_networkController, _noInterpolatedComponents.NoInterpolatedHeadTransform, _objectBlueprintPrefabToPlace, IsServer, 1.5f);

            _playerStateFactory = new DronePlayerStateFactory(_noInterpolatedComponents, _droneMovement, _stateVariables, gonetParticipant, _obstaclesCreator);
        }

        if (gonetParticipant.IsMine)
        {
            _inputsController = new PlayerInputsController(InputsHandlerType.Drone);
            _inputsController.Enable();

            _inputStateFactory = new DroneInputStateFactory(_inputsController, _stateVariables);

            _playerCameraController = FindObjectOfType<PlayerCameraController>();

            _networkRPCHandler = _networkController.GetRPCHandler();

            IsTickReceiver = true;
        }

        if (IsClient || gonetParticipant.IsMine)
        {
            _damageIndicatorController = FindObjectOfType<DamageIndicatorController>();
            _damageIndicatorController.CleanDamageIndicator();
            _damageIndicatorController.SetPlayerTransform(_transform);
        }

        if (IsServer)
        {
            _server_isPlacingObject_subscription = 
                EventBus.Subscribe<LocalContextEntityEvent>(Server_MineIsPlacingObject, 
                    (e) => e.IsSourceRemote && e.SourceAuthorityId == GONetParticipant.OwnerAuthorityId && 
                        e.Event.type == EntityEventType.EV_IS_PLACING_OBSTACLE);
        }

        _isSpawned = true;
    }

    private void Server_MineIsPlacingObject(GONetEventEnvelope<LocalContextEntityEvent> eventEnvelope)
    {
        _server_isPlacingObject = true;
        _server_placingObjectForwardY = BitConverter.ToSingle(eventEnvelope.Event.parameters);
    }

    public override void OnGONetParticipantDisabled()
    {
        if (IsClient && gonetParticipant.IsMine)
        {
            _damageIndicatorController.RemovePlayerTransform();

            //If there is an associated biped entity, return the control to that entity.
            if (_associatedBipedEntity != null)
            {
                bool foundTargetSuccesfully = false;
                if (_associatedBipedEntity.TryGetTarget(out var target))
                {
                    if (target.IsSpawned)
                    {
                        target.ActivateEntityControl();
                        foundTargetSuccesfully = true;
                    }
                }

                if (!foundTargetSuccesfully)
                {
                    _networkController.Client_SwitchToControlPlayerType(PlayableEntityType.Biped);
                }
            }
        }

        if (IsServer || gonetParticipant.IsMine)
        {
            _obstaclesCreator.Disable();

            if (IsServer)
            {
                _server_isPlacingObject_subscription.Unsubscribe();
            }
        } 

        GameObject.Destroy(_noInterpolatedComponents.gameObject);

        _isSpawned = false;

        base.OnGONetParticipantDisabled();
    }

    private void Update()
    {
        if (!_isSpawned || !_isActivated) return;

        if (GONetParticipant.IsMine)
        {
            DroneInputsState input = (DroneInputsState)_inputStateFactory.Create();
            Simulate(input, Time.deltaTime, (float)GONetMain.Time.ElapsedSeconds);

            _clientInterpolator.UpdateNetworkedTransform(); // NOTE: This moves the player/camera
        }
        else if (IsServer)
        {
            DroneInputsState input = new DroneInputsState(Vector3.zero, Vector2.zero, _server_isPlacingObject, default);
            input.placingObjectForwardY = _server_placingObjectForwardY;
            _server_isPlacingObject = false;

            Simulate(input, Time.deltaTime, (float)GONetMain.Time.ElapsedSeconds);

            _clientInterpolator.UpdateFromNetworkedTransform();
        }
    }

    private void UpdateEntityState(Vector3 newPosition, Vector3 cameraLookAtEulerAngles)
    {
        _noInterpolatedComponents.SetNoInterpolatedPlayerPosition(newPosition);
        _noInterpolatedComponents.NoInterpolatedPlayerPositionSource.localEulerAngles = new Vector3(_noInterpolatedComponents.NoInterpolatedPlayerPositionSource.localEulerAngles.x, cameraLookAtEulerAngles.y, _noInterpolatedComponents.NoInterpolatedPlayerPositionSource.localEulerAngles.z);
        _noInterpolatedComponents.NoInterpolatedHeadTransform.rotation = Quaternion.Euler(cameraLookAtEulerAngles.x, _noInterpolatedComponents.NoInterpolatedPlayerPositionSource.localEulerAngles.y, _noInterpolatedComponents.NoInterpolatedPlayerPositionSource.localEulerAngles.z);
    }

    private void NetworkTick(float deltaTime)
    {
        if (!_isSpawned || !GONetParticipant.IsMine) return;

        _stateVariables.SetClientTick(_stateVariables.ClientTick + 1);
    }

    private void SimulateMovement(in DroneInputsState inputState, float deltaTime)
    {
        Vector3 movementDirection = (_noInterpolatedComponents.NoInterpolatedPlayerUp * inputState.movementInput.y) + (_noInterpolatedComponents.NoInterpolatedPlayerRight * inputState.movementInput.x) + (_noInterpolatedComponents.NoInterpolatedPlayerForward * inputState.movementInput.z);
        Vector3 movementVelocity = _droneMovement.SimulateMovement(inputState.movementInput, movementDirection, deltaTime);
        _noInterpolatedComponents.CharacterController.Move(movementVelocity * deltaTime);
    }

    private void SimulateCameraMovement(in DroneInputsState inputState, float deltaTime)
    {
        float verticalRotation = _noInterpolatedComponents.NoInterpolatedHeadTransform.rotation.eulerAngles.x;
        while (verticalRotation > 180) verticalRotation -= 360; // this ensures the drone can look up past the horizon!

        Vector2 rotation = _droneCameraMovement.GetUpdatedRotationApplyingInput(inputState.mouseMovementInput, _noInterpolatedComponents.NoInterpolatedPlayerPositionSource.localEulerAngles.y, verticalRotation, deltaTime);
        _noInterpolatedComponents.NoInterpolatedPlayerPositionSource.localEulerAngles = new Vector3(_noInterpolatedComponents.NoInterpolatedPlayerPositionSource.localEulerAngles.x, rotation.x, _noInterpolatedComponents.NoInterpolatedPlayerPositionSource.localEulerAngles.z);
        _noInterpolatedComponents.NoInterpolatedHeadTransform.rotation = Quaternion.Euler(rotation.y, _noInterpolatedComponents.NoInterpolatedPlayerPositionSource.localEulerAngles.y, _noInterpolatedComponents.NoInterpolatedPlayerPositionSource.localEulerAngles.z);
    }

    private void SimulateObstaclePlacement(bool shouldStartPlacingObjectNow, float deltaTime, float placingObjectForwardY = 0)
    {
        _obstaclesCreator.Simulate(shouldStartPlacingObjectNow, deltaTime, placingObjectForwardY);
    }

    #region IPlayerNetworkEntity methods
    public void Simulate(I_InputState inputState, float deltaTime, float serverElapsedSeconds)
    {
        if (!_isSpawned || inputState.EntityType != PlayableEntityType.Drone) return;

        if (GONetParticipant.IsMine)
        {
            Mine_ApplyPlayerInput((DroneInputsState)inputState, deltaTime);
        }
        else if (IsServer)
        {
            DroneInputsState droneInput = (DroneInputsState)inputState;
            SimulateObstaclePlacement(droneInput.isPlacingObject, deltaTime, droneInput.placingObjectForwardY);
        }
    }

    private void Mine_ApplyPlayerInput(DroneInputsState inputState, float deltaTime)
    {
        _stateVariables.SetClientTick(inputState.ClientTick);

        SimulateMovement(inputState, deltaTime);
        SimulateCameraMovement(inputState, deltaTime);
        SimulateObstaclePlacement(inputState.isPlacingObject, deltaTime);
        
        if (inputState.isPlacingObject)
        {
            // let server know placing object since it spawns there
            EventBus.Publish(
                new LocalContextEntityEvent(
                    EntityEventType.EV_IS_PLACING_OBSTACLE, 
                    hasParameters: true, 
                    parameters: BitConverter.GetBytes(_noInterpolatedComponents.NoInterpolatedHeadTransform.forward.y), // TODO FIXME...this value represents where forward is at time of staring placement...really need it to be end of placement!  so, just need to network the splitRotation thing here just like biped/player
                    gonetParticipant.GONetId), 
                shouldPublishReliably: true);
        }
    }

    public IPlayerState GetPlayerState()
    {
        return _playerStateFactory.CreateSpecific();
    }

    public void ReceivePlayerState(IPlayerState playerState) { }

    public void ReceiveEntityState(EntityState entityState)
    {
        if (!_isSpawned) return;

        _mostRecentEntityState = entityState;
    }

    public void ReceiveEntityEvent(LocalContextEntityEvent entityEvent)
    {
        //throw new System.NotImplementedException();
    }

    public EntityState GetEntityStateWithoutEvent()
    {
        DronePlayerState playerState = _playerStateFactory.CreateSpecific();
        return new EntityState(playerState.networkObjectID, playerState.movementInput, playerState.position, playerState.cameraLookAtEulerAngles, Vector3.zero, false, false);
    }

    public ushort GetClientOwnerId()
    {
        return GONetParticipant.OwnerAuthorityId;
    }

    public bool IsPlayer()
    {
        return true;
    }

    public void Simulate(float elapsedTime, float serverTime)
    {
        throw new System.NotImplementedException();
    }

    public bool IsRewindable()
    {
        return true;
    }

    public GONetParticipant GetNetworkObject()
    {
        return gonetParticipant;
    }

    public bool DoReceiveEntityStates()
    {
        return true;
    }

    public uint GetNetworkEntityId()
    {
        return gonetParticipant.GONetId;
    }
    #endregion

    internal override void Tick(short uniqueTickHz, double elapsedSeconds, double deltaTime)
    {
        base.Tick(uniqueTickHz, elapsedSeconds, deltaTime);

        if (uniqueTickHz == NetworkController.TICK_RATE_HZ)
        {
            if (!_isSpawned || !IsClient)
            {
                return;
            }
            NetworkTick(NetworkController.TICK_RATE);
        }
    }

    public void Server_SetCollidersVisibility(bool visibility)
    {
        throw new System.NotImplementedException();
    }

    public void Server_TakeDamage(int damage)
    {
        if (IsClient)
        {
            _playerCameraController.ShakeCamera(0.5f);
        }

        _obstaclesCreator.CancelPlacement();
    }

    public PlayableEntityType GetPlayableEntityType()
    {
        return PlayableEntityType.Drone;
    }

    public void ActivateEntityControl()
    {
        if (_isSpawned)
        {
            ActivateEntityControl_Internal();
        }
        else
        {
            StartCoroutine(ActivateEntityControl_Coroutine());
        }
    }

    private IEnumerator ActivateEntityControl_Coroutine()
    {
        while (!_isSpawned)
        {
            yield return null;
        }

        ActivateEntityControl_Internal();
    }


    private void ActivateEntityControl_Internal()
    {
        //GONetLog.Debug("drone activated");
        _isActivated = true;
        
        if (IsClient) foreach (var renderer in _renderers) renderer.enabled = true;
        if (IsClient) foreach (var collider in _colliders) collider.enabled = true;

        if (IsMine)
        {
            _playerCameraController.SetCameraTarget(_cameraTransform);
            _playerCameraController.SetCameraAsMain();
        }
    }

    public void DeactivateEntityControl()
    {
        //GONetLog.Debug("drone DE-activated");
        _isActivated = false;
        
        if (IsClient) foreach (var renderer in _renderers) renderer.enabled = false;
        if (IsClient) foreach (var collider in _colliders) collider.enabled = false;
   }

    public void ResetAbove(Transform aboveTransform)
    {
        transform.position = 
            _noInterpolatedComponents.transform.position = 
                aboveTransform.position + new Vector3(0, DummyDroneFollow.FOLLOW_ABOVE_DISTANCE, 0);

        transform.rotation =
            _noInterpolatedComponents.transform.rotation =
                _noInterpolatedComponents.NoInterpolatedHeadTransform.rotation =
                    aboveTransform.rotation;
    }
}
