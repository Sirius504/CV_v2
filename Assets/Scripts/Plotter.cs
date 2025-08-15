using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using VContainer;

// responsible for target selection and path recalculation
[RequireComponent(typeof(ICellEntity))]
public class Plotter : CellComponent, IInitializable, IUpdatable
{
    [Inject]
    private Astar _astar;
    [Inject]
    private Level _level;
    private ICellEntity _target;
    private List<Vector2Int> _currentPath;

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

    private ICellEntity FindTarget()
    {
        var ourPosition = _level.GetEntityPosition(Entity);
        bool predicate(ICellEntity entity) => entity.Has<IEnemyTarget>();

        var availableTargets = _level.Entities.Where(predicate);
        if (!availableTargets.Any()) return null;
        var closestTargets = availableTargets
            .GroupBy(target => _level.Distance(ourPosition, _level.GetEntityPosition(target)))
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

    private void PlotToNewTarget()
    {
        if (_target != null)
        {
            _target.OnDestroyEvent -= OnTargetDestroy;
            _target = null;
        }

        _target = FindTarget();
        if (_target == null)
        {
            _currentPath = new();
            return;
        }
        
        _target.OnDestroyEvent += OnTargetDestroy;
        var ourPosition = _level.GetEntityPosition(Entity);
        var targetPosition = _level.GetEntityPosition(_target);
        _currentPath = _astar.FindPath(ourPosition, targetPosition, cellPosition => _level.GetCell(cellPosition).IsEmpty());
    }


    private bool ValidPath(List<Vector2Int> currentPath, ICellEntity _target)
    {
        var targetNull = _target == null;
        if (targetNull) return false;

        var isNull = currentPath == null;
        if (isNull) return false;

        var targetNotMoved = _level.GetEntityPosition(_target) == currentPath[^1];
        var pathIsClear = currentPath.Skip(1).Take(currentPath.Count - 2).All(cell => _level.GetCell(cell).IsEmpty());
        var atTheStartOfPath =  _level.GetEntityPosition(Entity) == _currentPath[0];

        return targetNotMoved
            && pathIsClear
            && atTheStartOfPath;
    }

    private void OnTargetDestroy(IDestroyable target)
    {
        PlotToNewTarget();    
    }

    public void UpdateManual()
    {
        if (ValidPath(_currentPath, _target)) return;
        PlotToNewTarget();
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
