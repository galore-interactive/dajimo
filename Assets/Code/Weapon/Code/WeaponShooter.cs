using GONet;
using System;
using UnityEngine;

public class WeaponShooter
{
    public event Action OnShotPerformed;

    private readonly WeaponShootingConfiguration _configuration;
    private readonly RaycastShooter _raycastShooter;
    private readonly GONetParticipant _gnp;
    private readonly int _raycastLayerMask;
    private readonly ShotConfiguration _shotConfiguration;

    private float _timesinceLastShot;
    public float TimeSinceLastShot => _timesinceLastShot;

    public void SetTimeSinceLastShot(float newValue)
    {
        _timesinceLastShot = newValue;
    }

    public WeaponShooter(WeaponShootingConfiguration configuration, RaycastShooter raycastShooter, GONetParticipant gnp, WeaponId weaponId)
    {
        _configuration = configuration;
        _raycastShooter = raycastShooter;
        _gnp = gnp;

        const string NON_SHOOTABLE_LAYER_MASK = "NonShootable";
        const string IGNORE_RAYCAST_LAYER_MASK_NAME = "Ignore Raycast";
        _raycastLayerMask = ~(LayerMask.GetMask(NON_SHOOTABLE_LAYER_MASK) | LayerMask.GetMask(IGNORE_RAYCAST_LAYER_MASK_NAME));

        _shotConfiguration = new ShotConfiguration(_gnp, 10, Mathf.Infinity, weaponId);
    }

    public void Shoot(Transform shotPointTransform)
    {
        _timesinceLastShot = _timesinceLastShot % _configuration.FireRate;

        if (GONetMain.IsServer)
        {
            _raycastShooter.Server_ShootRegisterShot(_shotConfiguration);
        }
        else
        {
            _raycastShooter.ShootWithRaycast(shotPointTransform.position, shotPointTransform.forward, _raycastLayerMask);
        }

        OnShotPerformed?.Invoke();
    }

    public void UpdateShooting(float elapsedTime)
    {
        _timesinceLastShot += elapsedTime;

        //This is to avoid that the first two shots are way too close
        if(_timesinceLastShot > 2 * _configuration.FireRate)
        {
            _timesinceLastShot = 2 * _configuration.FireRate;
        }
    }

    public bool IsPerformingAShot()
    {
        return _timesinceLastShot < _configuration.FireRate;
    }
}
