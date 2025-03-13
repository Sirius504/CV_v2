public interface IAttackable : ICellHabitant
{
    void ReceiveAttack(IAttacker attacker);
}
