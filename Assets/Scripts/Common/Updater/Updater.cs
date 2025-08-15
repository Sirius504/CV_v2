using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Updater : SystemBase<Updater, IUpdatable>
{
    private readonly HashSet<IUpdatable> _updatables = new();

    public InitOrder InitOrder => InitOrder.Updater;

    public override SystemsStartOrder ResolutionOrder => SystemsStartOrder.Updater;


    protected override void RegisterMany(IEnumerable<IUpdatable> updatables)
    {
        foreach(var updatable in updatables)
        {
            Register(updatable);
        }
    }

    private void Register(IUpdatable updatable)
    {
        if (updatable is IDestroyable destroyable)
        {
            destroyable.OnDestroyEvent += OnEntityDestroy;
        }
        _updatables.Add(updatable);
    }

    public void Update()
    {
        var ordered = _updatables.OrderBy(u => u.UpdateOrder).ToList();
        foreach (var updatable in ordered)
        {
            updatable.UpdateManual();
        }
    }

    private void OnDestroy()
    {
        foreach(var destroyable in _updatables.OfType<IDestroyable>())
        {
            destroyable.OnDestroyEvent -= OnEntityDestroy;
        }
    }

    private void OnEntityDestroy(IDestroyable updatable)
    {
        updatable.OnDestroyEvent -= OnEntityDestroy;
        _updatables.Remove((IUpdatable)updatable);
    }
}