using UnityEngine;

public class VillagerAnimations
{
    private readonly Animator _animator;
    
    private readonly int _animWalk;
    private readonly int _animIdle;
    private readonly int _animFishing;
    
    public VillagerAnimations(Animator animator)
    {
        _animator = animator;

        _animWalk = Animator.StringToHash("Villager_Walk");
        _animIdle = Animator.StringToHash("Villager_Idle");
        _animFishing = Animator.StringToHash("Villager_Fishing");
    }

    public void PlayWalk()
    {
        _animator.CrossFade(_animWalk, 0);
    }

    public void PlayIdle()
    {
        _animator.CrossFade(_animIdle, 0);
    }

    public void PlayFishing()
    {
        _animator.CrossFade(_animFishing, 0);
    }
}