using UnityEngine;
using VContainer;

public class Laser : CellComponent, ITickable, IAttacker, ILaserDrawer
{
    [Inject] private Level _level;

    [SerializeField] private int _phase = 0;
    [SerializeField] private int _period;
    [SerializeField] private int _damage;

    [SerializeField] private Vector2Int _direction;
    [SerializeField] private int _length;

    public int Damage => _damage;

    public float Progress { get; private set; } = 0f;

    public bool Enabled => true;

    public Vector2Int Source => _level.GetEntityPosition(Entity);

    public Vector2Int Target => Source + _direction * _length;

    public void OnTick(uint tick)
    {
        var progressInTurns = (tick + _phase) % _period;
        Progress = progressInTurns / (float)_period;

        if (progressInTurns != 0)
        {
            return;
        }

        // foreach cell on laser path
        var startCell = _level.GetEntityPosition(Entity);
        for (int i = 0; i < _length; i++)
        {
            var currentCell = _level.GetCell(startCell + i * _direction);
            // find all attackables
            if (!currentCell.TryGetAll<IAttackable>(out var attackables))
            {
                continue;
            }

            // and hit them all
            foreach (var attackable in attackables.Values)
            {
                attackable.ReceiveAttack(this);
            }
        }
    }
}
