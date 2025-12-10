using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputsHandler : I_InputsHandler
{
    private readonly PlayerInputActions _inputActionAsset;
    private bool _isUsingGamepad = false;

    // Thresholds for detecting active input device
    private const float GAMEPAD_STICK_THRESHOLD = 0.1f;
    private const float MOUSE_DELTA_THRESHOLD = 0.5f;

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

    public bool IsUsingGamepad => _isUsingGamepad;

    public Vector2 CameraMovementInput
    {
        get
        {
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
            _isUsingGamepad = true;
            return GetGamepadCameraMovementInput();
#else
            UpdateActiveInputDevice();
            return _isUsingGamepad ? GetGamepadCameraMovementInput(1.5f) : GetMouseCameraMovementInput();
#endif
        }
    }

    private void UpdateActiveInputDevice()
    {
        // Check for gamepad right stick input
        if (Gamepad.current != null)
        {
            Vector2 rightStick = Gamepad.current.rightStick.ReadValue();
            if (rightStick.sqrMagnitude > GAMEPAD_STICK_THRESHOLD * GAMEPAD_STICK_THRESHOLD)
            {
                _isUsingGamepad = true;
                return;
            }
        }

        // Check for mouse movement
        if (Mouse.current != null)
        {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();
            if (mouseDelta.sqrMagnitude > MOUSE_DELTA_THRESHOLD * MOUSE_DELTA_THRESHOLD)
            {
                _isUsingGamepad = false;
            }
        }
    }

    private Vector2 GetMouseCameraMovementInput()
    {
        Vector2 raw = Mouse.current != null ? Mouse.current.delta.ReadValue() : Vector2.zero;
        Vector2 scaled = raw * 0.05f;
        return scaled;
    }

    private Vector2 GetGamepadCameraMovementInput(float extraMultiplier = 1f)
    {
        if (Gamepad.current == null) return Vector2.zero;

        Vector2 rawInput = Gamepad.current.rightStick.ReadValue();
        // Apply non-linear response curve for better gamepad feel
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
