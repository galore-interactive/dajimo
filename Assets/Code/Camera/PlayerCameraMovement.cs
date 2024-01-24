using UnityEngine;

public class PlayerCameraMovement
{
    private readonly PlayerCameraMovementConfiguration _playerCameraMovementData = null;

    public PlayerCameraMovement(PlayerCameraMovementConfiguration playerCameraMovementData)
    {
        _playerCameraMovementData = playerCameraMovementData;
    }

    public Vector2 GetUpdatedRotationApplyingInput(Vector2 input, float currentHorizontalRotation, float currentVerticalRotation, float deltaTime)
    {
        Vector2 inputInverseY = new Vector2(input.x, -input.y);

        Vector2 updatedRotation = Vector2.zero;
        updatedRotation.x = CalculateHorizontalRotation(inputInverseY, currentHorizontalRotation, deltaTime);
        updatedRotation.y = CalculateVerticalAngle(inputInverseY, currentVerticalRotation, deltaTime);

        return updatedRotation;
    }

    private float CalculateHorizontalRotation(Vector2 input, float currentHorizontalRotation, float deltaTime)
    {
        currentHorizontalRotation += input.x * _playerCameraMovementData.MouseSensitivity * deltaTime;
        return currentHorizontalRotation;
    }

    private float CalculateVerticalAngle(Vector2 input, float currentVerticalRotation, float deltaTime)
    {
        currentVerticalRotation += input.y * _playerCameraMovementData.MouseSensitivity * deltaTime;
        currentVerticalRotation = Mathf.Clamp(currentVerticalRotation, -_playerCameraMovementData.VerticalClampAngle, _playerCameraMovementData.VerticalClampAngle);
        return currentVerticalRotation;
    }
}
