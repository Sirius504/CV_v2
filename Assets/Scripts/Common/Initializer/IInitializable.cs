public enum InitOrder
{
    Injector = 0,
    Initializer = Injector + 1,
    Updater = Initializer + 1,
    System = Updater + 1,
    AI = System + 1,
    Entity = AI + 1,
    UI = Entity + 1,
    Animation = UI + 1
}

public interface IInitializable
{
    public InitOrder InitOrder { get; }
    public void Init();
}