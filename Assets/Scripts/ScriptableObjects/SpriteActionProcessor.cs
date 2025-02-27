using ActionBehaviour;
using UnityEngine;

public abstract class SpriteActionProcessor : ScriptableObject
{
    public abstract float Duration { get; }
    public abstract void ProcessAction(SpriteRenderer sprite, ActionInfo actionInfo);
}
