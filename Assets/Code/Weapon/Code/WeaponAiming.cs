using UnityEngine;

public class WeaponAiming
{
    private readonly Transform _aimingTransform;
    private readonly Vector3 _notAimingWeaponLocalPosition;
    private readonly WeaponAimingConfiguration _configuration;

    public WeaponAiming(Transform aimingTransform, WeaponAimingConfiguration configuration)
    {
        _aimingTransform = aimingTransform;
        _notAimingWeaponLocalPosition = _aimingTransform.localPosition;

        _configuration = configuration;
    }

    public void UpdateAiming(bool isAiming)
    {
        Vector3 desiredPosition = CalculateDesiredAimingPosition(isAiming);

        Vector3 currentWeaponLocalPosition = Vector3.Lerp(_aimingTransform.localPosition, desiredPosition, _configuration.AimingSpeed * Time.deltaTime);
        _aimingTransform.localPosition = currentWeaponLocalPosition;

        UpdateAimingSway(isAiming);
    }

    private Vector3 CalculateDesiredAimingPosition(bool isAiming)
    {
        Vector3 desiredPosition;
        if (isAiming)
        {
            desiredPosition = Vector3.zero;
            desiredPosition.z = _notAimingWeaponLocalPosition.z - _configuration.AimingZoomInAmount;
        }
        else
        {
            desiredPosition = _notAimingWeaponLocalPosition;
        }

        return desiredPosition;
    }

    private void UpdateAimingSway(bool isAiming)
    {
        Vector3 aimingSwayRotation = Vector3.zero;

        if (isAiming)
        {
            aimingSwayRotation = CalculateAimingSway();
        }

        ApplyAimingSway(aimingSwayRotation);
    }

    private Vector3 CalculateAimingSway()
    {
        float x = _aimingTransform.localRotation.x;
        float y = _aimingTransform.localRotation.y;

        x += _configuration.AimingSwayHorizontalMovement * Mathf.Sin((1 * Time.time + (Mathf.PI / 2)) * _configuration.AimingSwaySpeed);
        y += _configuration.AimingSwayVerticalMovement * Mathf.Sin(2 * Time.time * _configuration.AimingSwaySpeed);

        return new Vector3(y, x, _aimingTransform.localRotation.z);
    }

    private void ApplyAimingSway(Vector3 rotationToApply)
    {
        _aimingTransform.localRotation = Quaternion.Euler(rotationToApply);
    }
}
