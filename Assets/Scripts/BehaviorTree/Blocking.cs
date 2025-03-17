using UnityEngine;

namespace Assets.Scripts.BehaviorTree
{
    public class Blocking : Node
    {
        private readonly BlockingAttackable blockingAttackable;

        public Blocking(string name, MonoBehaviour owner) : base(name)
        {
            blockingAttackable = owner.GetComponent<BlockingAttackable>();
        }

        public override bool Process()
        {
            return blockingAttackable.IsBlocking;
        }
    }
}
