public enum UpdateOrder
{
    Injector = 0,
    Initializer = Injector + 1,
    Player = Initializer + 1,
    Metronome = Player + 1,
    System = Metronome + 1,
    AI = System + 1,
    Entity = AI + 1,
    Animation = Entity + 1,
    UI = Animation + 1
}


public interface IUpdatable
{
    public UpdateOrder UpdateOrder { get; }
    public void UpdateManual();
}