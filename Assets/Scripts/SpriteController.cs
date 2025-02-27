using ActionBehaviour;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteController : MonoEntity, IInitializable, IUpdatable
{
    private IActioning _target;
    private SpriteRenderer _spriteRenderer;
    private SpriteActionProcessor _activeProcessor;

    private float _animationEndTime;

    [SerializeField] private Sprite _idleSprite;
    [SerializeField] private Vector2 _defaultOrientation = new(1f, 1f);
    [SerializeField] private SpriteProcessorsList _processorList;

    public UpdateOrder UpdateOrder => UpdateOrder.Animation;
    public InitOrder InitOrder => InitOrder.Animation;

    public void Init()
    {
        _target = transform.GetComponentInParent<IActioning>();
        _target.OnAction += OnTargetAction;
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void UpdateDirection(Vector2Int from, Vector2Int to)
    {
        var direction = to.x - from.x;
        if (direction == 0) return;
        var newScale = _spriteRenderer.transform.localScale;
        newScale.x = Mathf.Sign(direction) * _defaultOrientation.x * Mathf.Abs(newScale.x);
        _spriteRenderer.transform.localScale = newScale;
    }

    private void OnTargetAction(ActionInfo actionInfo)
    {
        UpdateDirection(actionInfo.From, actionInfo.To);

        if (!_processorList.TryGetProcessor(actionInfo.Action, out _activeProcessor))
        {
            CancelAnimation();
            return;
        }

        _activeProcessor.ProcessAction(_spriteRenderer, actionInfo);
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
}
 