public interface IInjectable
{
}

public interface IInjectable<T> : IInjectable
{
    public void Inject(T dependency);
}

public interface IInjectable<T1, T2> : IInjectable
{
    public void Inject(T1 dependency1, T2 dependency2);
}

public interface IInjectable<T1, T2, T3> : IInjectable
{
    public void Inject(T1 dependency1, T2 dependency2, T3 dependency3);
}