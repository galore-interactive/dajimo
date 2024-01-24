using UnityEngine;

public class PlayerAnimationController : IPlayerAnimationsController
{
    private const string FIRE = "Fire";
    private const string RELOAD = "Reload";

    private readonly Animator _animator;
    private readonly int _holsterAnimatorLayerIndex;
    private readonly int _actionsAnimatorLayerIndex;
    private readonly int _overlayAnimatorLayerIndex;

    private readonly int _isAimingValueHash;
    private readonly int _isRunningValueHash;
    private readonly int _movementSpeedValueHash;
    private readonly int _aimingAlphaSpeedValueHash;

    public PlayerAnimationController(Animator animator)
    {
        _animator = animator;
        _animator.updateMode = AnimatorUpdateMode.Normal;

        _isAimingValueHash = Animator.StringToHash("Aim");
        _isRunningValueHash = Animator.StringToHash("Running");
        _movementSpeedValueHash = Animator.StringToHash("Movement");
        _aimingAlphaSpeedValueHash = Animator.StringToHash("Aiming");

        _holsterAnimatorLayerIndex = _animator.GetLayerIndex("Layer Holster");
        _actionsAnimatorLayerIndex = _animator.GetLayerIndex("Layer Actions");
        _overlayAnimatorLayerIndex = _animator.GetLayerIndex("Layer Overlay");
    }

    public void SetRuntimeAnimationController(RuntimeAnimatorController newAnimatorController)
    {
        _animator.runtimeAnimatorController = newAnimatorController;
    }

    public void Update(bool isWalking, bool isRunning, bool isCrouched, bool isAiming, float verticalAngle)
    {
        if(_animator.runtimeAnimatorController == null)
        {
            return;
        }

        float movementSpeed;
        if(isWalking && isCrouched)
        {
            movementSpeed = 0.5f;
        }
        else if(isWalking && !isCrouched)
        {
            movementSpeed = 1f;
        }
        else
        {
            movementSpeed = 0f;
        }

        _animator.SetFloat(_movementSpeedValueHash, movementSpeed, 0.3f, Time.deltaTime);
        _animator.SetFloat(_aimingAlphaSpeedValueHash, (isAiming)?1f:0f, 0.1f, Time.deltaTime);
        _animator.SetBool(_isRunningValueHash, isRunning);
    }

    public void PlayAimInAnimation()
    {
        _animator.SetBool(_isAimingValueHash, true);
    }

    public void PlayAimOutAnimation()
    {
        _animator.SetBool(_isAimingValueHash, false);
    }

    public void PlayShootAnimation()
    {
        _animator.CrossFade(FIRE, 0.05f, _overlayAnimatorLayerIndex, 0f);
    }

    public void PlayReloadAnimation(float startNormalizedFraction = 0f)
    {
        _animator.Play(RELOAD, _actionsAnimatorLayerIndex, startNormalizedFraction);
    }
}
