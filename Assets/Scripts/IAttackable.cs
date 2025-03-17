using System;

public interface IAttackable : ICellHabitant
{
    event Action OnAttacked;
    void ReceiveAttack(IAttacker attacker);
}
