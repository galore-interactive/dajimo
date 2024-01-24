using System;
using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public class WeaponController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    //private PlayerAnimationController _animationsController;

    [SerializeField] private WeaponConfigurationSO _weaponConfiguration;

    [Header("Audio Configuration")]
    private AudioPlayer _audioPlayer;

    [Header("Sway Configuration")]
    [SerializeField] private Transform _swayTransform;
    private WeaponSway _swayComponent;

    [Header("Aiming Configuration")]
    [SerializeField] private Transform _aimingTransform;

    [Header("Recoil Configuration")]
    [SerializeField] private Transform _recoilRotatorApplierTransform;
    [SerializeField] private Transform _recoilKickBackApplierTransform;
    //private WeaponRecoil _recoilComponent;

    [Header("Shooting Configuration")]
    private BulletHoleSpawner _bulletHoleSpawner;
    private BulletImpactSpawner _bulletImpactSpawner;
    [SerializeField] private Transform _shotPointTransform;
    //private WeaponShotHandler _shotHandlerComponent;

    [Header("Ammo Configuration")]
    [SerializeField] private int _maxAmmo = 270;
    [SerializeField] private int _clipSize = 30;
    [SerializeField] private int _initialAmmo = 270;
    [SerializeField] private float _reloadTimeInSeconds = 2.37f;
    private WeaponAmmoHandler _weaponAmmoHandler;

    [Header("Muzzle Flash Configuration")]
    [SerializeField] private MuzzleFlashActivator _muzzleFlashActivator;
    [SerializeField] private float _muzzleFlashTimePerShotInSeconds = 0.05f;

    [Header("Bullet Case configuration")]
    [SerializeField] private ParticleSystem _bulletCaseParticleSystem;
    private BulletCaseSpawner _bulletCaseSpawner;

    private PlayerInputsController _inputsController;
    private PlayerStateVariables _playerStateVariables;

    private bool _isInitialized = false;
    private bool _isTaking = false;
    private float _currentTakeTimeInSeconds = 0f;
    [SerializeField] private float _takeTimeInSeconds = 0.8f;

    [SerializeField] private float _hideTimeInSeconds = 0.3f;
    private float _currentHideTimeInSeconds = 0f;
    public event Action OnHided;

    private PlayerCameraController _playerCameraController;

    public void Init(PlayerInputsController inputsController, PlayerStateVariables playerStateVariables)
    {
        _audioPlayer = GetComponent<AudioPlayer>();
        _swayComponent = new WeaponSway(_swayTransform, _weaponConfiguration.SwayConfiguration);
        //_recoilComponent = new WeaponRecoil(_recoilRotatorApplierTransform, _recoilKickBackApplierTransform, _weaponConfiguration.RecoilConfiguration);
        //_shotHandlerComponent = new WeaponShotHandler(_weaponConfiguration.ShootingConfiguration);
        _weaponAmmoHandler = new WeaponAmmoHandler(_maxAmmo, _clipSize, _initialAmmo);
        _playerStateVariables = playerStateVariables;
        //_animationsController = new PlayerAnimationController(_animator, playerStateVariables);
        _bulletHoleSpawner = FindObjectOfType<BulletHoleSpawner>();
        _bulletImpactSpawner = FindObjectOfType<BulletImpactSpawner>();
        _playerCameraController = FindObjectOfType<PlayerCameraController>();
        _bulletCaseSpawner = new BulletCaseSpawner(_bulletCaseParticleSystem);

        _isInitialized = true;
    }

    public void Enable()
    {
        _isTaking = true;
        _audioPlayer.PlayAudio(_weaponConfiguration.SoundsConfiguration.HolsterAudioName);
        SubscribeToShootingEvents();
        SubscribeToReloadingEvents();
    }

    private void SubscribeToShootingEvents()
    {
        //_shotHandlerComponent.Enable();

        //_shotHandlerComponent.OnShotPerformed += OnShotPerformed;

        //_shotHandlerComponent.OnShotHit += OnShotHit;
    }

    private void SubscribeToReloadingEvents()
    {
        //_inputsController.OnReloadInputPressed += TryReload;
    }

    public void Hide()
    {
        _playerStateVariables.SetIsHidingWeapon(true);
        _audioPlayer.PlayAudio(_weaponConfiguration.SoundsConfiguration.HideAudioName);
    }

    public void Disable()
    {
        UnsubscribeToShootingEvents();
        UnsubscribeToReloadingEvents();
        Reset();
    }

    private void UnsubscribeToShootingEvents()
    {
        //_shotHandlerComponent.Disable();

        //_shotHandlerComponent.OnShotPerformed -= OnShotPerformed;

        //_shotHandlerComponent.OnShotHit -= OnShotHit;
    }

    private void UnsubscribeToReloadingEvents()
    {
        //_inputsController.OnReloadInputPressed -= TryReload;
    }

    private void OnShotPerformed()
    {
        PlayShotAudio();
        ApplyRecoil();
        SubstractBullet();
        AddCameraShake();
        ActivateMuzzleFlash();
        SpawnBulletCase();
    }

    private void PlayShotAudio()
    {
        _audioPlayer.PlayAudio(_weaponConfiguration.SoundsConfiguration.ShotAudioName);
    }

    private void ApplyRecoil()
    {
        //_recoilComponent.ApplyRecoil(_inputsController.IsAimingInputBeingPressed);
    }

    private void SubstractBullet()
    {
        _weaponAmmoHandler.SubstractBullet();
    }

    private void AddCameraShake()
    {
        _playerCameraController.ShakeCamera(_weaponConfiguration.ShootingConfiguration.FireRate);
    }
    private void ActivateMuzzleFlash()
    {
        _muzzleFlashActivator.Activate(_muzzleFlashTimePerShotInSeconds);
    }

    private void SpawnBulletCase()
    {
        _bulletCaseSpawner.Spawn();
    }

    private void OnShotHit(ShotHitData hitData)
    {
        SpawnBulletHole(hitData);
        SpawnBulletImpact(hitData);
    }

    private void SpawnBulletHole(ShotHitData hitData)
    {
        _bulletHoleSpawner.Client_Spawn(hitData);
    }

    private void SpawnBulletImpact(ShotHitData hitData)
    {
        _bulletImpactSpawner.Client_Spawn(hitData);
    }

    private void Reset()
    {
        _animator.Rebind();

        _playerStateVariables.SetIsReloading(false);
        //_playerStateVariables.SetIsShooting(false);

        _muzzleFlashActivator.Reset();
    }

    public bool CanDisable()
    {
        if(!_playerStateVariables.IsReloading && !_isTaking && !_playerStateVariables.IsHidingWeapon)
        {
            return true;
        }

        return false;
    }

    //private void Update()
    //{
    //    if(!_isInitialized)
    //    {
    //        return;
    //    }

    //    UpdateTakingTime();
    //    UpdateHidingTime();

    //    UpdateWeaponVariables();

    //    if (CheckIfIsShooting())
    //    {
    //        if(!_shotHandlerComponent.IsPerformingAShot)
    //        {
    //            _shotHandlerComponent.Shoot(_shotPointTransform);
    //        }
    //    }

    //    _shotHandlerComponent.Update();

    //    _swayComponent.UpdateSway(_inputsController.IsAimingInputBeingPressed, _inputsController.CameraMovementInput);
    //    _recoilComponent.UpdateRecoil();

    //    _animationsController.Update();
    //}

    private void UpdateTakingTime()
    {
        if (!_isTaking)
        {
            return;
        }

        _currentTakeTimeInSeconds += Time.deltaTime;

        if (_currentTakeTimeInSeconds >= _takeTimeInSeconds)
        {
            _isTaking = false;
            _currentTakeTimeInSeconds = 0f;
        }
    }

    private void UpdateHidingTime()
    {
        if (!_playerStateVariables.IsHidingWeapon)
        {
            return;
        }

        _currentHideTimeInSeconds += Time.deltaTime;

        if (_currentHideTimeInSeconds >= _hideTimeInSeconds)
        {
            _playerStateVariables.SetIsHidingWeapon(false);
            _currentHideTimeInSeconds = 0f;
            OnHided?.Invoke();
        }
    }

    private bool CheckIfIsShooting()
    {
        return GetShootingInputDependingOnFireMode() && !_weaponAmmoHandler.IsClipEmpty() && !_playerStateVariables.IsReloading && !_isTaking && !_playerStateVariables.IsHidingWeapon;
    }

    private bool GetShootingInputDependingOnFireMode()
    {
        if(_weaponConfiguration.ShootingConfiguration.FireMode.CompareTo(FireMode.Auto) == 0)
        {
            return _inputsController.IsShootingInputFirstTimePressed;
        }
        else
        {
            return _inputsController.IsShootingInputBeingPressed;
        }
    }

    private void TryReload()
    {
        if(_playerStateVariables.IsReloading || _isTaking || _playerStateVariables.IsHidingWeapon)
        {
            return;
        }

        Reload();
    }

    private void Reload()
    {
        _playerStateVariables.SetIsReloading(true);
        _audioPlayer.PlayAudio(_weaponConfiguration.SoundsConfiguration.ReloadAudioName);
        StartCoroutine(ReloadDelayCoroutine());
    }

    private IEnumerator ReloadDelayCoroutine()
    {
        yield return new WaitForSeconds(_reloadTimeInSeconds);
        _weaponAmmoHandler.TryReload();
        _playerStateVariables.SetIsReloading(false);
    }
}
