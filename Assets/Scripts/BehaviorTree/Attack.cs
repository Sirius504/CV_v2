using ActionBehaviour;

public class Attack : Node
{
    private readonly Level _level;
    private readonly Plotter _plotter;
    private readonly IAttacker _owner;

    public Attack(string name, Level level, Plotter plotter, IAttacker owner) : base(name)
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
        if (nextTile.TryGet<IAttackable>(out var attackable))
        {
            attackable.ReceiveAttack(_owner);
            var @event = new AttackEvent(_owner, attackable);
            EventBus<AttackEvent>.Raise(@event);
            return true;
        }

        return false;
    }
}