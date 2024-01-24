using System;

public class AimingController
{
    public event Action OnAimOut;
    public event Action OnAimIn;

    private readonly PlayerStateVariables _stateVariables;

    public AimingController(PlayerStateVariables stateVariables)
    {
        _stateVariables = stateVariables;
    }

    public void UpdateAiming(bool isAimingInputBeingPressed)
    {
        if(isAimingInputBeingPressed && !_stateVariables.IsAiming)
        {
            AimIn();
        }
        else if(!isAimingInputBeingPressed && _stateVariables.IsAiming)
        {
            AimOut();
        }
    }

    private void AimIn()
    {
        _stateVariables.SetIsAiming(true);
        OnAimIn?.Invoke();
    }

    private void AimOut()
    {
        _stateVariables.SetIsAiming(false);
        OnAimOut?.Invoke();
    }
}
