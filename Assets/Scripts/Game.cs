using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class Game : SingletonBehaviour<Game>
{
    private readonly HashSet<ISystem> _systems = new();
    private readonly HashSet<ISystem> _passedSystems = new();

    private readonly List<(int generation, MonoBehaviour entity)> _creationQueue = new();
    private int _generation = 0;

    private void Start()
    {
        ResolveAll();
    }

    private void Update()
    {
        ResolveAll();
    }

    public void NotifyCreation(MonoBehaviour entity)
    {
        _creationQueue.Add((_generation, entity));
    }

    private void ResolveAll()
    {
        if (_creationQueue.Count == 0)
        {
            return;
        }

        foreach (var system in _creationQueue.Select(tuple => tuple.entity).OfType<ISystem>())
        {
            _systems.Add(system);
        }
                
        foreach (var system in _systems.OrderBy(sys => sys.StartOrder))
        {
            // during registration in some systems, new entities might be created. they will be assigned a new generation
            _generation++;
            system.Register(_creationQueue.Select(tuple => tuple.entity));
            _passedSystems.Add(system);

            foreach (var passedSystem in _passedSystems.OrderBy(sys => sys.StartOrder))
            {
                passedSystem.Register(_creationQueue.Where(tuple => tuple.generation == _generation).Select(tuple => tuple.entity));
            }
        }

        _creationQueue.Clear();
        _passedSystems.Clear();
        _generation = 0;
    }
}