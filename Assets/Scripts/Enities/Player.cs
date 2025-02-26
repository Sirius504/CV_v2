using ActionBehaviour;
using System;
using UnityEngine;

public class Player : MonoEntity, ICellHabitant, IEnemyTarget, IUpdatable, IInjectable<Level>, IActioning
{
    private Level _level;

    [SerializeField] private int _damage;

    public event Action<ActionInfo> OnAction;

    public UpdateOrder UpdateOrder => UpdateOrder.Player;

    public void Inject(Level level)
    {
        _level = level;
    }

    private Vector2Int ReadInput()
    {
        var input = new Vector2Int(0, 0);
        input = Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) ? Vector2Int.up : input;
        input = Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) ? Vector2Int.down : input;
        input = Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow) ? Vector2Int.left : input;
        input = Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow) ? Vector2Int.right : input;

        return input;
    }

    public void UpdateManual()
    {
        var input = ReadInput();
        if (input == Vector2Int.zero)
        {
            return;
        }

        var currentPosition = _level.GetEntityPosition(this);
        var targetPosition = currentPosition + input;
        if (!_level.InBounds(targetPosition))
        {
            return;
        }

        var targetCell = _level.GetCell(targetPosition);
        if (targetCell.IsEmpty())
        {
            _level.Move(this, targetPosition);
            OnAction?.Invoke(new ActionInfo(targetPosition, currentPosition, ActionBehaviour.Action.Movement));
        }
        else if (targetCell.TryGet(out Enemy target))
        {
            var health = target.GetComponent<Health>();
            health.TakeDamage(_damage);
            OnAction?.Invoke(new ActionInfo(targetPosition, currentPosition, ActionBehaviour.Action.Attack));
        }
    }
}
