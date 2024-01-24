using UnityEngine;

public class DroneInputsHandler : I_InputsHandler
{
    private readonly PlayerInputActions _inputActionAsset;

    public DroneInputsHandler(PlayerInputActions inputActionAsset)
    {
        _inputActionAsset = inputActionAsset;
    }

    public InputsHandlerType Type => InputsHandlerType.Drone;

    public void Enable()
    {
        _inputActionAsset.Drone.Enable();
    }

    public void Disable()
    {
        _inputActionAsset.Drone.Disable();
    }

    public bool IsShootingInputBeingPressed => throw new System.NotImplementedException();
    public bool IsShootingInputFirstTimePressed => throw new System.NotImplementedException();
    public bool IsAimingInputBeingPressed => throw new System.NotImplementedException();
    public bool IsRunningInputBeingPressed => throw new System.NotImplementedException();
    public bool IsCrouchingOrStandingInputBeingPressed => throw new System.NotImplementedException();
    public Vector2 MovementDirectionInputVector2Normalized => throw new System.NotImplementedException();
    public Vector3 MovementDirectionInputVector3Normalized => _inputActionAsset.Drone.Move.ReadValue<Vector3>();

    public Vector2 CameraMovementInput
    {
        get
        {
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
            Vector2 rawInput = _inputActionAsset.Drone.CameraMove.ReadValue<Vector2>();
            Vector2 squared = rawInput * rawInput;
            const float MULTIPLIER = 1.15f;
            Vector2 final = squared * MULTIPLIER;
            if (rawInput.x < 0) final.x = -final.x;
            if (rawInput.y < 0) final.y = -final.y;
            return final;
#else
            return _inputActionAsset.Drone.CameraMove.ReadValue<Vector2>() * 0.05f;
#endif
        }
    }


    public bool IsSwitchWeaponInputBeingPressed => throw new System.NotImplementedException();
    public bool IsRealoadingInputBeingPressed => throw new System.NotImplementedException();
    public bool IsPlacingObjectInputBeingPressed => _inputActionAsset.Drone.IsPlacingObject.IsPressed();
    public bool IsSmashingInputBeingPressed => throw new System.NotImplementedException();
}
