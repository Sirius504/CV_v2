using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Level : MonoBehaviour, IInitializable
{
    [SerializeField] private Vector2Int _gridSize;
    [SerializeField] private Grid _grid;

    public Vector2Int Size => _gridSize;

    private CellInfo[,] _cells;
    private Dictionary<ICellHabitant, Vector2Int> _entities;

    public int InitOrder => 1;
    public void Init(IEnumerable<MonoBehaviour> monoBehaviours)
    {
        _cells = new CellInfo[_gridSize.x, _gridSize.y];
        for (var i = 0; i < _gridSize.x; i++)
            for (var j = 0; j < _gridSize.y; j++)
            {
                _cells[i, j] = new CellInfo(new Vector2Int(i, j));
            }

        _entities = new Dictionary<ICellHabitant, Vector2Int>(64);

        // cell habitants
        foreach (var cellHabitant in monoBehaviours.OfType<ICellHabitant>())
        {
            var mb = (MonoBehaviour)cellHabitant;
            var cellPosition = WorldToCell(mb.transform.position);
            Add(cellHabitant, cellPosition);
            mb.transform.position = CellToWorld(cellPosition);
            cellHabitant.OnDestroyEvent += OnEntityDestroy;
        }
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
        return (Vector2Int)_grid.WorldToCell(position);
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

    public void Add(ICellHabitant entity, Vector2Int position)
    {
        if (_entities.TryAdd(entity, position))
        {
            var cell = GetCellInternal(position);
            cell.Put(entity.GetType(), entity);
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
            cell?.Remove(entity.GetType());
            _entities.Remove(entity);
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
            cell?.Remove(entity.GetType());

            var newCell = GetCellInternal(newPosition);
            newCell.Put(entity.GetType(), entity);
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

    private void OnEntityDestroy(MonoBehaviour mb)
    {
        var entity = (ICellHabitant)mb;
        Remove(entity);
        entity.OnDestroyEvent -= OnEntityDestroy;
    }
    #endregion
}
