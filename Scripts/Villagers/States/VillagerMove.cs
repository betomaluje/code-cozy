using UnityEngine;

[CreateAssetMenu(menuName = "States/Villagers/Move")]
public class VillagerMove : State<VillagerController>
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _movementRadius = 3f;
    [SerializeField] private float _arrivalDistance = .1f;

    private Vector2 _destination;

    public override void Enter(VillagerController parent)
    {
        base.Enter(parent);
        _destination = GetRandomPoint(_movementRadius);
        _runner.Animator.PlayWalk();
        
        if (!_runner.CanMove(_destination))
        {
            _runner.SetState(CreateInstance<VillagerIdle>());
            return;
        }

        var direction = (_destination - (Vector2) _runner.VillagerTransform.position).normalized;
        _runner.FlipSprite(direction);
    }

    private Vector2 GetRandomPoint(float radius)
    {
        var randomPoint = Random.insideUnitCircle * radius;
        return (Vector2) _runner.VillagerTransform.position + randomPoint;
    }

    public override void Tick(float deltaTime)
    {
        _runner.Move(_destination, _speed);
    }

    public override void FixedTick(float fixedDeltaTime)
    {
    }

    public override void ChangeState()
    {
        var dist = Vector2.Distance(_destination, _runner.VillagerTransform.position);

        if (dist < _arrivalDistance)
        {
            _runner.SetState(CreateInstance<VillagerIdle>());
        }
    }
}