using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class SystemBase<TSystem, TEntity> : SingletonBehaviour<TSystem>, ISystem
    where TSystem : SingletonBehaviour<TSystem>, ISystem
    where TEntity : class
{
    public virtual SystemsStartOrder StartOrder => SystemsStartOrder.Default;

    public void Register(IEnumerable<MonoBehaviour> monoBehaviours)
    {
        var validEntities = monoBehaviours.OfType<TEntity>();
        if (!validEntities.Any()) return;
        RegisterMany(validEntities);
    }

    protected override void Awake()
    {
        Game.Instance.NotifyCreation(this);
    }

    protected abstract void RegisterMany(IEnumerable<TEntity> entities);
}