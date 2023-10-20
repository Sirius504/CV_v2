using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class Game : SingletonBehaviour<Game>
{
    private HashSet<ISystem> _systems;
    private List<MonoBehaviour> _creationQueue;
    private List<MonoBehaviour> CreationQueue => _creationQueue ??= new List<MonoBehaviour>();
    private HashSet<ISystem> Systems => _systems ??= new HashSet<ISystem>();

    private void Start()
    {
        ResolveAll();
    }

    private void Update()
    {
        ResolveAll();
    }

    public void NotifyCreation(MonoBehaviour mb)
    {
        CreationQueue.Add(mb);
    }

    private void ResolveAll()
    {
        if (CreationQueue.Count == 0)
        {
            return;
        }

        foreach (var system in CreationQueue.OfType<ISystem>())
        {
            Systems.Add(system);
        }

        foreach (var system in Systems.OrderBy(sys => sys.StartOrder))
        {
            system.RegisterMany(CreationQueue);
        }

        CreationQueue.Clear();
    }
}