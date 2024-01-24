using UnityEngine;

[System.Serializable]
public class CrouchStandConfiguration
{
    [SerializeField] private float _crouchHeight;
    [SerializeField] private float _standHeight;
    [SerializeField] private float _crouchSpeed;

    [SerializeField] private Vector3 _crouchCharacterControllerCenter;
    [SerializeField] private Vector3 _standCharacterControllerCenter;

    public float CrouchHeight => _crouchHeight;
    public float StandHeight => _standHeight;
    public float CrouchSpeed => _crouchSpeed;
    public Vector3 CrouchCharacterControllerCenter => _crouchCharacterControllerCenter;
    public Vector3 StandCharacterControllerCenter => _standCharacterControllerCenter;

    public CrouchStandConfiguration() { }

    public CrouchStandConfiguration(float crouchHeight, float standHeight, float crouchSpeed, Vector3 crouchCharacterControllerCenter, Vector3 standCharacterControllerCenter)
    {
        _crouchHeight = crouchHeight;
        _standHeight = standHeight;
        _crouchSpeed = crouchSpeed;
        _crouchCharacterControllerCenter = crouchCharacterControllerCenter;
        _standCharacterControllerCenter = standCharacterControllerCenter;
    }
}
