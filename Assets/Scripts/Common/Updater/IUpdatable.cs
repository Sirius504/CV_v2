public enum UpdateOrder
{
    Injector = 0,
    Initializer = 1,
    Player = 2,
    Metronome = 3,
    System = 4,
    Entity = 5,
    Animation = 6,
    UI = 7
}


public interface IUpdatable
{
    public UpdateOrder UpdateOrder { get; }
    public void UpdateManual();
}