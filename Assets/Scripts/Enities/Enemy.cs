using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : MonoEntity, ICellHabitant, ITickable, IInjectable<Level>, IActionTelegraph, IUpdatable
{
    [SerializeField] private int _damage;

    private Level _level;

    private IEnemyTarget _target;
    private List<Vector2Int> _currentPath;

    public ActionInfo? ActionInfo { get; private set; } = null;

    public UpdateOrder UpdateOrder => UpdateOrder.System;

    public event Action<Vector2Int> OnMove;

    public void Inject(Level level)
    {
        _level = level;
    }
    public void UpdateManual()
    {
        if (_target == null || !_level.Entities.Contains(_target))
        {
            _target = SelectTarget();
            _currentPath = null;
        }

        if (_target == null)
        {
            // no valid target exists
            ActionInfo = null;
            return;
        }

        var targetPosition = _level.GetEntityPosition(_target);

        if (!ValidPath(_currentPath, _target))
        {
            _currentPath = _level.Astar.FindPath(_level.GetEntityPosition(this), targetPosition, cell => cell.IsEmpty());
        }

        if (_currentPath == null)
        {
            // valid path to target doesn't exist
            ActionInfo = null;
            return;
        }

        var actionType = _level.GetCell(_currentPath[1]).Has<IEnemyTarget>() ? Action.Attack : Action.Movement;
        ActionInfo = new ActionInfo(_currentPath[1], _level.GetEntityPosition(this), actionType);
    }

    public void OnTick(uint tick)
    {
        if (_target == null || _currentPath == null)
        {
            // target or valid path to target don't exist
            return;
        }

        var targetPosition = _level.GetEntityPosition(_target);
        var target = _currentPath[1];
        if (targetPosition == target)
        {
            Attack(targetPosition);
            return;
        }

        _level.Move(this, target);
        OnMove?.Invoke(target);
        _currentPath.RemoveAt(0);
    }

    private void Attack(Vector2Int targetPosition)
    {
        var targetCell = _level.GetCell(targetPosition);
        if (targetCell.TryGet(out IEnemyTarget targetEntity))
        {
            var health = ((MonoBehaviour)targetEntity).GetComponent<Health>();
            health.TakeDamage(_damage);
        }
    }

    private bool ValidPath(List<Vector2Int> currentPath, IEnemyTarget _target)
    {
        var notNull = currentPath != null;
        var targetNotMoved = notNull && _level.GetEntityPosition(_target) == currentPath[^1];
        var pathIsClear = targetNotMoved && currentPath.All(cell => _level.GetCell(cell).IsEmpty());
        var atTheStartOfPath = pathIsClear && _level.GetEntityPosition(this) == _currentPath[0];

        return notNull
            && targetNotMoved
            && pathIsClear
            && atTheStartOfPath;
    }

    private IEnemyTarget SelectTarget()
    {
        var availableTargets = _level.Entities.OfType<IEnemyTarget>().ToList();
        if (availableTargets.Count > 0)
        {
            var newTarget = availableTargets[UnityEngine.Random.Range(0, availableTargets.Count)];
            return newTarget;
        }

        return null;
    }

}