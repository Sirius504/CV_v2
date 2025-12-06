using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;

public class LasersSystem : SystemBase<LasersSystem, Laser>, ITickable
{
    [Inject] private Level _level;

    private HashSet<Laser> _lasers;

    public event Action<IDestroyable> OnDestroyEvent;

    private void Register(Laser laser)
    {
        _lasers.Add(laser);
        _lasers.OrderBy(laser => _level.GetEntityPosition(laser.Entity).y);
    }


    protected override void RegisterMany(IEnumerable<Laser> entities)
    {
        foreach(var entity in entities)
        {
            Register(entity);
        }
    }

    public void OnTick(uint tick)
    {
        foreach(var laser in _lasers)
        {

        }    
    }
}
