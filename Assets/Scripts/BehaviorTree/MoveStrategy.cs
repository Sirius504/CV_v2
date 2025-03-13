
using ActionBehaviour;

public class Move : Node
{
    readonly ICellHabitant owner;
    readonly Level level;
    readonly Plotter plotter;

    public Move(string name, ICellHabitant owner, Level level, Plotter plotter) : base(name)
    {
        this.owner = owner;
        this.level = level;
        this.plotter = plotter;
    }

    public override bool Process(out ActionInfo actionInfo)
    {
        var tile = plotter.Peek();
        actionInfo = default;
        if (tile == null)
            return false;
        actionInfo = new ActionInfo(tile.Value, level.GetEntityPosition(owner), Action.Movement);
        level.Move(owner, tile.Value);
        return true;
    }
}

