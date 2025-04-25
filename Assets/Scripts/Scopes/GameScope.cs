using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameScope : LifetimeScope
{
    [SerializeField] private LevelGrid _levelGrid;

    protected override void Configure(IContainerBuilder builder)
    {
        base.Configure(builder);
        builder.RegisterComponent(_levelGrid);
        builder.Register<Astar>(Lifetime.Singleton);
    }
}
