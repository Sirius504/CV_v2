using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Level : SystemBase<Level, ICellHabitant>, IInitializable
{
    [SerializeField] private Vector2Int _gridSize;
    [SerializeField] private Grid _grid;

    public Astar Astar { get; private set; }
    public Vector2Int Size => _gridSize;
    public IReadOnlyCollection<ICellHabitant> Entities => _entities.Keys;
    public IEnumerable<ICellInfo> Cells => _cells.Cast<ICellInfo>();

    private CellInfo[,] _cells;
    private Dictionary<ICellHabitant, Vector2Int> _entities;

    public InitOrder InitOrder => InitOrder.System;
    public void Init()
    {
        _cells = new CellInfo[_gridSize.x, _gridSize.y];
        for (var i = 0; i < _gridSize.x; i++)
            for (var j = 0; j < _gridSize.y; j++)
            {
                _cells[i, j] = new CellInfo(new Vector2Int(i, j));
            }

        _entities = new Dictionary<ICellHabitant, Vector2Int>(64);
        Astar = new Astar(this);
    }

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

    public bool InBounds(Vector2Int index)
    {
        return InBounds(index.x, index.y);
    }

    public bool InBounds(int x, int y)
    {
        return x >= 0 && x < _gridSize.x
            && y >= 0 && y < _gridSize.y;
    }

    public Vector2Int WorldToCell<T>(T entity) where T : MonoBehaviour, ICellHabitant
    {
        return WorldToCell(entity.transform.position);
    }

    public Vector2Int WorldToCell(Vector3 position)
    {
        var cellSpace = (Vector2Int)_grid.WorldToCell(position);
        cellSpace.Clamp(Vector2Int.zero, Size);
        return cellSpace;
    }

    public Vector3 CellToWorld(Vector2Int position)
    {
        return _grid.GetCellCenterWorld((Vector3Int)position);
    }

    public Vector3 CellToWorld<T>(T entity) where T : MonoBehaviour, ICellHabitant
    {
        return CellToWorld(_entities[entity]);
    }
    #endregion

    #region Entities related methods

    private void OnDestroy()
    {
        try
        {
            foreach (var entity in _entities.Keys)
            {
                entity.OnDestroyEvent -= OnEntityDestroy;
            }
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    protected override void RegisterMany(IEnumerable<ICellHabitant> habitants)
    {
        foreach(var habitant in habitants)
        {
            Register(habitant);
        }
    }

    private void Register(ICellHabitant entity)
    {
        var mb = (MonoBehaviour)entity;
        var cellPosition = WorldToCell(mb.transform.position);
        cellPosition.Clamp(Vector2Int.zero, _gridSize - Vector2Int.one);
        Add(entity, cellPosition);
        mb.transform.position = CellToWorld(cellPosition);
    }

    private void Add(ICellHabitant entity, Vector2Int position)
    {
        if (_entities.TryAdd(entity, position))
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

    public void Remove(ICellHabitant entity)
    {
        if (_entities.TryGetValue(entity, out var position))
        {
            var cell = GetCellInternal(position);
            cell?.Remove(entity);
            _entities.Remove(entity);
            entity.OnDestroyEvent -= OnEntityDestroy;
        }
        else
        {
            Debug.LogError("Can't delete entity; entity was not found.");
        }
    }

    public void Move(ICellHabitant entity, Vector2Int newPosition)
    {
        if (_entities.TryGetValue(entity, out var position))
        {
            var cell = GetCellInternal(position);
            cell?.Remove(entity);

            var newCell = GetCellInternal(newPosition);
            newCell.Put(entity);
            _entities[entity] = newPosition;
        }
        else
        {
            Debug.LogError("Can't move entity; entity was not found.");
        }
    }

    public Vector2Int GetEntityPosition(ICellHabitant entity)
    {
        return _entities[entity];
    }

    private void OnEntityDestroy(IDestroyable destroyable)
    {
        destroyable.OnDestroyEvent -= OnEntityDestroy;
        var entity = (ICellHabitant)destroyable;
        Remove(entity);
    }
    #endregion
}