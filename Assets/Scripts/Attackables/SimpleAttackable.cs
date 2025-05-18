using System;
using UnityEngine;
using VContainer;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(ICellHabitant))]
public class SimpleAttackable : MonoEntity, IAttackable, IInitializable, IChildCellHabitant
{
    [Inject]
    protected Level _level;
    protected Health _health;
    protected ICellHabitant _parent;

    public InitOrder InitOrder => InitOrder.Entity;

    public ICellHabitant Parent => _parent;

    public event Action OnAttacked;


    public void Init()
    {
        _health = GetComponent<Health>();
        _parent = GetComponent<ICellHabitant>();
    }

    public virtual void ReceiveAttack(IAttacker attacker)
    {
        _health.TakeDamage(attacker.Damage);
        OnAttacked?.Invoke();
    }
}
