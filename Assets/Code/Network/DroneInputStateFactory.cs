using UnityEngine;

public class DroneInputStateFactory : I_InputStateFactory
{
    private readonly PlayerInputsController _inputsController;
    private readonly PlayerStateVariables _stateVariables;

    public DroneInputStateFactory(PlayerInputsController inputsController, PlayerStateVariables stateVariables)
    {
        _inputsController = inputsController;
        _stateVariables = stateVariables;
    }

    public I_InputState Create()
    {
        return CreateSpecific();
    }

    public DroneInputsState CreateSpecific()
    {
        Vector3 movementInput = _inputsController.MovementDirectionInputVector3Normalized;
        Vector2 cameraMovementInput = _inputsController.CameraMovementInput;
        bool isPlacingObject = _inputsController.IsPlacingObjectInputBeingPressed;
        DroneInputsState inputState = new DroneInputsState(movementInput, cameraMovementInput, isPlacingObject, _stateVariables.ClientTick);
        return inputState;
    }
}
