using UnityEngine;

public class PlayerClientInterpolator
{
    private readonly int _interpolationSpeed;

    private readonly PlayerClientNoInterpolationGetter _noInterpolationGetter;
    private readonly Transform _networkedTransform;
    private readonly Transform _networkedHeadTransform;
    private readonly OwnedRotation<PlayerController> _networkedSplitRotationHolder;
    private readonly PlayerFullBodyAnimatorController _fullBodyAnimationsController;

    public PlayerClientInterpolator(
        int interpolationSpeed, 
        PlayerClientNoInterpolationGetter noInterpolationGetter, 
        Transform networkedTransform, 
        Transform networkedHeadTransform,
        OwnedRotation<PlayerController> networkedSplitRotationHolder,
        PlayerFullBodyAnimatorController fullBodyAnimationsController)
    {
        _interpolationSpeed = interpolationSpeed;
        _noInterpolationGetter = noInterpolationGetter;
        _networkedTransform = networkedTransform;
        _networkedHeadTransform = networkedHeadTransform;
        _networkedSplitRotationHolder = networkedSplitRotationHolder;
        _fullBodyAnimationsController = fullBodyAnimationsController;
    }

    public void UpdateNetworkedTransform(Vector2 movementDirection, bool isCrouched, bool isReloading, bool isShooting)
    {
        _networkedTransform.position = _noInterpolationGetter.NoInterpolatedPlayerPositionSource.position;
        UpdateNetworkedRotation(_noInterpolationGetter.NoInterpolatedSplitRotationSource.rotation);
        InterpolateFullBodyAnimations(movementDirection, isCrouched, isReloading, isShooting);
    }

    public void UpdateFromNetworkedTransform(Quaternion networkedSplitRotation)
    {
        Vector3 position_previous = _noInterpolationGetter.NoInterpolatedPlayerPositionSource.position;
        _noInterpolationGetter.NoInterpolatedPlayerPositionSource.position = _networkedTransform.position;

        {
            _networkedSplitRotationHolder.rotation = networkedSplitRotation;

            Quaternion netRotationY = Quaternion.Euler(0, networkedSplitRotation.eulerAngles.y, 0);
            _noInterpolationGetter.NoInterpolatedPlayerPositionSource.rotation = netRotationY;
            _networkedTransform.rotation = netRotationY;

            Quaternion netRotationX = Quaternion.Euler(networkedSplitRotation.eulerAngles.x, 0, 0);
            _noInterpolationGetter.NoInterpolatedHeadTransform.localRotation = netRotationX;
            _networkedHeadTransform.localRotation = netRotationX;
        }

        //UpdatePosition(sourceTransform);
        //InterpolateCameraMovement();

        {
            Vector3 movement = _networkedTransform.position - position_previous; // TODO better to normalize this and not multiply by some magic nubmer below...that is likely framerate dependant
            float relativeHorizontalMovement = Vector3.Dot(movement, _networkedTransform.right);
            float relativeVerticalMovement = Vector3.Dot(movement, _networkedTransform.forward);
            Vector2 relativeMovementXZ = new Vector2(relativeHorizontalMovement, relativeVerticalMovement) * 16.6f; // this puts it into range of -1 to 1 when walking
            InterpolateFullBodyAnimations(relativeMovementXZ, default, default, default);
        }
    }

    private void UpdateNetworkedRotation(Quaternion inputControlledSplitTransform)
    {
        _networkedTransform.rotation = Quaternion.Euler(0, inputControlledSplitTransform.eulerAngles.y, 0);
        _networkedHeadTransform.localRotation = Quaternion.Euler(inputControlledSplitTransform.eulerAngles.x, 0, 0);
    }

    private void InterpolateFullBodyAnimations(Vector2 movementDirection, bool isCrouched, bool isReloading, bool isShooting)
    {
        _fullBodyAnimationsController.Update(
            Mathf.LerpAngle(
                _networkedHeadTransform.localEulerAngles.x, 
                _noInterpolationGetter.NoInterpolatedSplitRotationSource.rotation.eulerAngles.x, 
                Time.deltaTime * _interpolationSpeed), 
            movementDirection, 
            isCrouched, 
            isReloading, 
            isShooting);
    }

    public void PlayShootAnimation()
    {
        _fullBodyAnimationsController.PlayShootAnimation();
    }

    public void PlayReloadAnimation()
    {
        _fullBodyAnimationsController.PlayReloadAnimation();
    }

    public void PlaySmashAnimation()
    {
        _fullBodyAnimationsController.PlaySmashAnimation();
    }
}
