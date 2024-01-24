using UnityEngine;

public class KeyboardPlayerInputs : PlayerInputsBase
{
    public KeyboardPlayerInputs()
    {
        _type = "Keyboard";
    }

    protected override void HandleAimingInput()
    {
        _isAimingInputBeingPressed = Input.GetMouseButton(1);
    }

    protected override void HandleMovementDirectionInput()
    {
        _movementDirectionInput.x = Input.GetAxisRaw("Horizontal");
        _movementDirectionInput.y = Input.GetAxisRaw("Vertical");
    }

    protected override void HandleCameraMovementInput()
    {
        _cameraMovementInput.x = Input.GetAxis("Mouse X");
        _cameraMovementInput.y = Input.GetAxis("Mouse Y");
    }

    protected override void HandleReloadingInput()
    {
        _isReloadingInputBeingPressed = Input.GetKey(KeyCode.R);
    }

    protected override void HandleRunningInput()
    {
        _isRunningInputBeingPressed = Input.GetKey(KeyCode.LeftShift);
    }

    protected override void HandleShootingInput()
    {
        _isShootingInputBeingPressed = Input.GetMouseButton(0);
        _isShootingInputFirstTimePressed = Input.GetMouseButtonDown(0);
    }

    protected override void HandleSwitchWeaponInput()
    {
        _isSwitchWeaponInputBeingPressed = (Input.mouseScrollDelta.y != 0)? true : false;
    }

    protected override void HandleCrouchStandInput()
    {
        _isCrouchStandInputPressed = Input.GetKeyDown(KeyCode.C);
    }
}
