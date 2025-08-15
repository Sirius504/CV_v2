public interface IAttacker : ICellComponent
{
    int Damage { get; }

    public void OnAttack(IAttackable target)
    {

    }
}
