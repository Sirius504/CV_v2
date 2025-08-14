using UnityEngine;
using VContainer;

[CreateAssetMenu(fileName = "GalciaBehaviourTree", menuName = "BehaviourTrees/Galcia")]
public class GalciaBehaviourTree : BaseBehaviourTreeSO
{
    public override Node BuildTree(IObjectResolver container)
    {
        var behaviourTree = container.Resolve<Selector>();       
        behaviourTree.AddChild(container.Resolve<Attack>());
        behaviourTree.AddChild(container.Resolve<Move>());

        return behaviourTree;
    }
}
