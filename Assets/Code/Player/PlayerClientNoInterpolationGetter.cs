using System;
using UnityEngine;

/// <summary>
/// Installed on the source of the movement on client side (i.e., this is isbling of <see cref="UnityEngine.CharacterController"/>).
/// </summary>
[RequireComponent (typeof(CharacterController))]
public class PlayerClientNoInterpolationGetter : MonoBehaviour
{
    [SerializeField] private Transform _noInterpolatedPlayerPositionSource;
    [SerializeField] private Transform _noInterpolatedHeadTransform;

    private OwnedRotation<PlayerController> _noInterpolatedSplitRotationSource;
    private CharacterController _characterController;

    public Transform NoInterpolatedHeadTransform => _noInterpolatedHeadTransform;
    
    public OwnedRotation<PlayerController> NoInterpolatedSplitRotationSource
    {
        get => _noInterpolatedSplitRotationSource;
        set
        {
            if (_noInterpolatedSplitRotationSource != null) throw new ArgumentException("Already set.  Cannot change it.  Sorry.  I know this is strange, but this is the case.");

            _noInterpolatedSplitRotationSource = value;
        }
    }

    public CharacterController CharacterController => _characterController;
    public Vector3 NoInterpolatedPlayerPosition => _noInterpolatedPlayerPositionSource.position;
    public Vector3 NoInterpolatedPlayerForward => _noInterpolatedPlayerPositionSource.forward;
    public Vector3 NoInterpolatedPlayerRight => _noInterpolatedPlayerPositionSource.right;
    public Vector3 NoInterpolatedPlayerUp => _noInterpolatedPlayerPositionSource.up;
    public Transform NoInterpolatedPlayerPositionSource => _noInterpolatedPlayerPositionSource;

    public float rotationHorizontal, rotationVertical;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    public void SetNoInterpolatedPlayerPosition(Vector3 newPosition)
    {
        _noInterpolatedPlayerPositionSource.position = newPosition;
    }
}
