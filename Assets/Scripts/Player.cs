using System;
using UnityEngine;

public class Player : MonoBehaviour, ICellHabitant
{
    [SerializeField] private Level _level;
    [SerializeField] private int _damage;

    public event Action<MonoBehaviour> OnDestroyEvent;

    public void UpdateManual()
    {
        var input = new Vector2Int(0, 0);
        input = Input.GetKeyDown(KeyCode.W) ? Vector2Int.up : input;
        input = Input.GetKeyDown(KeyCode.S) ? Vector2Int.down : input;
        input = Input.GetKeyDown(KeyCode.A) ? Vector2Int.left : input;
        input = Input.GetKeyDown(KeyCode.D) ? Vector2Int.right : input;

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
            transform.position = _level.CellToWorld(this);
        }
        else if (targetCell.TryGet(typeof(Enemy), out Enemy enemy))
        {
            var health = enemy.GetComponent<Health>();
            health.TakeDamage(_damage);
        }
    }

    private void OnDestroy()
    {
        OnDestroyEvent?.Invoke(this);
    }
}
