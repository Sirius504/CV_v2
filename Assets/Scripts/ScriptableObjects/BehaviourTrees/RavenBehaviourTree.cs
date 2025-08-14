using UnityEngine;
using VContainer;

[CreateAssetMenu(fileName = "RavenBehaviourTree", menuName = "BehaviourTrees/Raven")]
public class RavenBehaviourTree : BaseBehaviourTreeSO
{
    public override Node BuildTree(IObjectResolver container)
    {
        var behaviourTree = container.Resolve<Selector>();
        behaviourTree.AddChild(container.Resolve<Blocking>());
        behaviourTree.AddChild(container.Resolve<Attack>());
        behaviourTree.AddChild(container.Resolve<Move>());

        return behaviourTree;
    }
}
