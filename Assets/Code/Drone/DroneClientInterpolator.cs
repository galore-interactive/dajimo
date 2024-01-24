using UnityEngine;

public class DroneClientInterpolator
{
    private readonly PlayerClientNoInterpolationGetter _noInterpolationGetter;
    private readonly Transform _networkedTransform;
    private readonly Transform _networkedHeadTransform;

    public DroneClientInterpolator(
        PlayerClientNoInterpolationGetter noInterpolationGetter, 
        Transform networkedTransform, 
        Transform networkedHeadTransform)
    {
        _noInterpolationGetter = noInterpolationGetter;
        _networkedTransform = networkedTransform;
        _networkedHeadTransform = networkedHeadTransform;
    }

    /* keeping for reference
    private void InterpolateMovement()
    {
        //_playerInterpolatedTransform.position = Vector3.Lerp(_playerInterpolatedTransform.position, _noInterpolationGetter.NoInterpolatedPlayerTransform.position, Time.deltaTime * _interpolationSpeed);
        _playerInterpolatedTransform.position = Vector3.Lerp(_playerInterpolatedTransform.position, _noInterpolationGetter.NoInterpolatedHeadTransform.position, Time.deltaTime * _interpolationSpeed);
    }

    private void InterpolateCameraMovement()
    {
        _networkedHeadTransform.localEulerAngles = Vector3.right * (Mathf.LerpAngle(_networkedHeadTransform.localEulerAngles.x, _noInterpolationGetter.NoInterpolatedHeadTransform.localEulerAngles.x, Time.deltaTime * _interpolationSpeed));
        _playerInterpolatedTransform.rotation = Quaternion.Euler(Vector3.up * (Mathf.LerpAngle(_playerInterpolatedTransform.localEulerAngles.y, _noInterpolationGetter.NoInterpolatedPlayerPositionSource.localEulerAngles.y, Time.deltaTime * _interpolationSpeed)));
    }
    */

    internal void UpdateNetworkedTransform()
    {
        _networkedTransform.position = _noInterpolationGetter.NoInterpolatedPlayerPositionSource.position;
        _networkedTransform.rotation =
            Quaternion.Euler(0, _noInterpolationGetter.NoInterpolatedPlayerPositionSource.localEulerAngles.y, 0);
        _networkedHeadTransform.localRotation =
            Quaternion.Euler(_noInterpolationGetter.NoInterpolatedHeadTransform.localEulerAngles.x, 0, 0);
    }

    internal void UpdateFromNetworkedTransform()
    {
        _noInterpolationGetter.NoInterpolatedPlayerPositionSource.position = _networkedTransform.position;
        _noInterpolationGetter.NoInterpolatedPlayerPositionSource.localRotation = _networkedTransform.rotation;
        _noInterpolationGetter.NoInterpolatedHeadTransform.localRotation = _networkedHeadTransform.localRotation;
    }
}
