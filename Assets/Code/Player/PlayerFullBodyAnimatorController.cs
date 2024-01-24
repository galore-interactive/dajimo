using UnityEngine;

public class PlayerFullBodyAnimatorController
{
    private readonly Animator _animator;

    private int _verticalLookAtAnimatorParameterHash;
    private int _aimingValueAnimatorParameterHash;
    private int _verticalMovementAnimatorParameterHash;
    private int _horizontalMovementAnimatorParameterHash;
    private int _movementTypeAnimatorParameterHash;
    private int _isReloadingAnimatorParameterHash;
    private int _isShootingAnimatorParameterHash;
    private int _isSmashingAnimatorParameterHash;

    private int _weaponAnimatorLayerIndex;

    public PlayerFullBodyAnimatorController(Animator animator)
    {
        _animator = animator;

        _verticalLookAtAnimatorParameterHash = Animator.StringToHash("VerticalLookAt");
        _aimingValueAnimatorParameterHash = Animator.StringToHash("AimingValue");
        _verticalMovementAnimatorParameterHash = Animator.StringToHash("VerticalMovementDirection");
        _horizontalMovementAnimatorParameterHash = Animator.StringToHash("HorizontalMovementDirection");
        _movementTypeAnimatorParameterHash = Animator.StringToHash("MovementType");
        _isReloadingAnimatorParameterHash = Animator.StringToHash("IsReloading");
        _isShootingAnimatorParameterHash = Animator.StringToHash("IsShooting");
        _isSmashingAnimatorParameterHash = Animator.StringToHash("IsSmashing");

        _weaponAnimatorLayerIndex = _animator.GetLayerIndex("WeaponLayer");
    }

    public void PlayAimInAnimation()
    {

    }

    public void PlayAimOutAnimation()
    {

    }

    public void PlayShootAnimation()
    {
        _animator.SetBool(_isShootingAnimatorParameterHash, true);
        _animator.Play("WeaponShoot", _weaponAnimatorLayerIndex, 0f);
    }

    public void PlayReloadAnimation()
    {
        _animator.SetBool(_isReloadingAnimatorParameterHash, true);
        _animator.Play("WeaponReload", _weaponAnimatorLayerIndex, 0f);
    }

    public void PlaySmashAnimation()
    {
        _animator.SetBool(_isSmashingAnimatorParameterHash, true);
        _animator.Play("WeaponSmash", _weaponAnimatorLayerIndex, 0f);
    }

    public void SetRuntimeAnimationController(RuntimeAnimatorController newAnimatorController)
    {

    }

    public void Update(float verticalAngle, Vector2 movementDirection, bool isCrouched, bool isReloading, bool isShooting)
    {
        _animator.SetFloat(_verticalLookAtAnimatorParameterHash, CalculateVerticalAngle(verticalAngle, 80, -80));
        _animator.SetFloat(_horizontalMovementAnimatorParameterHash, movementDirection.x, 0.1f, Time.deltaTime);
        _animator.SetFloat(_verticalMovementAnimatorParameterHash, movementDirection.y, 0.1f, Time.deltaTime);

        if(isCrouched)
        {
            _animator.SetFloat(_movementTypeAnimatorParameterHash, 0, 0.1f, Time.deltaTime);
        }
        else
        {
            _animator.SetFloat(_movementTypeAnimatorParameterHash, 1, 0.1f, Time.deltaTime);
        }

        if(!_animator.GetBool(_isReloadingAnimatorParameterHash) && isReloading)
        {
            PlayReloadAnimation();
        }

        if(isShooting && !_animator.GetBool(_isShootingAnimatorParameterHash))
        {
            PlayShootAnimation();
        }
        else if(!isShooting && _animator.GetBool(_isShootingAnimatorParameterHash))
        {
            _animator.SetBool(_isShootingAnimatorParameterHash, false);
        }
    }

    private float CalculateVerticalAngle(float angle, float maxAngle, float minAngle)
    {
        float animatorAngle = 0f;

        if (angle <= 90)
        {
            animatorAngle = angle / minAngle;
        }
        else if(angle >= 270 && angle <= 360)
        {
            angle = 360 - angle;
            animatorAngle = angle / maxAngle;
        }

        return animatorAngle;
    }

    public void Shoot()
    {
        PlayShootAnimation();
    }

    public void Reload()
    {
        if (!_animator.GetBool(_isReloadingAnimatorParameterHash))
        {
            PlayReloadAnimation();
        }
    }
}
