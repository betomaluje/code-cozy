using UnityEngine;

public abstract class State<T> : ScriptableObject where T : MonoBehaviour
{
    protected T _runner;
    public virtual void Enter(T parent)
    {
        _runner = parent;
    }
    public abstract void Tick(float deltaTime);
    public abstract void FixedTick(float fixedDeltaTime);
    public abstract void ChangeState();
    public abstract void Exit();

    protected float GetNormalizedTime(Animator animator, string tag)
    {
        AnimatorStateInfo currentInfo = animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextInfo = animator.GetNextAnimatorStateInfo(0);

        if (animator.IsInTransition(0) && nextInfo.IsTag(tag))
        {
            return nextInfo.normalizedTime;
        }
        else if (!animator.IsInTransition(0) && currentInfo.IsTag(tag))
        {
            return currentInfo.normalizedTime;
        }
        else
        {
            return 0f;
        }
    }

}