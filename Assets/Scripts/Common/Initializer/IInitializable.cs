public enum InitOrder
{
    Injector = 0,
    Initializer = 1,
    Updater = 2,
    System = 3,
    Entity = 4,
    UI = 5,
    Animation = 6
}

public interface IInitializable
{
    public InitOrder InitOrder { get; }
    public void Init();
}