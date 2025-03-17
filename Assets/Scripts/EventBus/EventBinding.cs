using System;

public interface IEventBinding<T> {
    public Action<T> OnEvent { get; set; }
    public Func<T, bool> Predicate { get; }
    public Action OnEventNoArgs { get; set; }
}

public class EventBinding<T> : IEventBinding<T> where T : IEvent {
    private Action<T> onEvent = _ => { };
    private Func<T, bool> predicate;
    private Action onEventNoArgs = () => { };

    Action<T> IEventBinding<T>.OnEvent {
        get => onEvent;
        set => onEvent = value;
    }

    Action IEventBinding<T>.OnEventNoArgs {
        get => onEventNoArgs;
        set => onEventNoArgs = value;
    }

    Func<T, bool> IEventBinding<T>.Predicate => predicate;

    public EventBinding(Action<T> onEvent, Func<T, bool> predicate = null)
    {
        this.onEvent = onEvent;
        this.predicate = predicate ?? ((_) => true);
    }
    public EventBinding(Action onEventNoArgs, Func<T, bool> predicate = null)
    {
        this.onEventNoArgs = onEventNoArgs;
        this.predicate = predicate ?? ((_) => true);
    }
    
    public void Add(Action onEvent) => onEventNoArgs += onEvent;
    public void Remove(Action onEvent) => onEventNoArgs -= onEvent;
    
    public void Add(Action<T> onEvent) => this.onEvent += onEvent;
    public void Remove(Action<T> onEvent) => this.onEvent -= onEvent;
}