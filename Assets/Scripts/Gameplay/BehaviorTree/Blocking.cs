using UnityEngine;

public class Blocking : Node
{
    private readonly BlockingAttackable blockingAttackable;

    public Blocking(BlockingAttackable blockingAttackable)
    {
        this.blockingAttackable = blockingAttackable;
    }

    public override bool Process()
    {
        return blockingAttackable.IsBlocking;
    }
}
