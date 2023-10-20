using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Updater : SystemBase<Updater, IUpdatable>, IInitializable
{
    private HashSet<IUpdatable> _updatables;

    public InitOrder InitOrder => InitOrder.Updater;

    public override SystemsStartOrder StartOrder => SystemsStartOrder.Updater;

    public void Init()
    {
        _updatables = new HashSet<IUpdatable>();
    }

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
        foreach(var updatable in _updatables.OrderBy(u => u.UpdateOrder))
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

    private void OnEntityDestroy(MonoBehaviour updatable)
    {
        _updatables.Remove((IUpdatable)updatable);
    }
}