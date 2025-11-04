using System;
using UnityEngine;
using GONet;
using System.Collections;
using DATS;

public class OwnedRotation<T>
{
    public T Owner { get; private set; }
    public Quaternion rotation;

    public OwnedRotation(T owner, Quaternion rotation = default)
    {
        Owner = owner;
        this.rotation = rotation;
    }
}

[DisallowMultipleComponent]
public class PlayerController : GONetParticipantCompanionBehaviour, IPlayerNetworkEntity, IDamageable, IColorTeamMember
{
    public event Action<BipedPlayerState> OnPlayerStateChanged;

    private EntityState _mostRecentEntityState;

    [SerializeField] private PlayerConfigurationSO _configuration;
    [SerializeField] private PlayerClientNoInterpolationGetter _noInterpolatedComponentsPrefab;
    private PlayerClientNoInterpolationGetter _noInterpolatedComponents;

    [SerializeField] private Renderer _onlyArmsVisuals;
    private Renderer _fullBodyVisuals;

    private OwnedRotation<PlayerController> splitRotationHolder;
    [GONetAutoMagicalSync("_GONet_Transform_Rotation")] public Quaternion splitRotation;

    private PlayerMovement _playerMovement;

    private PlayerInputsController _inputsController;

    private PlayerStateVariables _stateVariables;

    [SerializeField] private Transform _headTransform;
    [SerializeField] private Transform _cameraFollowTransform;
    private PlayerCameraMovement _cameraMovement;
    private PlayerCameraController _cameraController;

    private CrouchStandController _crouchStandController;

    private BipedPlayerStateFactory _playerStateFactory;
    private BipedInputStateFactory _inputStateFactory;
    private PlayerClientInterpolator _clientInterpolator;
    private GONetRPCHandler _networkRPCHandler;

    [SerializeField] private Animator _onlyArmsAnimator;
    private PlayerAnimationController _armsOnlyAnimationsController;

    private PlayerFullBodyAnimatorController _fullBodyAnimationsController;

    [SerializeField] private Transform _weaponsHolderTransform;
    [SerializeField] private Transform _weaponBarrelEndTransform;
    [SerializeField] private WeaponComponent[] _initialWeaponComponents;
    private WeaponsHandler _weaponsHandler;
    private AimingController _aimingController;
    private PlayerWeaponRecoilController _weaponRecoilController;

    private SmashController _smashController;

    private NetworkController _networkController;

    private AudioSource _audioSource;
    [SerializeField] private AudioClip _shotSound;
    [SerializeField] private AudioClip _smashSound;

    private readonly Health _health = new Health(100);
    [SerializeField] private FullBodyPlayerController _fullBodyPlayerPrefab;
    private FullBodyPlayerController _fullBodyPlayerController;

    private DamageIndicatorController _damageIndicatorController;
    private CrosshairPresenter _crosshairPresenter;

    public byte currentTeamColor
    {
        get => PlayerLocalContext.LookupByAuthorityId(gonetParticipant.OwnerAuthorityId).currentTeamColor;
        set => PlayerLocalContext.LookupByAuthorityId(gonetParticipant.OwnerAuthorityId).currentTeamColor = value;
    }

    [SerializeField] private DummyDroneFollow _dummyDronePrefab;
    private DummyDroneFollow _dummyDroneFollow;

    private SmashHitMarker _smashHitMarker;

    //Is the object being input controlled by the local player?
    private bool _isActivated;
    public bool IsActivated => _isActivated;

    //Is the entity spawned with a valid GNP?
    private bool _isSpawned = false;
    public bool IsSpawned => _isSpawned;

    public bool HasUnprocessedRagdoll { get; internal set; }

    protected override void Awake()
    {
        base.Awake();

        GONetLog.Info($"[PlayerController.Awake] GameObject: {gameObject.name}, GONetParticipant exists: {gonetParticipant != null}");
        _audioSource = GetComponent<AudioSource>();
        splitRotationHolder = new OwnedRotation<PlayerController>(this, splitRotation);
    }

    public override void OnGONetReady(GONetParticipant gonetParticipant)
    {
        base.OnGONetReady(gonetParticipant);

        GONetLog.Info($"[PlayerController.OnGONetReady] Called with participant GONetId: {gonetParticipant?.GONetId}, this.gonetParticipant GONetId: {this.gonetParticipant?.GONetId}, Match: {gonetParticipant == this.gonetParticipant}, GONetId Match: {gonetParticipant?.GONetId == this.gonetParticipant?.GONetId}");

        // This is called for ALL participants (own and remote)
        // Only initialize THIS PlayerController if it's for THIS participant
        // Use GONetId comparison instead of reference equality for IL2CPP compatibility
        if (gonetParticipant == null || this.gonetParticipant == null)
        {
            GONetLog.Warning($"[PlayerController.OnGONetReady] Null participant detected - gonetParticipant null: {gonetParticipant == null}, this.gonetParticipant null: {this.gonetParticipant == null}");
            return;
        }

        if (gonetParticipant.GONetId != this.gonetParticipant.GONetId)
        {
            GONetLog.Info($"[PlayerController.OnGONetReady] Skipping - not for this PlayerController's participant");
            return; // This PlayerController is not for this participant, skip
        }

        // Continue with normal initialization for OUR participant
        GONetLog.Info($"[PlayerController.OnGONetReady] Proceeding with initialization for THIS participant");

        try
        {
            OnGONetReady_Internal();
            GONetLog.Info($"[PlayerController.OnGONetReady] Successfully completed initialization for GONetId: {gonetParticipant.GONetId}");
        }
        catch (System.Exception ex)
        {
            GONetLog.Error($"[PlayerController.OnGONetReady] Exception during initialization: {ex.Message}\n{ex.StackTrace}");
        }
    }

    /// <summary>
    /// Internal initialization called from OnGONetReady when this component's participant is ready
    /// </summary>
    private void OnGONetReady_Internal()
    {
        if (GONetParticipant.IsMine)
        {
            ShotProcessor.SetMyLocalPlayerGNP(GONetParticipant);
        }

        _networkController = FindObjectOfType<NetworkController>();
        _stateVariables = new PlayerStateVariables(_networkController, IsServer);

        _noInterpolatedComponents = Instantiate(_noInterpolatedComponentsPrefab, transform.position, transform.rotation);
        _noInterpolatedComponents.NoInterpolatedSplitRotationSource = splitRotationHolder;

        _fullBodyPlayerController = Instantiate(_fullBodyPlayerPrefab, new Vector3(0f, -1.85f, 0f), Quaternion.identity);
        _fullBodyPlayerController.SetTransform(_noInterpolatedComponents.NoInterpolatedPlayerPositionSource);
        _fullBodyPlayerController.SetCorrelationData(this);
        _fullBodyVisuals = _fullBodyPlayerController.GetComponentInChildren<Renderer>();

        if (IsClient && GONetParticipant.IsMine)
        {
            _damageIndicatorController = FindObjectOfType<DamageIndicatorController>(true);
            _damageIndicatorController.CleanDamageIndicator();
            _crosshairPresenter = FindObjectOfType<CrosshairPresenter>(true);

            _damageIndicatorController.SetPlayerTransform(this.transform);

            _crosshairPresenter.SetPlayerController(this);
            _crosshairPresenter.EnableCrosshairUpdate();

            _smashHitMarker = FindObjectOfType<SmashHitMarker>(true);
        }

        if (GONetParticipant.IsMine)
        {
            _armsOnlyAnimationsController = new PlayerAnimationController(_onlyArmsAnimator);
        }

        if (GONetParticipant.IsMine || IsServer)
        {
            RaycastShooter raycastShooter = FindObjectOfType<RaycastShooter>(true);
            _crouchStandController = new CrouchStandController(_noInterpolatedComponents.CharacterController, _configuration.CrouchStandConfiguration, _stateVariables, _noInterpolatedComponents.NoInterpolatedHeadTransform);
            _aimingController = new AimingController(_stateVariables);
            _weaponRecoilController = new PlayerWeaponRecoilController();
            _cameraMovement = new PlayerCameraMovement(_configuration.CameraMovementConfiguration);
            _playerMovement = new PlayerMovement(_configuration.MovementConfiguration, _stateVariables);
            _weaponsHandler = new WeaponsHandler(_weaponsHolderTransform, _weaponBarrelEndTransform, GetWeaponEnableConfiguration(),
                                                 raycastShooter, GONetParticipant, _initialWeaponComponents);
            _smashController = new SmashController(raycastShooter, _stateVariables, _noInterpolatedComponents.NoInterpolatedHeadTransform, GONetParticipant, 1.5f, 0.3f);
            _playerStateFactory = new BipedPlayerStateFactory(_noInterpolatedComponents, _playerMovement, _weaponRecoilController, _stateVariables, _weaponsHandler, _crouchStandController, _smashController, GONetParticipant);

            _aimingController.OnAimIn += OnAimInEvents;
            _aimingController.OnAimOut += OnAimOutEvents;
            _weaponsHandler.OnShotPerformed += OnShotPerformed;
            _weaponsHandler.OnWeaponEquiped += OnEquipWeapon;
            _weaponsHandler.OnStartReload += OnStartReload;
            _weaponsHandler.OnEndReload += OnEndReload;
            _smashController.OnSmashPerformed += OnSmashPerformed;

            _weaponsHandler.Start();
        }

        if (IsServer)
        {
            _fullBodyPlayerController.SetDamageableEntityToParts(this);
            _fullBodyPlayerController.SetNetworkEntityIDToDamageableParts(GetNetworkEntityId());

            _health.OnKilled += Server_OnKilled;
        }

        _fullBodyAnimationsController = new PlayerFullBodyAnimatorController(_fullBodyPlayerController.FullBodyAnimator);
        _clientInterpolator = new PlayerClientInterpolator(
            interpolationSpeed: GONetParticipant.IsMine ? 20 : int.MaxValue,
            noInterpolationGetter: _noInterpolatedComponents,
            networkedTransform: transform,
            networkedHeadTransform: _headTransform,
            networkedSplitRotationHolder: splitRotationHolder,
            fullBodyAnimationsController: _fullBodyAnimationsController);

        GONetLog.Info($"[PlayerController.OnGONetReady] GONetId: {GONetParticipant.GONetId}, OwnerAuthorityId: {GONetParticipant.OwnerAuthorityId}, IsMine: {GONetParticipant.IsMine}, MyAuthorityId: {GONetMain.MyAuthorityId}");

        if (GONetParticipant.IsMine)
        {
            GONetLog.Info($"[PlayerController] Setting FIRST-PERSON view (arms only) for GONetId: {GONetParticipant.GONetId}");
            SetFullBodyVisualsVisibility(false);
            SetOnlyArmsVisualsVisibility(true);

            _cameraController = FindObjectOfType<PlayerCameraController>();
            GONetLog.Debug($"[PlayerController] Found camera controller: {_cameraController != null}, using NoInterpolatedHeadTransform: {_noInterpolatedComponents.NoInterpolatedHeadTransform?.name}");
            _cameraController.SetCameraTarget(_noInterpolatedComponents.NoInterpolatedHeadTransform);
            _cameraController.SetCameraAsMain();

            _inputsController = new PlayerInputsController(InputsHandlerType.Player);
            EnableInputs();
            _inputStateFactory = new BipedInputStateFactory(_inputsController, _stateVariables);

            _networkRPCHandler = _networkController.GetRPCHandler();

            IsTickReceiver = true;
        }
        else
        {
            GONetLog.Info($"[PlayerController] Setting THIRD-PERSON view (full body) for GONetId: {GONetParticipant.GONetId}");
            SetFullBodyVisualsVisibility(true);
            SetOnlyArmsVisualsVisibility(false);
        }

        if (IsClient && !GONetParticipant.IsMine)
        {
            if (_initialWeaponComponents.Length > 0)
            {
                GameObject.Instantiate(_initialWeaponComponents[0], _fullBodyPlayerController.WeaponsHolderTransform);
            }
        }

        if (IsClient && GONetParticipant.IsMine)
        {
            // Initialize color for local player - GONetLocal is guaranteed to be in lookup
            Client_InitColor();
        }

        if (IsClient)
        {
            _dummyDroneFollow = GameObject.Instantiate(_dummyDronePrefab, _noInterpolatedComponents.NoInterpolatedPlayerPosition + new Vector3(0, DummyDroneFollow.FOLLOW_ABOVE_DISTANCE, 0), Quaternion.identity);
            _dummyDroneFollow.SetFollowTransform(_noInterpolatedComponents.NoInterpolatedPlayerPositionSource);
        }

        // For remote players, color will be initialized via sync event when PlayerLocalContext becomes available
        if (IsClient && !GONetParticipant.IsMine)
        {
            StartCoroutine(WaitForRemotePlayerColor());
        }

        _isActivated = true;
        _isSpawned = true;
    }

    /// <summary>
    /// Fallback mechanism to ensure initialization happens even if OnGONetReady never fires
    /// (can happen in IL2CPP builds where GONet's callback system may not work reliably)
    /// </summary>
    private IEnumerator FallbackInitializationCheck()
    {
        // Wait 2 seconds for normal initialization
        yield return new WaitForSeconds(2f);

        GONetLog.Info($"[PlayerController.FallbackCheck] Initial check - GONetId: {gonetParticipant?.GONetId}, _isSpawned: {_isSpawned}, IsReady: {(gonetParticipant != null ? GONetMain.IsGONetReady(gonetParticipant).ToString() : "null")}");

        // If still not spawned, wait for participant to be ready and force initialization
        if (!_isSpawned && gonetParticipant != null)
        {
            GONetLog.Info($"[PlayerController.FallbackCheck] Entering wait loop for GONetId: {gonetParticipant.GONetId}");

            // Wait up to 10 more seconds for participant to be ready
            float waitTime = 0f;
            while (gonetParticipant != null && !GONetMain.IsGONetReady(gonetParticipant) && waitTime < 10f)
            {
                if (waitTime == 0f || waitTime % 2f == 0f) // Log every 2 seconds
                {
                    GONetLog.Info($"[PlayerController.FallbackCheck] Still waiting... {waitTime}s elapsed, participant ready: {GONetMain.IsGONetReady(gonetParticipant)}");
                }
                yield return new WaitForSeconds(0.5f);
                waitTime += 0.5f;
            }

            if (gonetParticipant == null)
            {
                GONetLog.Error($"[PlayerController.FallbackCheck] Participant became null during wait!");
                yield break;
            }

            GONetLog.Info($"[PlayerController.FallbackCheck] Finished waiting - {waitTime}s elapsed, GONetId: {gonetParticipant.GONetId}, _isSpawned: {_isSpawned}, IsReady: {GONetMain.IsGONetReady(gonetParticipant)}");

            if (!_isSpawned && GONetMain.IsGONetReady(gonetParticipant))
            {
                GONetLog.Warning($"[PlayerController] FALLBACK: Forcing initialization for GONetId: {gonetParticipant.GONetId}, IsMine: {gonetParticipant.IsMine}");
                OnGONetReady_Internal();
                _isSpawned = true;
            }
            else if (!_isSpawned && !GONetMain.IsGONetReady(gonetParticipant))
            {
                GONetLog.Error($"[PlayerController.FallbackCheck] Participant {gonetParticipant.GONetId} still not ready after {waitTime}s! Giving up.");
            }
            else if (_isSpawned)
            {
                GONetLog.Info($"[PlayerController.FallbackCheck] Was spawned during wait for GONetId: {gonetParticipant.GONetId}");
            }
        }
        else if (_isSpawned)
        {
            GONetLog.Info($"[PlayerController.FallbackCheck] Already spawned, no fallback needed for GONetId: {gonetParticipant?.GONetId}");
        }
        else if (gonetParticipant == null)
        {
            GONetLog.Error($"[PlayerController.FallbackCheck] Participant is null!");
        }
    }

    /// <summary>
    /// Waits for remote player's PlayerLocalContext to be available, then initializes color.
    /// This handles the case where a late-joining client sees an existing player before their
    /// PlayerLocalContext is fully initialized.
    /// </summary>
    private IEnumerator WaitForRemotePlayerColor()
    {
        PlayerLocalContext remoteContext = null;
        int attempts = 0;
        const int maxAttempts = 300; // 5 seconds at 60fps

        while (remoteContext == null && attempts < maxAttempts)
        {
            remoteContext = PlayerLocalContext.LookupByAuthorityId(gonetParticipant.OwnerAuthorityId);
            if (remoteContext == null)
            {
                attempts++;
                yield return null;
            }
        }

        if (remoteContext != null && _fullBodyPlayerController != null)
        {
            // Now we can safely get the color
            Color teamColor = _networkController.ColorTeamAssigner.GetColor(remoteContext.currentTeamColor);
            _fullBodyPlayerController.SetColorTeam(teamColor);
            //GONetLog.Debug($"[Remote Player] Set color for authorityId {gonetParticipant.OwnerAuthorityId}: {teamColor}");
        }
        else
        {
            GONetLog.Warning($"[Remote Player] Failed to initialize color for authorityId {gonetParticipant.OwnerAuthorityId} after {maxAttempts} attempts");
        }
    }

    /// <summary>
    /// Initializes the player's color based on their team assignment.
    /// Called from OnGONetReady() which guarantees PlayerLocalContext is available in lookup for local player.
    /// </summary>
    private void Client_InitColor()
    {
        Color teamColor = _networkController.ColorTeamAssigner.GetColor(currentTeamColor);
        //GONetLog.Debug($"setting full body color: {teamColor}, isMine? {GONetParticipant.IsMine}");
        _fullBodyPlayerController.SetColorTeam(teamColor);
    }

    public override void OnGONetParticipantEnabled()
    {
        base.OnGONetParticipantEnabled();

        OnGONetParticipantEnabled_KilledToggleControl_On();
    }

    private void OnGONetParticipantEnabled_KilledToggleControl_On()
    {
        //_onCurrentTeamColorChangedSubscription = GONetMain.EventBus.Subscribe(
        //SyncEvent_GeneratedTypes.SyncEvent_PlayerController_currentTeamColor, OnMyCurrentColorTeamChanged, (e) => e.GONetParticipant == gonetParticipant);
        //SyncEvent_GeneratedTypes.SyncEvent_PlayerController_currentTeamColor, OnMyCurrentColorTeamChanged);

        GONetMain.EventBus.Subscribe(
            SyncEvent_GeneratedTypes.SyncEvent_GONetParticipant_GONetId, OnMyGONetParticipantIdChanged, (e) => e.GONetParticipant == gonetParticipant);

        GONetMain.EventBus.Subscribe<LocalContextEntityEvent>(Remote_OnEntityEvent, (e) => e.IsSourceRemote);

        GONetMain.EventBus.Subscribe(
            //SyncEvent_GeneratedTypes.SyncEvent_PlayerController_currentTeamColor, OnMyCurrentColorTeamChanged, (e) => e.GONetParticipant == gonetParticipant);
            SyncEvent_GeneratedTypes.SyncEvent_PlayerLocalContext_currentTeamColor, OnMyCurrentColorTeamChanged);
    }

    private void Remote_OnEntityEvent(GONetEventEnvelope<LocalContextEntityEvent> eventEnvelope)
    {
        if (IsServer)
        {
            switch (eventEnvelope.Event.type)
            {
                case EntityEventType.EV_FIRE_WEAPON:
                    if (eventEnvelope.GONetParticipant.OwnerAuthorityId == gonetParticipant.OwnerAuthorityId)
                    {
                        Server_Shoot();
                    }
                    break;
            }
        }
    }

    private void OnMyGONetParticipantIdChanged(GONetEventEnvelope<SyncEvent_ValueChangeProcessed> eventEnvelope)
    {
        _fullBodyPlayerController.SetNetworkEntityIDToDamageableParts(GetNetworkEntityId());
    }

    private WeaponComponentsEnableConfiguration GetWeaponEnableConfiguration()
    {
        if(GONetParticipant.IsMine || IsClient)
        {
            return new WeaponComponentsEnableConfiguration(true);
        }
        else
        {
            return new WeaponComponentsEnableConfiguration(false);
        }
    }

    private void SetFullBodyVisualsVisibility(bool visibility)
    {
        _fullBodyVisuals.enabled = visibility;
    }

    private void SetOnlyArmsVisualsVisibility(bool visibility)
    {
        _onlyArmsVisuals.enabled = visibility;
    }

    public override void OnGONetParticipantDisabled()
    {
        if (GONetParticipant.IsMine)
        {
            ShotProcessor.SetMyLocalPlayerGNP(null);
        }

        if (GONetParticipant.IsMine || IsServer)
        {
            _aimingController.OnAimIn -= OnAimInEvents;
            _aimingController.OnAimOut -= OnAimOutEvents;
            _weaponsHandler.OnShotPerformed -= OnShotPerformed;
            _weaponsHandler.OnWeaponEquiped -= OnEquipWeapon;
            _weaponsHandler.OnStartReload -= OnStartReload;
            _weaponsHandler.OnEndReload -= OnEndReload;
        }

        if (IsServer)
        {
            _health.OnKilled -= Server_OnKilled;
        }

        if (IsClient)
        {
            Destroy(_dummyDroneFollow.gameObject);
        }

        if (IsClient && GONetParticipant.IsMine)
        {
            _damageIndicatorController.RemovePlayerTransform();
        }

        Destroy(_noInterpolatedComponents.gameObject);
        Destroy(_fullBodyPlayerController.gameObject);

        _isSpawned = false;

        base.OnGONetParticipantDisabled();
    }

    internal void ForceActivateRagdoll()
    {
        HasUnprocessedRagdoll = true;
        OnGONetParticipantDisabled_KilledToggleControl_Off();
    }

    private void OnGONetParticipantDisabled_KilledToggleControl_Off()
    {
        if (HasUnprocessedRagdoll)
        {
            _fullBodyPlayerController.ActivateRagdoll();
            HasUnprocessedRagdoll = false;

            if (IsMine)
            {
                PlayerLocalContext.Mine._isBipedInRagdollState = true;
            }
        } // else the assumption is this biped is not killed but rather should remain upright while its drone is in use for a while
    }

    public void EnableInputs()
    {
        _inputsController.Enable();
    }

    public void DisableInputs()
    {
        _inputsController.Disable();
    }

    #region Simulation Methods
    private void Mine_ApplyPlayerInput(BipedInputState inputState, float deltaTime)
    {
        _stateVariables.SetClientTick(inputState.clientTick);

        _weaponsHandler.UpdateActiveWeapon(deltaTime);
        SimulateReloading(inputState);
        SimulateShooting(inputState);
        SimulateAiming(inputState);
        SimulateSmash(inputState, deltaTime);
        SimulateMovement(inputState, deltaTime);
        SimulateCameraMovement(inputState, deltaTime);
        SimulateRecoil(deltaTime);
        SimulateCrouchStand(inputState, deltaTime);

        OnPlayerStateChanged?.Invoke(_playerStateFactory.CreateSpecific());
    }

    private void SimulateShooting(BipedInputState inputState)
    {
        _stateVariables.SetIsShooting(false);
        
        if (inputState.isShooting && !_stateVariables.IsReloading)
        {
            _weaponsHandler.Shoot();
        }
    }

    void Server_Shoot()
    {
        _weaponsHandler.Server_Shoot();
    }

    private void SimulateAiming(BipedInputState inputState)
    {
        _aimingController.UpdateAiming(inputState.isAiming);
    }

    private void SimulateReloading(BipedInputState inputState)
    {
        if (inputState.isReloading && !_stateVariables.IsReloading)
        {
            _weaponsHandler.Reload();
        }
    }

    private void SimulateSmash(BipedInputState inputState, float deltaTime)
    {
        if (!_stateVariables.IsReloading)
        {
            _smashController.Simulate(inputState.isSmashing, deltaTime);
        }
    }

    private void SimulateMovement(BipedInputState inputState, float deltaTime)
    {
        Vector3 movementDirection = (_noInterpolatedComponents.NoInterpolatedPlayerForward * inputState.movementInput.y) + (_noInterpolatedComponents.NoInterpolatedPlayerRight * inputState.movementInput.x);
        Vector3 movementVelocity = _playerMovement.Simulate(inputState.movementInput, movementDirection, inputState.isRunning, _noInterpolatedComponents.CharacterController.isGrounded, inputState.isShooting, deltaTime);
        _noInterpolatedComponents.CharacterController.Move(movementVelocity * deltaTime);
    }

    private void SimulateCameraMovement(BipedInputState inputState, float deltaTime)
    {
        Vector2 currentRotation = new Vector2(_noInterpolatedComponents.rotationHorizontal, _noInterpolatedComponents.rotationVertical);

        Vector2 updatedRotation = _cameraMovement.GetUpdatedRotationApplyingInput(
            inputState.cameraMovementInput, 
            currentRotation.x, 
            currentRotation.y, 
            deltaTime);
        
        _noInterpolatedComponents.NoInterpolatedSplitRotationSource.rotation = 
            Quaternion.Euler(
                updatedRotation.y, 
                updatedRotation.x, 
                0);

        _noInterpolatedComponents.NoInterpolatedPlayerPositionSource.rotation = Quaternion.Euler(0, updatedRotation.x, 0);
        _noInterpolatedComponents.NoInterpolatedHeadTransform.localRotation = Quaternion.Euler(updatedRotation.y, 0, 0);

        _noInterpolatedComponents.rotationHorizontal = updatedRotation.x;
        _noInterpolatedComponents.rotationVertical = updatedRotation.y;
    }

    private void SimulateRecoil(float deltaTime)
    {
        /* TODO FIXME
        Vector3 resultRotation = _weaponRecoilController.UpdateRecoil(new Vector3(_noInterpolatedComponents.NoInterpolatedSplitRotationSource.localEulerAngles.x, _noInterpolatedComponents.NoInterpolatedPlayerLocalEulerAngles.y, 0), elapsedTime);
        _noInterpolatedComponents.SetNoInterpolatedPlayerLocalEulerAngles(new Vector3(_noInterpolatedComponents.NoInterpolatedPlayerLocalEulerAngles.x, resultRotation.y, _noInterpolatedComponents.NoInterpolatedPlayerLocalEulerAngles.z));
        _noInterpolatedComponents.NoInterpolatedSplitRotationSource.localEulerAngles = new Vector3(resultRotation.x, _noInterpolatedComponents.NoInterpolatedSplitRotationSource.localEulerAngles.y, _noInterpolatedComponents.NoInterpolatedSplitRotationSource.localEulerAngles.z);
        */
    }

    private void SimulateCrouchStand(BipedInputState inputState, float deltaTime)
    {
        _crouchStandController.Simulate(inputState.isCrouchingOrStandingUp, deltaTime);
    }
    #endregion

    #region IPlayerNetworkEntity Methods
    public void Simulate(I_InputState inputState, float deltaTime, float serverElapsedSeconds)
    {
        if (!_isSpawned || inputState.EntityType != PlayableEntityType.Biped) return;

        if (GONetParticipant.IsMine)
        {
            BipedInputState bipedInputState = (BipedInputState)inputState;
            Mine_ApplyPlayerInput(bipedInputState, deltaTime);
        }
    }

    public IPlayerState GetPlayerState()
    {
        return _playerStateFactory.Create();
    }

    public ushort GetClientOwnerId()
    {
        return GONetParticipant.OwnerAuthorityId;
    }

    public bool IsPlayer()
    {
        return true;
    }

    public void Simulate(float deltaTime, float serverTime)
    {
        throw new NotImplementedException();
    }

    public void ReceiveEntityState(EntityState entityState)
    {
        if (!_isSpawned)
        {
            return;
        }

        _mostRecentEntityState = entityState;
    }

    public void ReceiveEntityEvent(LocalContextEntityEvent entityEvent)
    {
        switch (entityEvent.type)
        {
            case EntityEventType.EV_COLOR_ASSIGNED:
                byte colorByte = entityEvent.parameters[0];
                Color teamColor = ColorTeamConfiguration.ColorByIndex[colorByte];
                GONetLog.Debug($"SERVER told me to set my currentTeamColor to: {teamColor}  my pc.GONetId: {this.gonetParticipant.GONetId} id all components? {this.gonetParticipant.DoesGONetIdContainAllComponents()}");
                currentTeamColor = colorByte; // this will then get networked to all others
                _fullBodyPlayerController.SetColorTeam(teamColor);
                _onlyArmsVisuals.material.color = teamColor;
                break;
        }

        if (!IsClient || gonetParticipant.IsMine) return;

        PerformEntityEvent(entityEvent, out bool isShooting, out bool isReloading, out bool isSmashing);
    }

    public EntityState GetEntityStateWithoutEvent()
    {
        BipedPlayerState playerState = _playerStateFactory.CreateSpecific();
        return new EntityState(playerState.networkObjectID, playerState.movementInput, playerState.position, playerState.cameraLookAtEulerAngles, playerState.velocityVector, playerState.isGrounded, playerState.isCrouched);
    }

    public uint GetNetworkEntityId()
    {
        return GONetParticipant.GONetId;
    }

    public GONetParticipant GetNetworkObject()
    {
        return GONetParticipant;
    }

    public bool DoReceiveEntityStates()
    {
        return true;
    }
    #endregion

    private void Update()
    {
        if (!_isSpawned || !_isActivated) return;

        Vector2 movementInputDirection = Vector2.zero;
        bool isCrouched = false;
        bool isReloading = false;
        bool isShooting = false;

        if (GONetParticipant.IsMine)
        {
            BipedInputState input = (BipedInputState)_inputStateFactory.Create();
            Simulate(input, Time.deltaTime, (float)GONetMain.Time.ElapsedSeconds);

            isCrouched = _stateVariables.IsCrouched;
            isReloading = _stateVariables.IsReloading;
            isShooting = _stateVariables.IsShooting;
            movementInputDirection = _playerMovement.MovementInput;
            
            _armsOnlyAnimationsController.Update(_stateVariables.IsWalking, _stateVariables.IsRunning, _stateVariables.IsCrouched, _stateVariables.IsAiming, 0f);

            _clientInterpolator.UpdateNetworkedTransform(movementInputDirection, isCrouched, isReloading, isShooting); // NOTE: This moves the player/camera
            splitRotation = splitRotationHolder.rotation; // splitRotation is the actual networked thing here
        }
        else
        {
            _clientInterpolator.UpdateFromNetworkedTransform(splitRotation);
        }

        /*
        if (previousColor.x != currentTeamColor.x ||
            previousColor.y != currentTeamColor.y ||
            previousColor.z != currentTeamColor.z)
        {
            GONetLog.Debug($"seems different than last check.  current color: {new Color(currentTeamColor.x, currentTeamColor.y, currentTeamColor.z)}, isMine? {GONetParticipant.IsMine}");
            previousColor = currentTeamColor;
        }
        */
    }

    Vector3 previousColor;

    private void PerformEntityEvent(LocalContextEntityEvent entityData, out bool isShooting, out bool isReloading, out bool isSmashing)
    {
        isShooting = false;
        isReloading = false;
        isSmashing = false;
        EntityEventType entityEvent = entityData.type;

        switch(entityEvent)
        {
            case EntityEventType.EV_NONE:
                break;
            case EntityEventType.EV_FIRE_WEAPON:
                //GONetLog.Debug("SHOTSKI");
                _audioSource.clip = _shotSound;
                _audioSource.Play();
                isShooting = true;
                break;
            case EntityEventType.EV_RELOAD_WEAPON:
                isReloading = true;
                break;
            case EntityEventType.EV_SMASH:
                _audioSource.clip = _smashSound;
                _audioSource.Play();
                isSmashing = true;
                break;
        }
    }

    private void OnAimInEvents()
    {
        if (!IsClient) return;

        if (GONetParticipant.IsMine)
        {
            _armsOnlyAnimationsController.PlayAimInAnimation();
        }
    }

    private void OnAimOutEvents()
    {
        if (IsClient && GONetParticipant.IsMine)
        {
            _armsOnlyAnimationsController.PlayAimOutAnimation();
        }
    }

    private void OnShotPerformed()
    {
        if (IsClient && GONetParticipant.IsMine)
        {
            _armsOnlyAnimationsController.PlayShootAnimation();
            _audioSource.clip = _shotSound;
            _audioSource.Play();

            LocalContextEntityEvent fireWeaponEntityEvent = new LocalContextEntityEvent(EntityEventType.EV_FIRE_WEAPON, false, null, GetNetworkEntityId());
            _networkController.CreateEntityEvent(GetNetworkEntityId(), fireWeaponEntityEvent);
        }
        else
        {
            //GONetLog.Debug("SHOTSKI, but not client or not mine");
        }

        _stateVariables.SetIsShooting(true);
        _weaponRecoilController.ApplyRecoil(_stateVariables.IsAiming);
    }

    private void OnStartReload()
    {
        _stateVariables.SetIsReloading(true);

        if (IsClient && GONetParticipant.IsMine)
        {
            _armsOnlyAnimationsController.PlayReloadAnimation();
            
            LocalContextEntityEvent reloadEntityEvent = new LocalContextEntityEvent(EntityEventType.EV_RELOAD_WEAPON, false, null, GetNetworkEntityId());
            _networkController.CreateEntityEvent(GetNetworkEntityId(), reloadEntityEvent);
        }
    }

    private void OnEndReload()
    {
        _stateVariables.SetIsReloading(false);
    }

    private void OnSmashPerformed()
    {
        if (IsClient && GONetParticipant.IsMine)
        {
            //Play animation...
            _smashHitMarker.ShowHitMarker();
            _audioSource.clip = _smashSound;
            _audioSource.Play();

            LocalContextEntityEvent smashEntityEvent = new LocalContextEntityEvent(EntityEventType.EV_SMASH, false, null, GetNetworkEntityId());
            _networkController.CreateEntityEvent(GetNetworkEntityId(), smashEntityEvent);
        }
    }

    private void OnEquipWeapon(WeaponComponent equippedWeapon)
    {
        if(IsClient && (GONetParticipant.IsMine || GONetParticipant.IsMine))
        {
            _armsOnlyAnimationsController.SetRuntimeAnimationController(equippedWeapon.PlayerAnimatorController);
        }

        _weaponRecoilController.SetConfiguration(equippedWeapon.Configuration.RecoilConfiguration);
    }

    private void Server_OnKilled()
    {
        _networkController.Server_InitiateKillPlayerProcess(GetNetworkEntityId());
    }

    private void NetworkTick(float deltaTime)
    {
        if (!_isSpawned || !_isActivated)
        {
            return;
        }

        if (IsClient)
        {
            if (!GONetParticipant.IsMine)
            {
                return;
            }

            _stateVariables.SetClientTick(_stateVariables.ClientTick + 1);
        }
        else
        {
            /* this is whacky bandaid ish....maybe want to add:
            // deal with an issue that happens from time to time where the client does not process the color assignment (properly) and no-one including their self changes/assigns the color...so keep checking and try to fix...ASSuME it is related to some init order of ops fuckery and this will only be done seldomly and not accidentally over and over for same player
            if (currentTeamColor.x != _server_assignedColor.r || 
                currentTeamColor.y != _server_assignedColor.g || 
                currentTeamColor.z != _server_assignedColor.b)
            {
                Server_SetColorTeam(_server_assignedColor);
            }
            */
        }
    }

    /// <summary>
    /// Takes damage to the current entity. Only supported from server
    /// </summary>
    /// <param name="damage"></param>
    public void Server_TakeDamage(int damage)
    {
        if (IsServer)
        {
            _health.TakeDamage(damage);
        }
    }

    internal override void Tick(short uniqueTickHz, double elapsedSeconds, double deltaTime)
    {
        base.Tick(uniqueTickHz, elapsedSeconds, deltaTime);

        if (uniqueTickHz == NetworkController.TICK_RATE_HZ)
        {
            if (!_isSpawned)
            {
                return;
            }
            
            NetworkTick(NetworkController.TICK_RATE);
        }
    }

    private void OnMyCurrentColorTeamChanged(GONetEventEnvelope<SyncEvent_ValueChangeProcessed> eventEnvelope)
    {
        GONetLog.Debug($"someone's color changed gonet sync style to: {eventEnvelope.Event.ValueNew}.  mine? {eventEnvelope.GONetParticipant.IsMine}  match? {eventEnvelope.GONetParticipant == gonetParticipant}");

        if (eventEnvelope.GONetParticipant.OwnerAuthorityId == gonetParticipant.OwnerAuthorityId)
        {
            UpdateAllMyColorRelatedValues(eventEnvelope.Event.ValueNew.System_Byte);
        }
    }

    public void UpdateAllMyColorRelatedValues(byte newTeamColor)
    {
        currentTeamColor = newTeamColor;

        if (IsClient)
        {
            Color updatedColor = _networkController.ColorTeamAssigner.GetColor(newTeamColor);
            GONetLog.Debug($"setting full body color: {updatedColor}, isMine? {gonetParticipant.IsMine}");
            _fullBodyPlayerController.SetColorTeam(updatedColor);
            _onlyArmsVisuals.material.color = updatedColor;
        }

        if (IsServer || gonetParticipant.IsMine)
        {
            _health.ResetHealthToMaximum();
        }
    }

    private Color _server_assignedColor;
    public void Server_SetColorTeam(byte colorByte)
    {
        if (!IsServer) return;

        Color color = ColorTeamConfiguration.ColorByIndex[colorByte];
        _server_assignedColor = color;

        GONetLog.Debug($"Set color: {color}");

        // NO longer setting diirectly since server no longer owns this GNP..so have to send owner event so he can change like this: currentTeamColor = new Vector3(color.r, color.g, color.b);

        byte[] paramColor = new byte[] { colorByte };
        GONetLocal local = GONetLocal.LookupByAuthorityId[gonetParticipant.OwnerAuthorityId]; // the event is related to the local of this player ctrl..get it and use its id in event
        GONetMain.EventBus.Publish(
            new LocalContextEntityEvent(EntityEventType.EV_COLOR_ASSIGNED, hasParameters: true, parameters: paramColor, local.GONetParticipant.GONetId),
            targetClientAuthorityId: gonetParticipant.OwnerAuthorityId, 
            shouldPublishReliably: true);

        // why not just go ahead and change the color stuff too in case the "client -> server" round trip doesn't go right (due to some not well understood issue...likely related to all the destroying/spawning/changing of player GNPs going on)
        UpdateAllMyColorRelatedValues(colorByte);
    }

    public byte GetColorTeam()
    {
        return currentTeamColor;
    }

    public Vector3 GetPositon()
    {
        return _noInterpolatedComponents.NoInterpolatedPlayerPositionSource.position;
    }

    public void Server_SetCollidersVisibility(bool visibility)
    {
        if (!IsServer) return;

        _fullBodyPlayerController.SetRagdollCollidersVisibility(visibility);
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
        _fullBodyPlayerController.DeactivateRagdoll();

        if (IsMine)
        {
            _inputsController.Enable();

            _fullBodyVisuals.enabled = false;
            _onlyArmsVisuals.enabled = true;

            GONetLog.Debug($"[PlayerController] ActivateEntityControl_Internal: Setting camera target to {_noInterpolatedComponents.NoInterpolatedHeadTransform?.name}");
            _cameraController.SetCameraTarget(_noInterpolatedComponents.NoInterpolatedHeadTransform);
            _cameraController.SetCameraAsMain();

            PlayerLocalContext.Mine._isBipedInRagdollState = false;
        }

        if (IsClient)
        {
            _dummyDroneFollow.Enable();
        }

        _isActivated = true;
    }

    public void DeactivateEntityControl()
    {
        if (IsMine)
        {
            _inputsController.Disable();

            _fullBodyVisuals.enabled = true;
            _onlyArmsVisuals.enabled = false;
        }

        if (IsClient)
        {
            _dummyDroneFollow.Disable();
        }

        OnGONetParticipantDisabled_KilledToggleControl_Off();

        _isActivated = false;
    }

    public PlayableEntityType GetPlayableEntityType()
    {
        return PlayableEntityType.Biped;
    }

    void IPlayerNetworkEntity.ReceivePlayerState(IPlayerState playerState)
    {
    }

    internal bool TrySetTransform(Vector3 spawnPosition, Quaternion spawnRotation)
    {
        if (!_noInterpolatedComponents) return false;
        
        _noInterpolatedComponents.SetNoInterpolatedPlayerPosition(spawnPosition);
        return true;
    }
}
