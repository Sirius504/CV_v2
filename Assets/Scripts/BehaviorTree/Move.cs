public class Move : Node
{
    readonly ICellHabitant owner;
    readonly Level level;
    readonly Plotter plotter;

    public Move(ICellHabitant owner, Level level, Plotter plotter)
    {
        this.owner = owner;
        this.level = level;
        this.plotter = plotter;
    }

    public override bool Process()
    {
        var tile = plotter.Peek();
        if (tile == null)
            return false;
        level.Move(owner, tile.Value);
        return true;
    }
}

