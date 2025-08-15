using System;

public interface IAttackable : ICellComponent
{
    event Action OnAttacked;
    void ReceiveAttack(IAttacker attacker);
}
