using UnityEngine;

[System.Serializable]
public class PlayerMovementConfiguration
{
    [SerializeField] private float _walkingSpeed = 4f;
    [SerializeField] private float _runningSpeed = 6f;
    [SerializeField] private float _crouchingSpeed = 2f;
    [SerializeField] private float _movementAccelerationTime = 0.1f;
    [SerializeField] private float _aimingSpeed = 1.9f;

    public float WalkingSpeed => _walkingSpeed;

    public float RunningSpeed => _runningSpeed;
    public float CrouchingSpeed => _crouchingSpeed;

    public float MovementAccelerationTime => _movementAccelerationTime;
    public float AimingSpeed => _aimingSpeed;

    public PlayerMovementConfiguration() { }

    public PlayerMovementConfiguration(float walkingSpeed, float runningSpeed, float crouchingSpeed, float movementAccelerationTime, float aimingSpeed)
    {
        _walkingSpeed = walkingSpeed;
        _runningSpeed = runningSpeed;
        _crouchingSpeed = crouchingSpeed;
        _movementAccelerationTime = movementAccelerationTime;
        _aimingSpeed = aimingSpeed;
    }
}
