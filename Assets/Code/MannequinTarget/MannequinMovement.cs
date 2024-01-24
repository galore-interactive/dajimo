using UnityEngine;

public class MannequinMovement
{
    private readonly float _movementSpeed;
    private Vector3 _currentPosition;
    private readonly Transform _waypoint1;
    private readonly Transform _waypoint2;
    private Transform _targetWaypoint;
    private int _targetWaypointIndex;

    public Vector3 CurrentPosition => _currentPosition;
    public void SetCurrentPosition(Vector3 newPosition)
    {
        _currentPosition = newPosition;
    }

    public MannequinMovement(Transform wayPoint1, Transform wayPoint2, float movementSpeed)
    {
        _waypoint1 = wayPoint1;
        _waypoint2 = wayPoint2;
        _movementSpeed = movementSpeed;
        _targetWaypoint = wayPoint2;
        _targetWaypointIndex = 2;
        _currentPosition = wayPoint1.position;
    }

    public void SimulateMovement(float elapsedTime)
    {
        _currentPosition = CalculateNextPosition(elapsedTime);
        if (CheckIfHasReachedTarget())
        {
            SetNewTargetWaypoint();
        }
    }

    private Vector3 CalculateNextPosition(float elapsedTime)
    {
        return Vector3.MoveTowards(_currentPosition, _targetWaypoint.position, elapsedTime * _movementSpeed);
    }

    private bool CheckIfHasReachedTarget()
    {
        float distanceToTarget = Vector3.Distance(_targetWaypoint.position, _currentPosition);
        if(distanceToTarget <= 0.1f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void SetNewTargetWaypoint()
    {
        _currentPosition = _targetWaypoint.position;
        if (_targetWaypointIndex == 1)
        {
            _targetWaypointIndex = 2;
            _targetWaypoint = _waypoint2;
        }
        else
        {
            _targetWaypointIndex = 1;
            _targetWaypoint = _waypoint1;
        }
    }
}
