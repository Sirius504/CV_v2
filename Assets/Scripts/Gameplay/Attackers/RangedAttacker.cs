using UnityEngine;
using VContainer;

public class RangedAttacker : CellComponent, IAttacker, ILaserDrawer
{
    [SerializeField] private int _damage = 1;
    [SerializeField] private int _ticksToAim = 3;

    [Inject]
    private Plotter _plotter;

    private ICellEntity _currentTarget;



    private int _aimProgress = 0;

    public int Damage => _damage;

    public bool HasLockedIn => _aimProgress == _ticksToAim - 1;

    public ICellEntity Source => Entity;

    public ICellEntity Target => _plotter.Target;

    public float Progress => _aimProgress / (float)(_ticksToAim - 1);

    public void Aim()
    {
        if (_currentTarget != _plotter.Target)
        {
            _currentTarget = _plotter.Target;
            if (_aimProgress > 0)
            {
                Interrupt();
            }
        }

        _aimProgress = Mathf.Min(++_aimProgress, _ticksToAim);
    }

    public void Interrupt()
    {
        _aimProgress = 0;
    }

    public void OnAttack(IAttackable attackable)
    {
        Interrupt();
    }
}
