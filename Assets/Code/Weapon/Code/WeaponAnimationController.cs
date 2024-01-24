using UnityEngine;

public class WeaponAnimationController
{
    private readonly Animator _animator;

    public WeaponAnimationController(Animator animator)
    {
        _animator = animator;
    }

    public void PlayShootAnimation()
    {
        _animator.CrossFade("Fire", 0.05f, -1, 0f);
    }

    public void PlayReloadAnimation()
    {
        _animator.CrossFade("Reload", 0.05f, -1, 0f);
    }
}
