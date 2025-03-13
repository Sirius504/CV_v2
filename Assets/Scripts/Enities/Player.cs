using ActionBehaviour;
using System;
using UnityEngine;

public class Player : MonoEntity, ICellHabitant, IEnemyTarget, IUpdatable, IInjectable<Level>, IActioning, IAttackable, IAttacker
{
    private Level _level;
    private Health _health;

    [SerializeField] private int _damage;
    public int Damage => _damage;

    public event Action<ActionInfo> OnAction;

    public UpdateOrder UpdateOrder => UpdateOrder.Player;

    public void Inject(Level level)
    {
        _level = level;
        _health = GetComponent<Health>();
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
        else if (targetCell.TryGet(out IAttackable target) && target is not IEnemyTarget)
        {
            target.ReceiveAttack(this);
            OnAction?.Invoke(new ActionInfo(targetPosition, currentPosition, ActionBehaviour.Action.Attack));
        }
    }

    public void ReceiveAttack(IAttacker attacker)
    {
        _health.TakeDamage(attacker.Damage);
    }
}
