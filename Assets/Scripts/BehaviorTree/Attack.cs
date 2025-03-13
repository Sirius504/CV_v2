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

    public override bool Process(out ActionInfo actionInfo)
    {
        actionInfo = default;
        if (_plotter.Peek() == null)
        {
            return false;
        }

        var nextPosition = _plotter.Peek().Value;
        var nextTile = _level.GetCell(nextPosition);
        if (nextTile.TryGet<IAttackable>(out var attackable))
        {
            actionInfo = new ActionInfo(nextPosition, _level.GetEntityPosition(_owner), Action.Attack);
            attackable.ReceiveAttack(_owner);
            return true;
        }

        return false;
    }
}