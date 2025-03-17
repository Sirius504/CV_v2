public interface ICellHabitant : IDestroyable
{
}

public interface IChildCellHabitant : ICellHabitant
{
    ICellHabitant Parent { get; }
}