using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteController : MonoEntity, IInitializable, IUpdatable, IInjectable<Level>
{
    private SpriteRenderer _spriteRenderer;
    private SpriteActionProcessor _activeProcessor;
    private Level _level;

    private EventBinding<AttackEvent> attackBinding;
    private EventBinding<BlockingEvent> blockingBinding;
    private float _animationEndTime;

    [SerializeField] private Sprite _idleSprite;
    [SerializeField] private Vector2 _defaultOrientation = new(1f, 1f);
    [SerializeField] private SpriteProcessorsList _processorList;

    public UpdateOrder UpdateOrder => UpdateOrder.Animation;
    public InitOrder InitOrder => InitOrder.Animation;

    public void Inject(Level level)
    {
        _level = level;
    }

    public void Init()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        var attacker = GetComponentInParent<IAttacker>();
        var attackable = GetComponentInParent<IAttackable>();
        var entity = GetComponentInParent<ICellHabitant>();

        bool attackEventPredicate(AttackEvent @event) => @event.attacker == attacker;
        attackBinding = new EventBinding<AttackEvent>(OnAttack, attackEventPredicate);
        EventBus<AttackEvent>.Register(attackBinding);

        bool blockingEventPredicate(BlockingEvent @event) => @event.entity == entity;
        blockingBinding = new EventBinding<BlockingEvent>(OnBlock, blockingEventPredicate);
        EventBus<BlockingEvent>.Register(blockingBinding);
    }

    private void UpdateDirection(Vector2Int from, Vector2Int to)
    {
        var direction = to.x - from.x;
        if (direction == 0) return;
        var newScale = _spriteRenderer.transform.localScale;
        newScale.x = Mathf.Sign(direction) * _defaultOrientation.x * Mathf.Abs(newScale.x);
        _spriteRenderer.transform.localScale = newScale;
    }

    private void OnBlock(BlockingEvent @event)
    {
        var ourPosition = _level.GetEntityPosition(@event.entity);
        UpdateDirection(ourPosition, @event.attackSource);

        if (!_processorList.TryGetProcessor(ActionBehaviour.Action.Block, out _activeProcessor))
        {
            CancelAnimation();
            return;
        }

        _activeProcessor.ProcessAction(_spriteRenderer, @event);
        _animationEndTime = Time.time + _activeProcessor.Duration;
    }

    private void OnAttack(AttackEvent attackEvent)
    {
        var from = _level.GetEntityPosition(attackEvent.attacker);
        var to = _level.GetEntityPosition(attackEvent.attackable);
        UpdateDirection(from, to);

        if (!_processorList.TryGetProcessor(ActionBehaviour.Action.Attack, out _activeProcessor))
        {
            CancelAnimation();
            return;
        }

        _activeProcessor.ProcessAction(_spriteRenderer, attackEvent);
        _animationEndTime = Time.time + _activeProcessor.Duration;
    }

    private void CancelAnimation()
    {
        _spriteRenderer.sprite = _idleSprite;
        _activeProcessor = null;
    }

    public void UpdateManual()
    {
        if (_activeProcessor && Time.time >= _animationEndTime)
            CancelAnimation();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        EventBus<AttackEvent>.Deregister(attackBinding);
        EventBus<BlockingEvent>.Deregister(blockingBinding);
    }
}
 