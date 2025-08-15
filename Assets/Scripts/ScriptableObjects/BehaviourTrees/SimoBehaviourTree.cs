using UnityEngine;
using VContainer;

[CreateAssetMenu(fileName = "SimoBehaviourTree", menuName = "BehaviourTrees/Simo")]
public class SimoBehaviourTree : BaseBehaviourTreeSO
{
    public override Node BuildTree(IObjectResolver container)
    {
        var behaviourTree = container.Resolve<Sequence>();
        behaviourTree.AddChild(container.Resolve<RangedAttack>());
        return behaviourTree;
    }
}
