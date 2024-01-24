using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Player/Player Configuration", fileName = "PlayerConfiguration")]
public class PlayerConfigurationSO : ScriptableObject
{
    [SerializeField] private PlayerMovementConfiguration _movementConfiguration;
    [SerializeField] private CrouchStandConfiguration _crouchStandConfiguration;
    [SerializeField] private PlayerCameraMovementConfiguration _cameraMovementConfiguration;
    [SerializeField] private WeaponSwayConfiguration _armsSwayConfiguration;

    public PlayerMovementConfiguration MovementConfiguration => _movementConfiguration;
    public CrouchStandConfiguration CrouchStandConfiguration => _crouchStandConfiguration;
    public PlayerCameraMovementConfiguration CameraMovementConfiguration => _cameraMovementConfiguration;
    public WeaponSwayConfiguration ArmsSwayConfiguration => _armsSwayConfiguration;
}
