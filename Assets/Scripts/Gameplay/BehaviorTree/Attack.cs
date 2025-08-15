using ActionBehaviour;

public class Attack : Node
{
    private readonly Level _level;
    private readonly Plotter _plotter;
    private readonly IAttacker _owner;

    public Attack(Level level, Plotter plotter, IAttacker owner)
    {
        _level = level;
        _plotter = plotter;
        _owner = owner;
    }

    public override bool Process()
    {
        if (_plotter.Peek() == null)
        {
            return false;
        }

        var nextPosition = _plotter.Peek().Value;
        var nextTile = _level.GetCell(nextPosition);
        if (nextTile.TryGetFirst<IAttackable>(out var attackable))
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