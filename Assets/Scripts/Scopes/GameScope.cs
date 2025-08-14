using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameScope : LifetimeScope
{
    [SerializeField] private LevelGrid _levelGrid;
    [SerializeField] private Level _level;
    [SerializeField] private Grid _grid;
    [SerializeField] private Metronome _metronome;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponent(_levelGrid);
        builder.RegisterComponent(_grid);
        builder.RegisterComponent(_level);
        builder.RegisterComponent(_metronome);
        builder.Register<Astar>(Lifetime.Singleton);
    }
}
