using ActionBehaviour;
using UnityEngine;
using VContainer;

public class Enemy : CellComponent,
    ITickable,
    IActionTelegraph,
    IUpdatable,
    IInitializable
{
    [SerializeField] private BaseBehaviourTreeSO _behaviourTreeAsset;

    [Inject]
    private Level _level;

    [Inject]
    private IObjectResolver _container;

    [Inject]
    private Plotter _plotter;
    private Node _behaviourTree;

    public ActionInfo? ActionInfo { get; private set; } = null;

    public UpdateOrder UpdateOrder => UpdateOrder.Entity;

    public override InitOrder InitOrder => InitOrder.Entity;


    public override void Init()
    {
        base.Init();
        _behaviourTree = _behaviourTreeAsset.BuildTree(_container);
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
        var actionType = _level.GetCell(targetPosition).Has<IEnemyTarget>(out _)
            ? Action.Attack
            : Action.Movement;
        ActionInfo = new ActionInfo(targetPosition, _level.GetEntityPosition(Entity), actionType);
    }

    public void OnTick(uint tick)
    {
        _behaviourTree.Process();
        _behaviourTree.Reset();
    }
}