using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class Game : SingletonBehaviour<Game>
{
    private readonly HashSet<ISystem> _systems = new();
    private readonly HashSet<ISystem> _passedSystems = new();

    private readonly List<(int generation, MonoBehaviour entity)> _creationQueue = new();
    private readonly List<MonoEntity> _entities = new();
    private int _generation = 0;
    private bool _firstResolvePassed = false;

    private void Start()
    {
        ResolveAll();
        _firstResolvePassed = true;
    }

    private void Update()
    {
        ResolveAll();
    }

    public void NotifyCreation(MonoBehaviour entity)
    {
        _creationQueue.Add((_generation, entity));
        if (_firstResolvePassed) ResolveAll();
    }

    private void ResolveAll()
    {
        if (_creationQueue.Count == 0)
        {
            return;
        }

        var newSystems = _creationQueue.Select(tuple => tuple.entity).OfType<ISystem>();
        if (newSystems.Any() && _firstResolvePassed)
        {
            Debug.LogWarning($"Systems were added after initial resolution; untested behaviour");
            var allSystemsString = string.Join(", ", newSystems.Select(system => ((MonoBehaviour)system).name));
            Debug.LogWarning($"Systems list: {allSystemsString}");
        }

        foreach (var system in newSystems)
        {
            _systems.Add(system);
        }
                
        foreach (var system in _systems.OrderBy(sys => sys.ResolutionOrder))
        {
            // during registration in some systems, new entities might be created. they will be assigned a new generation
            _generation++;           
            system.Register(_creationQueue.Select(tuple => tuple.entity));
            _passedSystems.Add(system);

            foreach (var passedSystem in _passedSystems.OrderBy(sys => sys.ResolutionOrder))
            {
                passedSystem.Register(_creationQueue
                    .Where(tuple => tuple.generation == _generation)
                    .Select(tuple => tuple.entity));
            }
        }

        _creationQueue.Clear();
        _passedSystems.Clear();
        _generation = 0;
    }
}