using UnityEngine;

[CreateAssetMenu(menuName = "States/Villagers/Expression")]
public class VillagerExpression : State<VillagerController>
{
    public Expressions ExpressionType;
    public Sprite _sprite;
    public float _expressionTime = 1f;

    private float _timeRemaining;

    public static VillagerExpression Copy(Expressions type, Sprite sprite, float time)
    {
        var newExpression = CreateInstance<VillagerExpression>();

        newExpression.ExpressionType = type;
        newExpression._sprite = sprite;
        newExpression._expressionTime = time;

        return newExpression;
    }

    public override void Enter(VillagerController parent)
    {
        base.Enter(parent);
        _runner.Expressions.DoExpression(_sprite, _expressionTime);
        _timeRemaining = _expressionTime;
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
            _runner.SetState(CreateInstance<VillagerIdle>());
        }
    }
}