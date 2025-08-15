using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;

public class Level : SystemBase<Level, ICellEntity>, IInitializable
{
    [SerializeField] private LevelGrid _grid;

    [Inject]
    private Astar _astar;
    public IReadOnlyCollection<ICellEntity> Entities => _entitiesPositionsMap.Keys;
    public IEnumerable<ICellInfo> Cells => _cells.Cast<ICellInfo>();

    private CellInfo[,] _cells;
    private Dictionary<ICellEntity, Vector2Int> _entitiesPositionsMap = new();

    public InitOrder InitOrder => InitOrder.System;

    public void Init()
    {
        _cells = new CellInfo[_grid.Size.x, _grid.Size.y];
        for (var i = 0; i < _grid.Size.x; i++)
            for (var j = 0; j < _grid.Size.y; j++)
            {
                _cells[i, j] = new CellInfo(new Vector2Int(i, j));
            }

        _entitiesPositionsMap = new Dictionary<ICellEntity, Vector2Int>(64);
    }

    public double Distance(Vector2Int from, Vector2Int to) => _astar.ManhattanDistance(from, to);

    #region Cells related methods
    public ICellInfo GetCell(Vector2Int index)
    {
        return GetCellInternal(index);
    }

    private CellInfo GetCellInternal(Vector2Int index)
    {
        return _cells[index.x, index.y];
    }

    public ICellInfo GetCell(int x, int y)
    {
        return _cells[x, y];
    }

    #endregion

    #region Entities related methods

    private void OnDestroy()
    {
        try
        {
            foreach (var entity in _entitiesPositionsMap.Keys)
            {
                entity.OnDestroyEvent -= OnEntityDestroy;
            }
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public Vector2Int GetEntityCellPosition(ICellEntity entity)
    {
        return _entitiesPositionsMap[entity];
    }

    public Vector3 GetEntitiyWorldPosition(ICellEntity entity)
    {
        return _grid.CellToWorld(_entitiesPositionsMap[entity]);
    }

    protected override void RegisterMany(IEnumerable<ICellEntity> habitants)
    {
        foreach(var habitant in habitants)
        {
            Register(habitant);
        }
    }

    private void Register(ICellEntity entity)
    {
        var mb = (MonoBehaviour)entity;
        var cellPosition = _grid.WorldToCell(mb.transform.position);
        cellPosition.Clamp(Vector2Int.zero, _grid.Size - Vector2Int.one);
        Add(entity, cellPosition);
        mb.transform.position = _grid.CellToWorld(cellPosition);
    }

    private void Add(ICellEntity entity, Vector2Int position)
    {
        if (_entitiesPositionsMap.TryAdd(entity, position))
        {
            var cell = GetCellInternal(position);
            cell.Put(entity);
            entity.OnDestroyEvent += OnEntityDestroy;
        }
        else
        {
            Debug.LogError("Can't add entity; entity was already added.");
        }
    }

    public void Remove(ICellEntity entity)
    {
        if (_entitiesPositionsMap.TryGetValue(entity, out var position))
        {
            var cell = GetCellInternal(position);
            cell?.Remove(entity);
            _entitiesPositionsMap.Remove(entity);
            entity.OnDestroyEvent -= OnEntityDestroy;
        }
        else
        {
            Debug.LogError("Can't delete entity; entity was not found.");
        }
    }

    public void Move(ICellEntity entity, Vector2Int newPosition)
    {
        if (_entitiesPositionsMap.TryGetValue(entity, out var position))
        {
            var cell = GetCellInternal(position);
            if (cell == null)
            {
                Debug.LogError("Can't get cell for entity; wrong position.");
                return;
            }

            cell?.Remove(entity);

            var newCell = GetCellInternal(newPosition);
            newCell.Put(entity);
            _entitiesPositionsMap[entity] = newPosition;
        }
        else
        {
            Debug.LogError("Can't move entity; entity was not found.");
        }
    }

    public Vector2Int GetEntityPosition(ICellEntity entity)
    {
        if (!_entitiesPositionsMap.TryGetValue(entity, out var position))
        {
            Debug.LogError("Can't get entity position; entity was not found.");
        }
        return position;
    }

    private void OnEntityDestroy(IDestroyable destroyable)
    {
        destroyable.OnDestroyEvent -= OnEntityDestroy;
        var entity = (ICellEntity)destroyable;
        Remove(entity);
    }
    #endregion
}