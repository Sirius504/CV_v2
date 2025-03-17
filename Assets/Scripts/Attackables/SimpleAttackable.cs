using System;
using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(ICellHabitant))]
public class SimpleAttackable : MonoEntity, IAttackable, IInjectable<Level>, IChildCellHabitant
{
    protected Level _level;
    protected Health _health;
    protected ICellHabitant _parent;

    public InitOrder InitOrder => InitOrder.Entity;

    public ICellHabitant Parent => _parent;

    public event Action OnAttacked;


    public void Inject(Level level)
    {
        _level = level;
        _health = GetComponent<Health>();
        _parent = GetComponent<ICellHabitant>();
    }

    public virtual void ReceiveAttack(IAttacker attacker)
    {
        _health.TakeDamage(attacker.Damage);
        OnAttacked?.Invoke();
    }
}
