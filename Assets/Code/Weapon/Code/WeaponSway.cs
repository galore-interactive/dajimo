using UnityEngine;

public class WeaponSway
{
    private readonly Transform _swayTransform = null;
    private readonly WeaponSwayConfiguration _swayConfiguration;

    public WeaponSway(Transform swayTransform, WeaponSwayConfiguration swayConfiguration)
    {
        _swayTransform = swayTransform;
        _swayConfiguration = swayConfiguration;
    }

    public void UpdateSway(bool isAiming, Vector2 cameraMovement)
    {
        Vector3 newSwayPosition = CalculateLinearSway(isAiming, cameraMovement);
        Quaternion newSwayRotation = CalculateRotationalSway(isAiming, cameraMovement);
        ApplySway(newSwayPosition, newSwayRotation);
    }

    public Vector3 CalculateLinearSway(bool isAiming, Vector2 cameraMovement)
    {
        float mouseX = -cameraMovement.x;
        float mouseY = -cameraMovement.y;

        if (isAiming)
        {
            mouseX *= _swayConfiguration.LinearSwayAmountWhenAiming;
            mouseY *= _swayConfiguration.LinearSwayAmountWhenAiming;
        }
        else
        {
            mouseX *= _swayConfiguration.LinearSwayAmount;
            mouseY *= _swayConfiguration.LinearSwayAmount;
        }

        mouseX = Mathf.Clamp(mouseX, -_swayConfiguration.LinearSwayMaxAmount, _swayConfiguration.LinearSwayMaxAmount);
        mouseY = Mathf.Clamp(mouseY, -_swayConfiguration.LinearSwayMaxAmount, _swayConfiguration.LinearSwayMaxAmount);

        //Calculate adjustment rotation
        Vector3 finalSwayPosition = new Vector3(mouseX, mouseY, 0);
        return Vector3.Lerp(_swayTransform.localPosition, finalSwayPosition, Time.deltaTime * _swayConfiguration.LinearSwaySmooth);
    }

    private Quaternion CalculateRotationalSway(bool isAiming, Vector2 cameraMovement)
    {
        float mouseX = -cameraMovement.x;
        float mouseY = -cameraMovement.y;
        if (isAiming)
        {
            mouseX *= _swayConfiguration.HorizontalRotationalSwayAmountWhenAiming;
            mouseY *= _swayConfiguration.VerticalRotationalSwayAmountWhenAiming;
        }
        else
        {
            mouseX *= _swayConfiguration.HorizontalRotationalSwayAmount;
            mouseY *= _swayConfiguration.VerticalRotationalSwayAmount;
        }

        mouseX = Mathf.Clamp(mouseX, -_swayConfiguration.RotationalSwayMaxAmount, _swayConfiguration.RotationalSwayMaxAmount);
        mouseY = Mathf.Clamp(mouseY, -_swayConfiguration.RotationalSwayMaxAmount, _swayConfiguration.RotationalSwayMaxAmount);

        Quaternion desiredSwayRotation = Quaternion.Euler(mouseY, 0, mouseX);
        return Quaternion.Slerp(_swayTransform.localRotation, desiredSwayRotation, Time.deltaTime * _swayConfiguration.RotationalSwaySmooth);
    }

    private void ApplySway(Vector3 swayPositionToApply, Quaternion swayRotationToApply)
    {
        _swayTransform.localPosition = swayPositionToApply;
        _swayTransform.localRotation = swayRotationToApply;
    }
}
