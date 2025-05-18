using ActionBehaviour;
using Assets.Scripts.BehaviorTree;
using UnityEngine;
using VContainer;

public class Enemy : MonoEntity, ICellHabitant,
    ITickable,
    IActionTelegraph,
    IUpdatable,
    IAttacker,
    IInitializable
{
    [SerializeField] private int _damage;

    [Inject]
    private Level _level;

    private Plotter _plotter;
    private Node _behaviourTree;

    public ActionInfo? ActionInfo { get; private set; } = null;

    public UpdateOrder UpdateOrder => UpdateOrder.Entity;

    public int Damage => _damage;

    public InitOrder InitOrder => InitOrder.Entity;


    public void Init()
    {
        _plotter = GetComponent<Plotter>();
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
            ? Action.Attack
            : Action.Movement;
        ActionInfo = new ActionInfo(targetPosition, _level.GetEntityPosition(this), actionType);
    }

    public void OnTick(uint tick)
    {
        _behaviourTree.Process();
        _behaviourTree.Reset();
    }
}