using GONet;
using System;
using UnityEngine;

public class SmashController
{
    public event Action OnSmashPerformed;

    private readonly RaycastShooter _raycastShooter;
    private readonly PlayerStateVariables _stateVariables;
    private readonly Transform _smashOriginTransform;
    private readonly GONetParticipant _gnp;
    private readonly float _smashingDistance;
    private readonly float _smashingLifeTime;
    private readonly ShotConfiguration _shotConfiguration;

    private readonly int _raycastLayerMask;

    private float _smashingTimeLeft;
    public float SmashingTimeLeft => _smashingTimeLeft;

    public void SetSmashingTimeLeft(float smashingTimeLeft)
    {
        _smashingTimeLeft = smashingTimeLeft;
    }

    public SmashController(RaycastShooter raycastShooter, PlayerStateVariables stateVariables, Transform smashOriginTransform, GONetParticipant gnp, float smashingDistance, float smashingLifeTime)
    {
        _raycastShooter = raycastShooter;
        _stateVariables = stateVariables;
        _smashOriginTransform = smashOriginTransform;
        _gnp = gnp;
        _smashingDistance = smashingDistance;
        _smashingLifeTime = smashingLifeTime;

        const string NON_SHOOTABLE_LAYER_MASK = "NonShootable";
        const string IGNORE_RAYCAST_LAYER_MASK_NAME = "Ignore Raycast";
        _raycastLayerMask = ~(LayerMask.GetMask(NON_SHOOTABLE_LAYER_MASK) | LayerMask.GetMask(IGNORE_RAYCAST_LAYER_MASK_NAME));

        _shotConfiguration = new ShotConfiguration(_gnp, 100, _smashingDistance, WeaponId.Buttstock);
    }

    public void Simulate(bool isSmashingInput, float elapsedTime)
    {
        if(isSmashingInput && !_stateVariables.IsSmashing)
        {
            _stateVariables.SetIsSmashing(true);
            Smash();
        }

        if(_stateVariables.IsSmashing)
        {
            _smashingTimeLeft -= elapsedTime;

            if(_smashingTimeLeft <= 0)
            {
                _stateVariables.SetIsSmashing(false);
                _smashingTimeLeft = 0;
            }
        }
    }

    private void Smash()
    {
        _smashingTimeLeft = _smashingLifeTime;

        if(GONetMain.IsServer)
        {
            _raycastShooter.Server_ShootRegisterShot(_shotConfiguration);
        }
        else
        {
            _raycastShooter.ShootWithRaycast(_smashOriginTransform.position, _smashOriginTransform.forward, _raycastLayerMask, _smashingDistance);
        }

        OnSmashPerformed?.Invoke();
    }
}
