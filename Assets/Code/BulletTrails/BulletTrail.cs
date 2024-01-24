using UnityEngine;

[DisallowMultipleComponent]
public class BulletTrail : MonoBehaviour
{
    private Transform _transform;
    private Vector3 _destinationPosition;

    private void Awake()
    {
        _transform = transform;
    }

    public void SetDestinationPosition(Vector3 destination)
    {
        _destinationPosition = destination;
    }

    public void UpdateTrail(float speed)
    {
        _transform.position = Vector3.MoveTowards(_transform.position, _destinationPosition, Time.deltaTime * speed);
    }

    public bool HasReachedDestination()
    {
        Vector3 offset = _destinationPosition - _transform.position;
        if(Mathf.Abs(Vector3.SqrMagnitude(offset)) <= 0.2f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
