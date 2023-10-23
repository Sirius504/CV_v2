using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class ActionInfoManager : SystemBase<ActionInfoManager, IActionTelegraph>, IInjectable<Grid>, IUpdatable
{
    // for each tile we spawn TurnInfo either to the right or down or both, like that
    //
    //  - - -
    //  -   r
    //  - d -
    //
    [SerializeField] private ActionInfoView _horizontalPrefab;
    [SerializeField] private ActionInfoView _verticalPrefab;
    private Grid _grid;

    private readonly HashSet<IActionTelegraph> _entities = new();
    private readonly List<ActionInfoView> _infoViews = new();

    public InitOrder InitOrder => InitOrder.System;

    public UpdateOrder UpdateOrder => UpdateOrder.System;

    public void Inject(Grid grid)
    {
        _grid = grid;
    }

    private void OnDestroy()
    {
        foreach(var entity in _entities)
        {
            entity.OnDestroyEvent -= OnEntityDestroyed;
        }
    }

    protected override void RegisterMany(IEnumerable<IActionTelegraph> entities)
    {
        foreach(var entity in entities)
        {
            entity.OnDestroyEvent += OnEntityDestroyed;
            _entities.Add(entity);
        }
    }

    private void OnEntityDestroyed(IDestroyable entity)
    {
        entity.OnDestroyEvent -= OnEntityDestroyed;
        _entities.Remove((IActionTelegraph)entity);
    }

    public void UpdateManual()
    {
        foreach(var infoView in _infoViews)
        {
            Destroy(infoView.gameObject);
        }
        _infoViews.Clear();

        foreach (var tileActions in _entities
            .Where(entity => entity.ActionInfo != null)
            .Select(entity => entity.ActionInfo.Value)
            .GroupBy(GetTile))
        {
            var tile = tileActions.Key;
            var tileCenterWorld = _grid.GetCellCenterWorld((Vector3Int)tile);

            CreateInfoView(tileActions, tileCenterWorld, ActionDirection.Horizontal);
            CreateInfoView(tileActions, tileCenterWorld, ActionDirection.Vertical);
        }
    }

    private void CreateInfoView(IEnumerable<ActionInfo> actions, Vector3 tileCenter, ActionDirection direction)
    {
        var directionalActions = actions.Where(action => action.Direction == direction);
        if (!directionalActions.Any())
        {
            return;
        }
        var prefab = GetPrefab(direction);
        var offset = GetOffset(direction);
        var infoView = Instantiate(_verticalPrefab, tileCenter + offset, Quaternion.identity, transform);
        _infoViews.Add(infoView);
        infoView.Set(directionalActions.ToArray());
    }

    private Vector3 GetOffset(ActionDirection direction)
    {
        return direction switch
        {
            ActionDirection.Horizontal => Vector3.right * _grid.cellSize.x / 2,
            ActionDirection.Vertical => Vector3.down * _grid.cellSize.y / 2,
            _ => throw new NotImplementedException()
        };
    }

    private ActionInfoView GetPrefab(ActionDirection direction)
    {
        return direction switch
        {
            ActionDirection.Horizontal => _horizontalPrefab,
            ActionDirection.Vertical => _verticalPrefab,
            _ => throw new NotImplementedException()
        };
    }

    private Vector2Int GetTile(ActionInfo actionInfo)
    {
        return actionInfo.Direction switch
        {
            ActionDirection.Horizontal => GetLeftmost(actionInfo.To, actionInfo.From),
            ActionDirection.Vertical => GetTopmost(actionInfo.To, actionInfo.From),
            _ => throw new NotImplementedException()
        };
    }

    private Vector2Int GetLeftmost(Vector2Int one, Vector2Int two)
    {
        return one.x < two.x ? one: two;
    }

    private Vector2Int GetTopmost(Vector2Int one, Vector2Int two)
    {
        return one.y > two.y ? one : two;
    }
}