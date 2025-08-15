public class RangedAttack : Node
{
    private readonly Level _level;
    private readonly Plotter _plotter;
    private readonly RangedAttacker _owner;

    public RangedAttack(Level level, Plotter plotter, RangedAttacker owner)
    {
        _level = level;
        _plotter = plotter;
        _owner = owner;
    }

    public override bool Process()
    {
        if (_plotter.Target == null)
        {
            return false;
        }

        var target = _plotter.Target;
        if (!_owner.HasLockedIn)
        {
            _owner.Aim();
            return false;
        }

        if (target.TryGet<IAttackable>(out var attackable))
        {
            _owner.OnAttack(attackable);
            attackable.ReceiveAttack(_owner);

            var @event = new AttackEvent(_owner, attackable);
            EventBus<AttackEvent>.Raise(@event);
            return true;
        }

        return false;
    }
}
