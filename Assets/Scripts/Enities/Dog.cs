public class Dog : MonoEntity, ICellHabitant, IEnemyTarget, IAttackable
{
    private Health _health;
    private void Start()
    {
        _health = GetComponent<Health>();
    }

    public void ReceiveAttack(IAttacker attacker)
    {
        _health.TakeDamage(attacker.Damage);
    }
}
