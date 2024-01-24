using UnityEngine;

public class DroneMovement
{
    private readonly DroneMovementConfiguration _configuration;

    private Vector3 _currentMovementVector;
    private Vector3 _smoothDampVelocity;
    private Vector3 _velocityVector;
    private Vector2 _movementInput;

    public DroneMovement(DroneMovementConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Vector3 SmoothDampVelocity => _smoothDampVelocity;
    public Vector3 CurrentMovementVector => _currentMovementVector;
    public Vector3 VelocityVector => _velocityVector;
    public Vector2 MovementInput => _movementInput;

    public void SetSmoothDampVelocity(Vector3 newValue)
    {
        _smoothDampVelocity = newValue;
    }

    public void SetCurrentMovementVector(Vector3 newValue)
    {
        _currentMovementVector = newValue;
    }

    public Vector3 SimulateMovement(Vector2 movementInput, Vector3 movementDirection, float elapsedTime)
    {
        _movementInput = movementInput;
        CalculateMovementVelocity(movementDirection, elapsedTime);
        return _velocityVector;
    }

    private void CalculateMovementVelocity(Vector3 movementDirection, float elapsedTime)
    {
        _currentMovementVector = Vector3.SmoothDamp(_currentMovementVector, movementDirection, ref _smoothDampVelocity, _configuration.MovementAccelerationTime, Mathf.Infinity, elapsedTime);

        _velocityVector = _currentMovementVector * _configuration.MovementSpeed;
    }
}
