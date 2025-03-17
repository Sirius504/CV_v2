using UnityEngine;

public struct AttackEvent : IEvent
{
    public IAttacker attacker;
    public IAttackable attackable;

    public AttackEvent(IAttacker attacker, IAttackable attackable)
    {
        this.attacker = attacker;
        this.attackable = attackable;
    }
}

public struct MovementEvent : IEvent
{
    public ICellHabitant entity;
    public Vector2Int from;
    public Vector2Int to;

    public MovementEvent(ICellHabitant entity, Vector2Int from, Vector2Int to) : this()
    {
        this.to = to;
        this.from = from;
        this.entity = entity;
    }
}


public struct BlockingEvent : IEvent
{
    public ICellHabitant entity;
    public Vector2Int attackSource;

    public BlockingEvent(ICellHabitant entity, Vector2Int attackSource)
    {
        this.entity = entity;
        this.attackSource = attackSource;
    }
}
