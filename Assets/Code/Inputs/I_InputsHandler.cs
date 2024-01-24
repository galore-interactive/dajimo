using UnityEngine;

public interface I_InputsHandler
{
    public InputsHandlerType Type { get; }
    public void Enable();
    public void Disable();
    public bool IsShootingInputBeingPressed { get; }
    public bool IsShootingInputFirstTimePressed { get; }
    public bool IsAimingInputBeingPressed { get; }
    public bool IsRunningInputBeingPressed { get; }
    public bool IsCrouchingOrStandingInputBeingPressed { get; }
    public Vector2 MovementDirectionInputVector2Normalized { get; }
    public Vector3 MovementDirectionInputVector3Normalized { get; }
    public Vector2 CameraMovementInput { get; }
    public bool IsSwitchWeaponInputBeingPressed { get; }
    public bool IsRealoadingInputBeingPressed { get; }
    public bool IsPlacingObjectInputBeingPressed { get; }
    public bool IsSmashingInputBeingPressed { get; }
}