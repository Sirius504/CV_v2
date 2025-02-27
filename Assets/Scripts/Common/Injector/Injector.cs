using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Injector : SystemBase<Injector, IInjectable>, IInitializable
{
    [SerializeField] private Level _level;
    [SerializeField] private Metronome _metronome;
    [SerializeField] private Grid _grid;

    private Dictionary<Type, object> _dependencies;
    private Dictionary<Type, object> Dependencies => _dependencies ??= (_dependencies = new Dictionary<Type, object>()  {
            { typeof(Level), _level },
            { typeof(Metronome), _metronome },
            { typeof(Grid), _grid },
        });

    public InitOrder InitOrder => InitOrder.Injector;

    public override SystemsStartOrder ResolutionOrder => SystemsStartOrder.Injector;

    public void Init()
    {
        _level = _level != null ? _level : FindObjectOfType<Level>();
        _metronome = _metronome != null ? _metronome : FindObjectOfType<Metronome>();

        _dependencies = new Dictionary<Type, object>
        {
            { typeof(Level), _level },
            { typeof(Metronome), _metronome }
        };
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
        var dependencies = injectable.GetType()
            .GetInterfaces()
            .First(type => typeof(IInjectable).IsAssignableFrom(type) && type.IsGenericType)
            .GetGenericArguments()
            .Select(type => Dependencies[type]).ToArray();


        var injectMethod = injectable.GetType().GetMethod("Inject");
        injectMethod.Invoke(injectable, dependencies);
    }
}