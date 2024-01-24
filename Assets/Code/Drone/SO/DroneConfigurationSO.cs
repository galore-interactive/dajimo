using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Drone/Drone Configuration", fileName = "DroneConfiguration")]
public class DroneConfigurationSO : ScriptableObject
{
    [SerializeField] private DroneMovementConfiguration _movementConfiguration;
    [SerializeField] private PlayerCameraMovementConfiguration _cameraMovementConfiguration;
    public DroneMovementConfiguration MovementConfiguration => _movementConfiguration;
    public PlayerCameraMovementConfiguration CameraMovementConfiguration => _cameraMovementConfiguration;
}
