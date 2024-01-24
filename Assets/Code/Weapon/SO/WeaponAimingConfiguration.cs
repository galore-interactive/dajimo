using UnityEngine;

[System.Serializable]
public class WeaponAimingConfiguration
{
    [SerializeField] private float _aimingSpeed = 10f;
    [SerializeField] private float _aimingZoomInAmount = 0.1f;

    [Header("Aiming Sway (Lissajous Curves)")]
    [SerializeField] private float _aimingSwayHorizontalMovement = 0.25f;
    [SerializeField] private float _aimingSwayVerticalMovement = 0.1f;
    [SerializeField] private float _aimingSwaySpeed = 0.8f;

    public float AimingSpeed => _aimingSpeed;
    public float AimingZoomInAmount => _aimingZoomInAmount;
    public float AimingSwayHorizontalMovement => _aimingSwayHorizontalMovement;
    public float AimingSwayVerticalMovement => _aimingSwayVerticalMovement;
    public float AimingSwaySpeed => _aimingSwaySpeed;

    public WeaponAimingConfiguration(){ }

    public WeaponAimingConfiguration(float aimingSpeed, float aimingZoomInAmount, float aimingSwayHorizontalMovement, float aimingSwayVerticalMovement, float aimingSwaySpeed)
    {
        _aimingSpeed = aimingSpeed;
        _aimingZoomInAmount = aimingZoomInAmount;
        _aimingSwayHorizontalMovement = aimingSwayHorizontalMovement;
        _aimingSwayVerticalMovement = aimingSwayVerticalMovement;
        _aimingSwaySpeed = aimingSwaySpeed;
    }
}
