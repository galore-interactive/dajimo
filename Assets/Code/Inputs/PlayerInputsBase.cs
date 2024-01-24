using UnityEngine;

public abstract class PlayerInputsBase
{
    protected string _type;
    protected Vector2 _movementDirectionInput;
    protected bool _isRunningInputBeingPressed;
    protected bool _isShootingInputBeingPressed;
    protected bool _isShootingInputFirstTimePressed;
    protected bool _isAimingInputBeingPressed;
    protected bool _isReloadingInputBeingPressed;
    protected Vector2 _cameraMovementInput;
    protected bool _isSwitchWeaponInputBeingPressed;
    protected bool _isCrouchStandInputPressed;

    public string Type => _type;
    public Vector2 MovementDirectionInputNormalized => _movementDirectionInput;
    public bool IsRunningInputBeingPressed => _isRunningInputBeingPressed;
    public bool IsShootingInputBeingPressed => _isShootingInputBeingPressed;
    public bool IsShootingInputFirstTimePressed => _isShootingInputFirstTimePressed;
    public bool IsReloadingInputBeingPressed => _isReloadingInputBeingPressed;
    public bool IsAimingInputBeingPressed => _isAimingInputBeingPressed;
    public Vector2 CameraMovementInput => _cameraMovementInput;
    public bool IsSwitchWeaponInputBeingPressed => _isSwitchWeaponInputBeingPressed;
    public bool IsCrouchStandInputPressed => _isCrouchStandInputPressed;

    public void Update()
    {
        HandleInputs();
    }

    private void HandleInputs()
    {
        HandleMovementDirectionInput();
        HandleCameraMovementInput();
        HandleRunningInput();
        HandleShootingInput();
        HandleReloadingInput();
        HandleAimingInput();
        HandleSwitchWeaponInput();
        HandleCrouchStandInput();

        NormalizeMovement();
    }

    protected abstract void HandleMovementDirectionInput();
    protected abstract void HandleCameraMovementInput();
    protected abstract void HandleRunningInput();
    protected abstract void HandleShootingInput();
    protected abstract void HandleAimingInput();
    protected abstract void HandleReloadingInput();
    protected abstract void HandleSwitchWeaponInput();
    protected abstract void HandleCrouchStandInput();

    private void NormalizeMovement()
    {
        _movementDirectionInput = _movementDirectionInput.normalized;
    }
}
