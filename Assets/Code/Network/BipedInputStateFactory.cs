using UnityEngine;

public class BipedInputStateFactory : I_InputStateFactory
{
    private readonly PlayerInputsController _inputsController;
    private readonly PlayerStateVariables _stateVariables;

    public BipedInputStateFactory(PlayerInputsController inputsController, PlayerStateVariables stateVariables)
    {
        _inputsController = inputsController;
        _stateVariables = stateVariables;
    }

    public I_InputState Create()
    {
        return CreateSpecific();
    }

    public BipedInputState CreateSpecific()
    {
        Vector2 movementInput = _inputsController.MovementDirectionInputVector2Normalized;
        Vector2 cameraMovementInput = _inputsController.CameraMovementInput;
        bool isRunning = _inputsController.IsRunningInputBeingPressed;
        bool isCrouchingOrStandingUp = _inputsController.IsCrouchingOrStandingInputBeingPressed;
        bool isAiming = _inputsController.IsAimingInputBeingPressed;
        bool isShooting = _inputsController.IsShootingInputBeingPressed;
        bool isReloading = _inputsController.IsRealoadingInputBeingPressed;
        bool isSmashing = _inputsController.IsSmashingInputBeingPressed;
        BipedInputState inputState = new BipedInputState(movementInput, cameraMovementInput, isRunning, isCrouchingOrStandingUp, isAiming, isShooting, isReloading, isSmashing, _stateVariables.ClientTick);
        return inputState;
    }
}
