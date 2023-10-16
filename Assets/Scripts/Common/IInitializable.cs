public enum InitOrder
{
    System = 0,
    Entity = 1,
    UI = 2
}

public interface IInitializable
{
    public InitOrder InitOrder { get; }
    public void Init();
}