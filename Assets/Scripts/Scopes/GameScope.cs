using UnityEngine;
using VContainer;
using VContainer.Unity;
using Yarn.Unity;

public class GameScope : LifetimeScope
{
    [SerializeField] private LevelGrid _levelGrid;
    [SerializeField] private Level _level;
    [SerializeField] private Grid _grid;
    [SerializeField] private Metronome _metronome;
    [SerializeField] private DialogueRunner _dialogueRunner;
    [SerializeField] private LevelLoader _levelLoader;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponent(_levelGrid);
        builder.RegisterComponent(_grid);
        builder.RegisterComponent(_level);
        builder.RegisterComponent(_metronome);
        builder.RegisterComponent(_dialogueRunner);
        builder.RegisterComponent(_levelLoader);
        builder.Register<Astar>(Lifetime.Singleton);
    }
}
