using UnityEngine;

[System.Serializable]
public class WeaponSwayConfiguration
{
    [Header("Linear Sway")]
    [SerializeField] private float _linearSwayAmount;
    [SerializeField] private float _linearSwayAmountWhenAiming;
    [SerializeField] private float _linearSwaySmooth;
    [SerializeField] private float _linearSwayMaxAmount;

    [Header("Rotational Sway")]
    [SerializeField] private float _horizontalRotationalSwayAmount;
    [SerializeField] private float _verticalRotationalSwayAmount;
    [SerializeField] private float _horizontalRotationalSwayAmountWhenAiming;
    [SerializeField] private float _verticalRotationalSwayAmountWhenAiming;
    [SerializeField] private float _rotationalSwaySmooth;
    [SerializeField] private float _rotationalSwayMaxAmount;

    public float LinearSwayAmount => _linearSwayAmount;
    public float LinearSwayAmountWhenAiming => _linearSwayAmountWhenAiming;
    public float LinearSwaySmooth => _linearSwaySmooth;
    public float LinearSwayMaxAmount => _linearSwayMaxAmount;

    public float HorizontalRotationalSwayAmount => _horizontalRotationalSwayAmount;
    public float VerticalRotationalSwayAmount => _verticalRotationalSwayAmount;
    public float HorizontalRotationalSwayAmountWhenAiming => _horizontalRotationalSwayAmountWhenAiming;
    public float VerticalRotationalSwayAmountWhenAiming => _horizontalRotationalSwayAmountWhenAiming;
    public float RotationalSwaySmooth => _rotationalSwaySmooth;
    public float RotationalSwayMaxAmount => _rotationalSwayMaxAmount;

    public WeaponSwayConfiguration() { }

    public WeaponSwayConfiguration(float linearSwayAmount, float linearSwayAmountWhenAiming, float linearSwaySmooth, float linearSwayMaxAmount, float rotationalSwayAmount, float rotationalSwayAmountWhenAiming, float rotationalSwaySmooth, float rotationalSwayMaxAmount)
    {
        _linearSwayAmount = linearSwayAmount;
        _linearSwayAmountWhenAiming = linearSwayAmountWhenAiming;
        _linearSwaySmooth = linearSwaySmooth;
        _linearSwayMaxAmount = linearSwayMaxAmount;
        _horizontalRotationalSwayAmount = rotationalSwayAmount;
        _horizontalRotationalSwayAmountWhenAiming = rotationalSwayAmountWhenAiming;
        _rotationalSwaySmooth = rotationalSwaySmooth;
        _rotationalSwayMaxAmount = rotationalSwayMaxAmount;
    }
}
