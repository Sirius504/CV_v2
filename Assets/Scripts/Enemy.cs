using System;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour, ICellHabitant, ITickable
{
    [SerializeField] private Level _level;

    public event Action<MonoBehaviour> OnDestroyEvent;

    public void OnTick(uint tick)
    {
        var adjacent = _level.GetAdjacentCells(this)
            .Where(cell => cell.IsEmpty())
            .Select(cell => cell.Position)
            .ToList();
        var target = adjacent[UnityEngine.Random.Range(0, adjacent.Count)];
        _level.Move(this, target);
        transform.position = _level.CellToWorld(this);
    }

    private void OnDestroy()
    {
        OnDestroyEvent?.Invoke(this);
    }
}