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
    public ICellEntity entity;
    public Vector2Int from;
    public Vector2Int to;

    public MovementEvent(ICellEntity entity, Vector2Int from, Vector2Int to) : this()
    {
        this.to = to;
        this.from = from;
        this.entity = entity;
    }
}


public struct BlockingEvent : IEvent
{
    public ICellEntity entity;
    public Vector2Int attackSource;

    public BlockingEvent(ICellEntity entity, Vector2Int attackSource)
    {
        this.entity = entity;
        this.attackSource = attackSource;
    }
}
