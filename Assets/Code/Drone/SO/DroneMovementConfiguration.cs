using UnityEngine;

[System.Serializable]
public class DroneMovementConfiguration
{
    [SerializeField] private float _movementSpeed = 5f;
    public float MovementSpeed => _movementSpeed;
    [SerializeField] private float _movementAccelerationTime = 0.2f;
    public float MovementAccelerationTime => _movementAccelerationTime;
}
