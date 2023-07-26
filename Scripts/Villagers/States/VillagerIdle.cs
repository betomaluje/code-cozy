using UnityEngine;

[CreateAssetMenu(menuName = "States/Villagers/Idle")]
public class VillagerIdle : State<VillagerController>
{
    [SerializeField] private float _countDownTime = 5f;
    private float _timeRemaining;

    public override void Enter(VillagerController parent)
    {
        base.Enter(parent);

        _runner.Animator.PlayIdle();
        _timeRemaining = Random.Range(0, _countDownTime);
    }

    public override void Tick(float deltaTime)
    {
        _timeRemaining -= deltaTime;
    }

    public override void FixedTick(float fixedDeltaTime)
    {
    }

    public override void ChangeState()
    {
        if (_timeRemaining <= 0)
        {
            _runner.SetState(CreateInstance<VillagerMove>());
        }
    }
}