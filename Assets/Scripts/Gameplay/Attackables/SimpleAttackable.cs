using System;
using UnityEngine;
using VContainer;

[RequireComponent(typeof(Health))]
public class SimpleAttackable : CellComponent, IAttackable, IInitializable
{
    [Inject]
    protected Level _level;
    protected Health _health;

    public event Action OnAttacked;

    public override void Init()
    {
        base.Init();
        _health = GetComponent<Health>();
    }

    public virtual void ReceiveAttack(IAttacker attacker)
    {
        _health.TakeDamage(attacker.Damage);
        OnAttacked?.Invoke();
    }
}
