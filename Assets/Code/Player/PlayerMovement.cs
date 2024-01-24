using UnityEngine;

public class PlayerMovement
{
    private readonly PlayerMovementConfiguration _movementConfiguration = null;
    private readonly PlayerStateVariables _stateVariables;

    private Vector2 _movementInput;
    private Vector3 _smoothDampVelocity = Vector3.zero;
    private Vector3 _currentMovementVector = Vector3.zero;
    private Vector3 _velocityVector = Vector3.zero;
    private float _verticalSpeed = 0f;

    private const int GRAVITY = 15;

    public PlayerMovement(PlayerMovementConfiguration configuration, PlayerStateVariables stateVariables)
    {
        _movementConfiguration = configuration;
        _stateVariables = stateVariables;
    }

    public float VerticalSpeed => _verticalSpeed;
    public Vector3 SmoothDampVelocity => _smoothDampVelocity;
    public Vector3 CurrentMovementVector => _currentMovementVector;
    public Vector3 VelocityVector => _velocityVector;
    public Vector2 MovementInput => _movementInput;

    public void SetVerticalSpeed(float newValue)
    {
        _verticalSpeed = newValue;
    }

    public void SetSmoothDampVelocity(Vector3 newValue)
    {
        _smoothDampVelocity = newValue;
    }

    public void SetCurrentMovementVector(Vector3 newValue)
    {
        _currentMovementVector = newValue;
    }

    public Vector3 Simulate(Vector2 movementInput, Vector3 movementDirection, bool isRunningInputBeingPressed, bool isGrounded, bool isShootingInputBeingPressed, float elapsedTime)
    {
        _movementInput = movementInput;
        UpdateMovementVariables(movementInput, isRunningInputBeingPressed, isShootingInputBeingPressed);
        Vector3 velocityVector = CalculateMovementVelocity(movementDirection, isGrounded, elapsedTime);
        return velocityVector;
    }

    private void UpdateMovementVariables(Vector2 movementInput, bool isRunningInputBeingPressed, bool isShootingInputBeingPressed)
    {
        UpdateIsMoving(CheckIfIsMoving(movementInput));
        UpdateIsMovingBackwards(CheckIfIsMovingBackwards(movementInput));
        UpdateIsRunning(CheckIfIsRunning(isRunningInputBeingPressed, isShootingInputBeingPressed));
        UpdateIsWalking(CheckIfIsWalking());
    }

    private bool CheckIfIsMoving(Vector2 movementInput)
    {
        return Mathf.Abs(movementInput.x) >= 0.001f || Mathf.Abs(movementInput.y) >= 0.001f;
    }

    private void UpdateIsMoving(bool newState)
    {
        _stateVariables.SetIsMoving(newState);
    }

    private bool CheckIfIsMovingBackwards(Vector2 movementInput)
    {
        return _stateVariables.IsMoving && movementInput.y < 0.001f;
    }

    private void UpdateIsMovingBackwards(bool newState)
    {
        _stateVariables.SetIsMovingBackwards(newState);
    }

    private bool CheckIfIsRunning(bool isRunningInputBeingPressed, bool isShootingInputBeingPressed)
    {
        return isRunningInputBeingPressed && !_stateVariables.IsReloading && !_stateVariables.IsAiming && !_stateVariables.IsShooting && !isShootingInputBeingPressed && _stateVariables.IsMoving && !_stateVariables.IsMovingBackwards && !_stateVariables.IsCrouched;
    }

    private void UpdateIsRunning(bool newState)
    {
        _stateVariables.SetIsRunning(newState);
    }

    private bool CheckIfIsWalking()
    {
        return _stateVariables.IsMoving && !_stateVariables.IsRunning;
    }

    private void UpdateIsWalking(bool newState)
    {
        _stateVariables.SetIsWalking(newState);
    }

    private Vector3 CalculateMovementVelocity(Vector3 movementDirection, bool isGrounded, float elapsedTime)
    {
        _currentMovementVector = Vector3.SmoothDamp(_currentMovementVector, movementDirection, ref _smoothDampVelocity, _movementConfiguration.MovementAccelerationTime, Mathf.Infinity, elapsedTime);

        float velocityMultiplier = GetVelocityMultiplier();
        _velocityVector = _currentMovementVector * velocityMultiplier;

        CalculateVerticalVelocity(isGrounded, elapsedTime);
        _velocityVector.y += _verticalSpeed;

        return _velocityVector;
    }

    private float GetVelocityMultiplier()
    {
        float velocityMultiplier;

        if(_stateVariables.IsCrouched)
        {
            velocityMultiplier = _movementConfiguration.CrouchingSpeed;
        }
        else if (_stateVariables.IsRunning)
        {
            velocityMultiplier = _movementConfiguration.RunningSpeed;
        }
        else if(_stateVariables.IsAiming)
        {
            velocityMultiplier = _movementConfiguration.AimingSpeed;
        }
        else
        {
            velocityMultiplier = _movementConfiguration.WalkingSpeed;
        }

        return velocityMultiplier;
    }

    private void CalculateVerticalVelocity(bool isGrounded, float elapsedTime)
    {
        if(isGrounded)
        {
            _verticalSpeed = -0.1f;
        }
        else
        {
            _verticalSpeed -= GRAVITY * elapsedTime;
        }
    }
}
