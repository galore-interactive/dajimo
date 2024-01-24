using GONet;
using System;
using UnityEngine;

public class WeaponComponent : MonoBehaviour
{
    public event Action OnStartReload;
    public event Action OnEndReload;
    [SerializeField] private WeaponConfigurationSO _configuration;

    [SerializeField] private Animator _animator;
    [SerializeField] private RuntimeAnimatorController _playerAnimatorController;
    private WeaponAnimationController _animationController;

    private WeaponAmmoHandler _ammoHandler;

    private Transform _shotPointTransform;
    private WeaponShooter _weaponShooter;

    private float _reloadTimeLeft = 0f;
    private bool _isBeingReloaded = false;

    private WeaponComponentsEnableConfiguration _enableConfiguration;

    [SerializeField] private ParticleSystem _bulletCaseParticleSystem;
    private BulletCaseSpawner _bulletCaseSpawner;

    public RuntimeAnimatorController PlayerAnimatorController => _playerAnimatorController;
    public WeaponConfigurationSO Configuration => _configuration;
    public float TimeSinceLastShot => _weaponShooter.TimeSinceLastShot;
    public int ClipAmmoLeft => _ammoHandler.ClipAmmoLeft;
    public int AmmoLeft => _ammoHandler.AmmoLeft;
    public float ReloadTimeLeft => _reloadTimeLeft;

    public void SetTimeSinceLastShot(float newTime)
    {
        _weaponShooter.SetTimeSinceLastShot(newTime);
    }

    public void SetClipAmmoLeft(int newValue)
    {
        _ammoHandler.SetClipAmmoLeft(newValue);
    }

    public void SetAmmoLeft(int newValue)
    {
        _ammoHandler.SetAmmoLeft(newValue);
    }

    public void SetReloadTimeLeft(float newValue)
    {
        _reloadTimeLeft = newValue;
    }

    public void SetIsBeingReloaded(bool newValue)
    {
        _isBeingReloaded = newValue;
    }

    public void Initialize(Transform shotPointTransform, WeaponComponentsEnableConfiguration enableConfiguration, RaycastShooter raycastShooter, GONetParticipant gnp)
    {
        _shotPointTransform = shotPointTransform;
        _enableConfiguration = enableConfiguration;

        _ammoHandler = new WeaponAmmoHandler(1_000, 30, 1_000);
        _animationController = new WeaponAnimationController(_animator);

        _weaponShooter = new WeaponShooter(_configuration.ShootingConfiguration, raycastShooter, gnp, _configuration.WeaponId);
        _bulletCaseSpawner = new BulletCaseSpawner(_bulletCaseParticleSystem);
    }

    public void Enable()
    {
        _weaponShooter.OnShotPerformed += OnShotPerformedEvents;
    }

    public void Disable()
    {
        _weaponShooter.OnShotPerformed -= OnShotPerformedEvents;
    }

    private void OnShotPerformedEvents()
    {
        _ammoHandler.SubstractBullet();

        if (GONetMain.IsClient)
        {
            _animationController.PlayShootAnimation();
        }
    }

    public bool CanShot()
    {
        return _ammoHandler.HasAmmoInClip() && !_weaponShooter.IsPerformingAShot();
    }

    public void Shoot()
    {
        _weaponShooter.Shoot(_shotPointTransform);
        
        if (GONetMain.IsClient)
        {
            _bulletCaseSpawner.Spawn();
        }
    }

    public bool CanReload()
    {
        return _ammoHandler.CanReload();
    }

    public void Reload()
    {
        _reloadTimeLeft = 2.5f;
        _isBeingReloaded = true;
        _animationController.PlayReloadAnimation();
        OnStartReload?.Invoke();
    }

    public void UpdateWeapon(float elapsedTime)
    {
        _weaponShooter.UpdateShooting(elapsedTime);
        UpdateReloading(elapsedTime);
    }

    private void UpdateReloading(float elapsedTime)
    {
        if (!_isBeingReloaded)
        {
            return;
        }

        _reloadTimeLeft -= elapsedTime;

        if (_reloadTimeLeft <= 0f)
        {
            _reloadTimeLeft = 0f;
            _isBeingReloaded = false;
            _ammoHandler.TryReload();
            OnEndReload?.Invoke();
        }
    }
}
