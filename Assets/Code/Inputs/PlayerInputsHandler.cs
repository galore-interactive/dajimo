using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputsHandler : I_InputsHandler
{
    private readonly PlayerInputActions _inputActionAsset;

    public PlayerInputsHandler(PlayerInputActions inputActionAsset)
    {
        _inputActionAsset = inputActionAsset;
    }

    public InputsHandlerType Type => InputsHandlerType.Player;

    public void Enable()
    {
        _inputActionAsset.Player.Enable();


    }

    public void Disable()
    {
        _inputActionAsset.Player.Disable();
    }

    public bool IsShootingInputBeingPressed => _inputActionAsset.Player.Shoot.IsPressed();
    public bool IsShootingInputFirstTimePressed => _inputActionAsset.Player.Shoot.WasPressedThisFrame();
    public bool IsAimingInputBeingPressed => _inputActionAsset.Player.Aim.IsPressed();
    public bool IsRunningInputBeingPressed => _inputActionAsset.Player.Run.IsPressed();
    public bool IsCrouchingOrStandingInputBeingPressed => _inputActionAsset.Player.CrouchStand.WasPressedThisFrame();
    public Vector2 MovementDirectionInputVector2Normalized => _inputActionAsset.Player.Move.ReadValue<Vector2>();
    public Vector3 MovementDirectionInputVector3Normalized => throw new System.NotImplementedException();
    public Vector2 CameraMovementInput
    {
        get
        {
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
            return GetGamepadCameraMovementInput();
#else
            return Gamepad.all.Count > 0 ? GetGamepadCameraMovementInput(1.5f) : GetMouseCameraMovementInput();
#endif
        }
    }

    private Vector2 GetMouseCameraMovementInput()
    {
        return _inputActionAsset.Player.CameraMove.ReadValue<Vector2>() * 0.05f;
    }

    private Vector2 GetGamepadCameraMovementInput(float extraMultiplier = 1f)
    {
        Vector2 rawInput = _inputActionAsset.Player.CameraMove.ReadValue<Vector2>();
        Vector2 squared = rawInput * rawInput;
        const float MULTIPLIER = 1.25f;

        Vector2 final = squared * MULTIPLIER * extraMultiplier;
        if (rawInput.x < 0) final.x = -final.x;
        if (rawInput.y < 0) final.y = -final.y;
        
        return final;
    }

    public bool IsSwitchWeaponInputBeingPressed => false;
    public bool IsRealoadingInputBeingPressed => _inputActionAsset.Player.Reload.WasPressedThisFrame();
    public bool IsPlacingObjectInputBeingPressed => throw new System.NotImplementedException();
    public bool IsSmashingInputBeingPressed => _inputActionAsset.Player.Smash.WasPerformedThisFrame();
}
