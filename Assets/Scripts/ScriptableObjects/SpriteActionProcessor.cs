using ActionBehaviour;
using System;
using UnityEngine;

public abstract class SpriteActionProcessor : ScriptableObject
{
    public abstract float Duration { get; }
    public abstract void ProcessAction(SpriteRenderer sprite, IEvent @event);

    public virtual void OnTick(uint tick)
    {
        // for override
    }
}
