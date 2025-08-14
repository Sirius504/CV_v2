using UnityEngine;
using VContainer;

public abstract class BaseBehaviourTreeSO : ScriptableObject
{
    public abstract Node BuildTree(IObjectResolver container);
}
