using ActionBehaviour;
using System;
using UnityEngine;

public class Enemy : MonoEntity, ICellHabitant,
    ITickable,
    IInjectable<Level>,
    IActionTelegraph,
    IUpdatable,
    IActioning,
    IAttacker,
    IAttackable,
    IInitializable
{
    [SerializeField] private int _damage;
    [SerializeField] private SpriteProcessorsList _processorList;

    private Level _level;

    private Plotter _plotter;
    private Health _health;
    private Node _behaviourTree;

    public ActionInfo? ActionInfo { get; private set; } = null;

    public UpdateOrder UpdateOrder => UpdateOrder.Entity;

    public int Damage => _damage;

    public InitOrder InitOrder => InitOrder.Entity;

    public event Action<ActionInfo> OnAction;

    public void Inject(Level level)
    {
        _level = level;
        _health = GetComponent<Health>();
        _plotter = GetComponent<Plotter>();
    }

    public void Init()
    {
        _behaviourTree = new Selector("selector");
        _behaviourTree.AddChild(new Attack("attack", _level, _plotter, this));
        _behaviourTree.AddChild(new Move("move", this, _level, _plotter));
    }

    public void UpdateManual()
    {
        var targetPositionNullable = _plotter.Peek();
        if (!targetPositionNullable.HasValue)
        {
            // valid path to target doesn't exist
            ActionInfo = null;
            return;
        }

        var targetPosition = targetPositionNullable.Value;
        var actionType = _level.GetCell(targetPosition).Has<IEnemyTarget>()
            ? ActionBehaviour.Action.Attack
            : ActionBehaviour.Action.Movement;
        ActionInfo = new ActionInfo(targetPosition, _level.GetEntityPosition(this), actionType);
    }

    public void OnTick(uint tick)
    {
        if (_behaviourTree.Process(out var actionInfo))
        {
            OnAction?.Invoke(actionInfo);
        }
        _behaviourTree.Reset();
    }

    public void ReceiveAttack(IAttacker attacker)
    {
        _health.TakeDamage(attacker.Damage);
    }
}