using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class SystemBase<TSystem, TEntity> : MonoBehaviour, ISystem
    where TSystem : ISystem
    where TEntity : class
{
    public virtual SystemsStartOrder ResolutionOrder => SystemsStartOrder.Default;

    public void Register(IEnumerable<MonoBehaviour> monoBehaviours)
    {
        var validEntities = monoBehaviours.OfType<TEntity>();
        if (!validEntities.Any()) return;
        RegisterMany(validEntities);
    }

    protected virtual void Awake()
    {
        Game.Instance.NotifyCreation(this);
    }

    protected abstract void RegisterMany(IEnumerable<TEntity> entities);
}