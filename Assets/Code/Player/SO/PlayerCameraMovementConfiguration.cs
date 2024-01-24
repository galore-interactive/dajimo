using UnityEngine;

[System.Serializable]
public class PlayerCameraMovementConfiguration
{
    [SerializeField] private float _mouseSensitivity = 4f;
    [SerializeField] private float _cameraMovementAcceleration = 0.05f;
    [SerializeField] private float _verticalClampAngle = 80f;

    public float MouseSensitivity => _mouseSensitivity;
    public float CameraMovementAcceleration => _cameraMovementAcceleration;
    public float VerticalClampAngle => _verticalClampAngle;

    public PlayerCameraMovementConfiguration() { }

    public PlayerCameraMovementConfiguration(float mouseSensitivity, float cameraMovementAcceleration, float verticalClampAngle)
    {
        _mouseSensitivity = mouseSensitivity;
        _cameraMovementAcceleration = cameraMovementAcceleration;
        _verticalClampAngle = verticalClampAngle;
    }
}
