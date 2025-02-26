using ActionBehaviour;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteController : MonoEntity, IInitializable, IUpdatable
{
    public UpdateOrder UpdateOrder => UpdateOrder.Animation;
    public InitOrder InitOrder => InitOrder.Animation;

    [SerializeField] private Sprite _attackSprite;
    [SerializeField] private Sprite _idleSprite;
    [SerializeField] private float _attackAnimationDuration = 0.1f;
    [SerializeField] private Vector2 _defaultOrientation = new(1f, 1f);

    private IActioning _target;
    private SpriteRenderer _spriteRenderer;
    private bool _attackAnimation;
    private float _animationEndTime;

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
        if (actionInfo.Action == Action.Movement)
        {
            CancelAnimation();
            return;
        }

        _spriteRenderer.sprite = _attackSprite;
        _attackAnimation = true;
        _animationEndTime = Time.time + _attackAnimationDuration;
    }

    private void CancelAnimation()
    {
        if (!_attackAnimation) return;
        _spriteRenderer.sprite = _idleSprite;
        _attackAnimation = false;
    }

    public void UpdateManual()
    {
        if (_attackAnimation && Time.time >= _animationEndTime)
            CancelAnimation();
    }
}
