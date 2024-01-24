using UnityEngine;

public class CrouchStandController
{
    private readonly CharacterController _characterController;
    private readonly CrouchStandConfiguration _configuration;
    private readonly PlayerStateVariables _stateVariables;
    private readonly Transform _cameraTransform;

    private bool _isPerformingTransition = false;
    private float _currentTransitionTime = 0f;

    private float _targetCharacterControllerHeight;
    private float _originCharacterControllerHeight;

    private Vector3 _targetCharacterControllerCenter;
    private Vector3 _originCharacterControllerCenter;

    private float _targetVerticalPosition;
    private float _originVerticalPosition;

    public bool IsPerformingTransition => _isPerformingTransition;
    public float CurrentTransitionTime => _currentTransitionTime;
    public float TargetVerticalPosition => _targetVerticalPosition;
    public float OriginVerticalPosition => _originVerticalPosition;
    public Vector3 TargetCharacterControllerCenter => _targetCharacterControllerCenter;
    public Vector3 OriginCharacterControllerCenter => _originCharacterControllerCenter;
    public float TargetCharacterControllerHeight => _targetCharacterControllerHeight;
    public float OriginCharacterControllerHeight => _originCharacterControllerHeight;

    public void SetIsPerformingTransition(bool newValue)
    {
        _isPerformingTransition = newValue;
    }

    public void SetCurrentTransitionTime(float newValue)
    {
        _currentTransitionTime = newValue;
    }

    public void SetOriginVerticalPosition(float newValue)
    {
        _originVerticalPosition = newValue;
    }

    public void SetTargetVerticalPosition(float newValue)
    {
        _targetVerticalPosition = newValue;
    }

    public void SetTargetCharacterControllerCenter(Vector3 newValue)
    {
        _targetCharacterControllerCenter = newValue;
    }

    public void SetOriginCharacterControllerCenter(Vector3 newValue)
    {
        _originCharacterControllerCenter = newValue;
    }

    public void SetTargetCharacterControllerHeight(float newValue)
    {
        _targetCharacterControllerHeight = newValue;
    }

    public void SetOriginCharacterControllerHeight(float newValue)
    {
        _originCharacterControllerHeight = newValue;
    }

    public CrouchStandController(CharacterController characterController, CrouchStandConfiguration configuration, PlayerStateVariables stateVariables, Transform cameraTransform)
    {
        _configuration = configuration;
        _characterController = characterController;
        _stateVariables = stateVariables;
        _cameraTransform = cameraTransform;

        _stateVariables.SetIsCrouched(false);
        _originCharacterControllerHeight = _configuration.StandHeight;
        _originCharacterControllerCenter = _configuration.StandCharacterControllerCenter;
    }

    public void Simulate(bool isCrouchStandInputPressed, float elapsedTime)
    {
        if(isCrouchStandInputPressed)
        {
            DecideNextState();
        }

        if (!_isPerformingTransition)
        {
            return;
        }

        UpdateCrouching(elapsedTime);
    }

    private void DecideNextState()
    {
        if (CanStartACrouchStandAction())
        {
            return;
        }

        if (_stateVariables.IsCrouched)
        {
            if(!IsThereSomethingAbove())
            {
                StandUp();
            }
        }
        else
        {
            Crouch();
        }
    }

    private bool CanStartACrouchStandAction()
    {
        return _isPerformingTransition || !_characterController.isGrounded;
    }

    private void UpdateCrouching(float elapsedTime)
    {
        _currentTransitionTime += elapsedTime;

        InterpolateValues();

        if (_currentTransitionTime >= _configuration.CrouchSpeed)
        {
            AdjustPositions();
            ResetValues();
        }
    }

    private void InterpolateValues()
    {
        InterpolateCharacterControllerHeight();
        InterpolateCharacterControllerCenter();
        InterpolateCameraVerticalPosition();
    }

    private void InterpolateCharacterControllerHeight()
    {
        float characterControllerHeightInCurrentFrame = Mathf.Lerp(_originCharacterControllerHeight, _targetCharacterControllerHeight, _currentTransitionTime / _configuration.CrouchSpeed);
        _characterController.height = characterControllerHeightInCurrentFrame;
    }

    private void InterpolateCharacterControllerCenter()
    {
        Vector3 characterControllerCenterInCurrentFrame = Vector3.Lerp(_originCharacterControllerCenter, _targetCharacterControllerCenter, _currentTransitionTime / _configuration.CrouchSpeed);
        _characterController.center = characterControllerCenterInCurrentFrame;
    }

    private void InterpolateCameraVerticalPosition()
    {
        float frameHeightPosition = Mathf.Lerp(_originVerticalPosition, _targetVerticalPosition, _currentTransitionTime / _configuration.CrouchSpeed);
        _cameraTransform.position = new Vector3(_cameraTransform.position.x, frameHeightPosition, _cameraTransform.position.z);
    }

    private void AdjustPositions()
    {
        _characterController.height = _targetCharacterControllerHeight;
        _characterController.center = _targetCharacterControllerCenter;
        _cameraTransform.position = new Vector3(_cameraTransform.position.x, _targetVerticalPosition, _cameraTransform.position.z);
    }

    private void ResetValues()
    {
        _currentTransitionTime = 0f;
        _isPerformingTransition = false;

        _originCharacterControllerHeight = _targetCharacterControllerHeight;
        _originCharacterControllerCenter = _targetCharacterControllerCenter;
    }

    private bool IsThereSomethingAbove()
    {
        float rayDistance = _configuration.StandHeight - _configuration.CrouchHeight;

        if(Physics.Raycast(_cameraTransform.position, Vector3.up, rayDistance))
        {
            return true;
        }

        return false;
    }

    private void Crouch()
    {
        _stateVariables.SetIsCrouched(true);
        _isPerformingTransition = true;
        _targetCharacterControllerHeight = _configuration.CrouchHeight;
        _targetCharacterControllerCenter = _configuration.CrouchCharacterControllerCenter;
        SetVerticalPositions(true);
    }

    private void StandUp()
    {
        _stateVariables.SetIsCrouched(false);
        _isPerformingTransition = true;
        _targetCharacterControllerHeight = _configuration.StandHeight;
        _targetCharacterControllerCenter = _configuration.StandCharacterControllerCenter;
        SetVerticalPositions(false);
    }

    private void SetVerticalPositions(bool crouch)
    {
        if(crouch)
        {
            _targetVerticalPosition = _cameraTransform.position.y - (_configuration.StandHeight - _configuration.CrouchHeight);
        }
        else
        {
            _targetVerticalPosition = _cameraTransform.position.y + (_configuration.StandHeight - _configuration.CrouchHeight);
        }

        _originVerticalPosition = _cameraTransform.position.y;
    }
}
