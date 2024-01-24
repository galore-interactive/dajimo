using UnityEngine;

public interface IPlayerAnimationsController
{
    public void Update(bool isWalking, bool isRunning, bool isCrouched, bool isAiming, float verticalAngle);
    public void SetRuntimeAnimationController(RuntimeAnimatorController newAnimatorController);
    public void PlayAimInAnimation();
    public void PlayAimOutAnimation();
    public void PlayShootAnimation();
}
