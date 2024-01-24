using System.Collections.Generic;
using UnityEngine;

public class PlayerInputsController
{
    private bool _enable = false;
    private readonly PlayerInputActions _inputActionAsset;
    private readonly Dictionary<InputsHandlerType, I_InputsHandler> _inputsHandlerMap;
    private I_InputsHandler _activeInputsHandler;

    public bool IsShootingInputBeingPressed => _enable ? _activeInputsHandler.IsShootingInputBeingPressed : default;
    public bool IsShootingInputFirstTimePressed => _enable ? _activeInputsHandler.IsShootingInputFirstTimePressed : default;
    public bool IsAimingInputBeingPressed => _enable ? _activeInputsHandler.IsAimingInputBeingPressed : default;
    public bool IsRunningInputBeingPressed => _enable ? _activeInputsHandler.IsRunningInputBeingPressed : default;
    public bool IsCrouchingOrStandingInputBeingPressed => _enable ? _activeInputsHandler.IsCrouchingOrStandingInputBeingPressed : default;
    public Vector2 MovementDirectionInputVector2Normalized => _enable ? _activeInputsHandler.MovementDirectionInputVector2Normalized : default;
    public Vector3 MovementDirectionInputVector3Normalized => _enable ? _activeInputsHandler.MovementDirectionInputVector3Normalized : default;
    public Vector2 CameraMovementInput => _enable ? _activeInputsHandler.CameraMovementInput : default;
    public bool IsSwitchWeaponInputBeingPressed => false;
    public bool IsRealoadingInputBeingPressed => _enable ? _activeInputsHandler.IsRealoadingInputBeingPressed : default;
    public bool IsPlacingObjectInputBeingPressed => _enable ? _activeInputsHandler.IsPlacingObjectInputBeingPressed : default;
    public bool IsSmashingInputBeingPressed => _enable ? _activeInputsHandler.IsSmashingInputBeingPressed : default;

    public PlayerInputsController(InputsHandlerType initialInputType)
    {
        _inputActionAsset = new PlayerInputActions();
        _inputsHandlerMap = new Dictionary<InputsHandlerType, I_InputsHandler>();

        _inputsHandlerMap.Add(InputsHandlerType.Player, new PlayerInputsHandler(_inputActionAsset));
        _inputsHandlerMap.Add(InputsHandlerType.Drone, new DroneInputsHandler(_inputActionAsset));

        _activeInputsHandler = _inputsHandlerMap[InputsHandlerType.Player];
        _activeInputsHandler.Enable();

        SwitchInputs(initialInputType);
    }

    public void SwitchInputs(InputsHandlerType newInputType)
    {
        if(_activeInputsHandler.Type == newInputType)
        {
            return;
        }

        if(_inputsHandlerMap.TryGetValue(newInputType, out var inputHandlerToSwitch))
        {
            _activeInputsHandler.Disable();
            _activeInputsHandler = inputHandlerToSwitch;
            _activeInputsHandler.Enable();
        }
    }

    public void Enable()
    {
        //Debug.Log("Enabled");
        _enable = true;
    }

    public void Disable()
    {
        //Debug.Log("DIS---abled");
        _enable = false;
    }
}
