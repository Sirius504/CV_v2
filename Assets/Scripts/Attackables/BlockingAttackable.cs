using UnityEngine;

public class BlockingAttackable : SimpleAttackable, ITickable
{
    private bool isBlocking = false;
    public bool IsBlocking => isBlocking;

    public OrderInTick OrderInTick => OrderInTick.TickEnd; 

    public override void ReceiveAttack(IAttacker attacker)
    {
        if (isBlocking)
        {
            return;
        }

        base.ReceiveAttack(attacker);
        var @event = new BlockingEvent(Parent, _level.GetEntityPosition(attacker));
        EventBus<BlockingEvent>.Raise(@event);
        isBlocking = true;
    }

    public void OnTick(uint _)
    {
        isBlocking = false;
    }
}
