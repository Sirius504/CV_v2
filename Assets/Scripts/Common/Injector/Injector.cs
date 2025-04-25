using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Injector : SystemBase<Injector, IInjectable>, IInitializable
{
    [SerializeField] private Level _level;
    [SerializeField] private Metronome _metronome;
    [SerializeField] private Grid _grid;
    [SerializeField] private LevelGrid _levelGrid;

    private Dictionary<Type, object> _dependencies;
    private Dictionary<Type, object> Dependencies => _dependencies ??= (_dependencies = new Dictionary<Type, object>()  {
            { typeof(Level), _level },
            { typeof(Metronome), _metronome },
            { typeof(Grid), _grid },
            { typeof(LevelGrid), _levelGrid  }
        });

    public InitOrder InitOrder => InitOrder.Injector;

    public override SystemsStartOrder ResolutionOrder => SystemsStartOrder.Injector;

    public void Init()
    {
        _level = _level != null ? _level : FindObjectOfType<Level>();
        _metronome = _metronome != null ? _metronome : FindObjectOfType<Metronome>();
    }


    protected override void RegisterMany(IEnumerable<IInjectable> injectables)
    {
        if (!injectables.Any())
        {
            return;
        }

        foreach (var injectable in injectables)
        {
            Resolve(injectable);
        }
    }

    private void Resolve(IInjectable injectable)
    {
        var injectableInterfaces = injectable.GetType()
            .GetInterfaces()
            .Where(type => typeof(IInjectable).IsAssignableFrom(type));

        var genericInjectable = injectableInterfaces.FirstOrDefault(type => typeof(IInjectable).IsAssignableFrom(type) && type.IsGenericType);

        var injectMethod = injectable.GetType().GetMethod("Inject");
        object[] dependencies = null;

        if (genericInjectable != null)
        {
            dependencies = genericInjectable
                .GetGenericArguments()
                .Select(type => Dependencies[type]).ToArray();
        }

        injectMethod.Invoke(injectable, dependencies);
    }
}