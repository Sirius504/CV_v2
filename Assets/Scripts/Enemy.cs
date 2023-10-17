using System.Linq;
using UnityEngine;

public class Enemy : MyMono, ICellHabitant, ITickable
{
    private Level _level;

    private IEnemyTarget _target;

    public void Init(Level level)
    {
        _level = level;
    }

    private void Update()
    {
        transform.position = _level.CellToWorld(this);
    }

    public void OnTick(uint tick)
    {
        if (_target == null || !_level.Entities.Contains(_target))
        {
            _target = SelectTarget();
        }

        var path = _level.Astar.FindPath(cell => cell.IsEmpty(), _level.GetEntityPosition(this), _level.GetEntityPosition(_target));        

        var adjacent = _level.GetAdjacentCells(this)
            .Where(cell => cell.IsEmpty())
            .Select(cell => cell.Position)
            .ToList();
        var target = adjacent[Random.Range(0, adjacent.Count)];
        _level.Move(this, target);
    }

    private IEnemyTarget SelectTarget()
    {
        var availableTargets = _level.Entities.OfType<Dog>().ToList();
        if (availableTargets.Count > 0)
        {
            var newTarget = availableTargets[Random.Range(0, availableTargets.Count)];
            return newTarget;
        }

        return null;
    }
}