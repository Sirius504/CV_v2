using ActionBehaviour;
using Assets.Scripts.BehaviorTree;
using System;
using UnityEngine;

public class Enemy : MonoEntity, ICellHabitant,
    ITickable,
    IInjectable<Level>,
    IActionTelegraph,
    IUpdatable,
    IAttacker,
    IInitializable
{
    [SerializeField] private int _damage;

    private Level _level;

    private Plotter _plotter;
    private Node _behaviourTree;

    public ActionInfo? ActionInfo { get; private set; } = null;

    public UpdateOrder UpdateOrder => UpdateOrder.Entity;

    public int Damage => _damage;

    public InitOrder InitOrder => InitOrder.Entity;


    public void Inject(Level level)
    {
        _level = level;
        _plotter = GetComponent<Plotter>();
    }

    public void Init()
    {
        _behaviourTree = new Selector("Galcia");
        if (GetComponent<BlockingAttackable>() != null)
            _behaviourTree.AddChild(new Blocking("blocking", this));
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
        _behaviourTree.Process();
        _behaviourTree.Reset();
    }
}