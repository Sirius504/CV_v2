using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using VContainer;

// responsible for target selection and part recalculation
[RequireComponent(typeof(ICellHabitant))]
public class Plotter : MonoEntity, IInjectable<Level>, IUpdatable
{
    [Inject]
    private Astar _astar;
    private ICellHabitant _target;
    private List<Vector2Int> _currentPath;
    private Level _level;
    private ICellHabitant _owner;

    public UpdateOrder UpdateOrder => UpdateOrder.AI;

    public Vector2Int? Peek()
    {
        if (_currentPath == null || !_currentPath.Any()) return null;
        return _currentPath[1];
    }

    public void RemoveFirst()
    {
        _currentPath.RemoveAt(0);
    }


    // recalculate on
    //  - after tick
    //  - on level change
    //  - on target becoming untargetable
    //  - on entity added/removed from level

    public void Inject(Level level)
    {
        _level = level;
        _owner = GetComponent<ICellHabitant>();
    }


    private ICellHabitant FindTarget()
    {
        var ourPosition = _level.GetEntityPosition(_owner);
        var availableTargets = _level.Entities.OfType<IEnemyTarget>();
        if (!availableTargets.Any()) return null;
        var closestTargets = availableTargets
            .GroupBy(enemy => _level.Distance(ourPosition, _level.GetEntityPosition(enemy)))
            .OrderBy(group => group.Key)
            .FirstOrDefault()
            .ToList();  // TODO: Move target predicate out
        if (closestTargets.Count > 0)
        {
            var newTarget = closestTargets[Random.Range(0, closestTargets.Count)];
            return newTarget;
        }

        return null;
    }

    private void SelectNewTarget()
    {
        if (_target != null)
        {
            _target.OnDestroyEvent -= OnTargetDestroy;
            _target = null;
        }

        _target = FindTarget();
        if (_target == null) return;
        _target.OnDestroyEvent += OnTargetDestroy;
        RefreshPath();
    }

    private void RefreshPath()
    {
        if (_target == null)
        {
            _currentPath = new();
            return;
        }
        var ourPosition = _level.GetEntityPosition(_owner);
        var targetPosition = _level.GetEntityPosition(_target);
        _currentPath = _astar.FindPath(ourPosition, targetPosition, cellPosition => _level.GetCell(cellPosition).IsEmpty());      
    }

    private bool ValidPath(List<Vector2Int> currentPath, ICellHabitant _target)
    {
        var targetNull = _target == null;
        if (targetNull) return false;

        var isNull = currentPath == null;
        if (isNull) return false;

        var targetNotMoved = _level.GetEntityPosition(_target) == currentPath[^1];
        var pathIsClear = currentPath.Skip(1).Take(currentPath.Count - 2).All(cell => _level.GetCell(cell).IsEmpty());
        var atTheStartOfPath =  _level.GetEntityPosition(_owner) == _currentPath[0];

        return targetNotMoved
            && pathIsClear
            && atTheStartOfPath;
    }

    private void OnTargetDestroy(IDestroyable target)
    {
        SelectNewTarget();    
    }

    public void UpdateManual()
    {
        if (ValidPath(_currentPath, _target)) return;
        SelectNewTarget();
        RefreshPath();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (_target != null)
        {
            _target.OnDestroyEvent -= OnTargetDestroy;
        }
    }
}
