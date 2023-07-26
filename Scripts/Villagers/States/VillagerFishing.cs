using UnityEngine;

[CreateAssetMenu(menuName = "States/Villagers/Fishing")]
public class VillagerFishing : State<VillagerController>
{
    [SerializeField] private float _fishingRadius = 1.5f;
    [SerializeField] private float _timeForFishing = 1.5f;
    [SerializeField] private int _amountToFish = 1;
    [SerializeField, Range(0f, 1f)] private float _chanceOfCatching = .75f;
    [SerializeField, Range(0f, 1f)] private float _chanceToStopFishing = .5f;
    
    private Vector2 _initialWaterPoint;
    private Vector2 _fishingPoint;
    private float _timeRemaining;

    public static VillagerFishing CreateInstance(Vector2 initialPosition)
    {
        var data = CreateInstance<VillagerFishing>();
        data._initialWaterPoint = initialPosition;
        return data;
    }

    public override void Enter(VillagerController parent)
    {
        base.Enter(parent);
        _fishingPoint = GetRandomPoint(_fishingRadius);
        _runner.Animator.PlayFishing();
        _timeRemaining = _timeForFishing;
        _runner.StartFishing(_fishingPoint);

        var direction = (_fishingPoint - (Vector2) _runner.VillagerTransform.position).normalized;
        _runner.FlipSprite(direction);
    }

    private Vector2 GetRandomPoint(float radius)
    {
        var randomPoint = Random.insideUnitCircle + Vector2.one * (radius * RandomSign());
        return _initialWaterPoint + randomPoint;
    }

    private static int RandomSign() => Random.value < 0.5f ? 1 : -1;

    public override void Tick(float deltaTime)
    {
        _timeRemaining -= deltaTime;
    }

    public override void FixedTick(float fixedDeltaTime)
    {
    }

    public override void ChangeState()
    {
        if (!(_timeRemaining <= 0)) return;

        _runner.FinishFishing();
        
        // check if fished something or not
        if (_chanceOfCatching >= Random.value)
        {
            MoneyManager.Instance.AddMoney(_amountToFish);
        }

        // finally check if we continue fishing or go to idle
        if (_chanceToStopFishing >= Random.value)
        {
            _runner.SetState(CreateInstance<VillagerMove>());
        }
        else
        {
            _runner.SetState(CreateInstance(_initialWaterPoint));
        }
    }
}