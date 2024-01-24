using UnityEngine;

public class DummyDroneFollow : MonoBehaviour
{
    public const float FOLLOW_ABOVE_DISTANCE = 10;

    [SerializeField] private Renderer _droneRenderer;
    private Transform _followTransform;
    private Transform _transform;
    private int _interpolationSpeed = 3;
    private bool _isEnable = true;

    private void Awake()
    {
        _transform = transform;
    }

    public void SetFollowTransform(Transform transform)
    {
        _followTransform = transform;
    }

    public void Enable()
    {
        _isEnable = true;
        SetVisualsVisibility(true);
    }

    public void Disable()
    {
        _isEnable = false;
        SetVisualsVisibility(false);
    }

    private void Update()
    {
        if (!_isEnable) return;

        Interpolate();
    }

    public void Interpolate()
    {
        Vector3 position = Vector3.zero;
        Quaternion rotation = Quaternion.identity;
        InterpolateMovement(out position);
        InterpolateRotation(out rotation);
        _transform.SetPositionAndRotation(position, rotation);
    }

    private void InterpolateMovement(out Vector3 position)
    {
        position = Vector3.Lerp(_transform.position, _followTransform.position, Time.deltaTime * _interpolationSpeed);
        position.y = _followTransform.position.y + FOLLOW_ABOVE_DISTANCE;
    }

    private void InterpolateRotation(out Quaternion rotation)
    {
        Vector3 lerp = Vector3.Lerp(_transform.localEulerAngles, _followTransform.localEulerAngles, Time.deltaTime * _interpolationSpeed);
        lerp.x = 0;
        rotation = Quaternion.Euler(lerp);
    }

    private void SetVisualsVisibility(bool visibility)
    {
        _droneRenderer.enabled = visibility;
    }
}
