using System.Collections;
using System.Linq;
using UnityEngine;

public class VillagerController : StateRunner<VillagerController>
{
    [SerializeField] private VillagerDetector _villagerDetector;
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _expressionsRenderer;

    public Transform VillagerTransform { get; private set; }
    public VillagerAnimations Animator;
    public VillagerExpressions Expressions;
    public Transform SpriteTransform;

    public bool CanMove(Vector2 destination) => _villagerDetector.CanMove(destination);

    private BezierRenderer _fishingBezierRenderer;
    private float _lastX = 1;
    private bool _isFishing;

    protected override void Awake()
    {
        Animator = new VillagerAnimations(_animator);
        Expressions = new VillagerExpressions(this, _expressionsRenderer);
        base.Awake();
        VillagerTransform = transform;
        
        _fishingBezierRenderer = GetComponentInChildren<BezierRenderer>();
    }

    private void OnEnable()
    {
        PopulationManager.Instance.OnPopulationReduced += HandlePopulationReduced;
        PopulationManager.Instance.OnPopulationIncreased += HandlePopulationIncreased;
    }

    private void OnDisable()
    {
        PopulationManager.Instance.OnPopulationReduced -= HandlePopulationReduced;
        PopulationManager.Instance.OnPopulationIncreased -= HandlePopulationIncreased;
    }

    private void LateUpdate()
    {
        if (!_villagerDetector.IsThereWater(Vector2.right * _lastX, out Vector2 tilePos) || 
            _isFishing || _activeState.GetType() == typeof(VillagerFishing)) return;
        
        _isFishing = true;
        
        SetState(VillagerFishing.CreateInstance(tilePos));
    }

    private void HandlePopulationReduced(int killed)
    {
        DoExpression(global::Expressions.Happy);
    }

    private void HandlePopulationIncreased(int amount)
    {
        DoExpression(global::Expressions.Sad);
    }

    private void DoExpression(Expressions type)
    {
        var expression = States.OfType<VillagerExpression>()
            .FirstOrDefault(ex => ex.ExpressionType == type);

        if (expression == null) return;

        var newExpression = VillagerExpression.Copy(
            expression.ExpressionType,
            expression._sprite,
            expression._expressionTime
        );
        SetState(newExpression);
    }

    public void FlipSprite(Vector2 direction)
    {
        _lastX = Mathf.Sign(direction.x);
        var localScale = SpriteTransform.localScale;
        localScale.x = _lastX;
        SpriteTransform.localScale = localScale;
    }

    public void StartFishing(Vector2 destinationPoint)
    {
        var startPoint = _villagerDetector.GetFishingRodStart();
        var midPoint = (startPoint + destinationPoint) / 2f;
        midPoint.y += 1.5f;
        _fishingBezierRenderer.Render(new[]{startPoint, midPoint, destinationPoint});
    }
    
    public void FinishFishing()
    {
        _fishingBezierRenderer.Finish();
        StartCoroutine(StopFishing());
    }

    private IEnumerator StopFishing()
    {
        yield return new WaitForSeconds(2f);
        _isFishing = false;
    }

    public void Move(Vector2 destination, float speed)
    {
        VillagerTransform.position = Vector2.MoveTowards(
            VillagerTransform.position,
            destination,
            speed * Time.deltaTime
        );
    }
}