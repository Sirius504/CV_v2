public interface ICellEntity : IDestroyable
{
    public bool Has<T>() where T : ICellComponent;
    public bool TryGet<T>(out T value) where T : ICellComponent;
}

public interface ICellComponent
{
    public ICellEntity Entity { get; }
}