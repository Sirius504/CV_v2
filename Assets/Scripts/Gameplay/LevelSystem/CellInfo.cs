using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CellInfo : ICellInfo
{
    private readonly List<ICellEntity> _entities;

    public Vector2Int Position { get; }
    public IReadOnlyCollection<ICellEntity> Entities => _entities;

    public CellInfo(Vector2Int position)
    {
        Position = position;
        _entities = new List<ICellEntity>();
    }

    public bool IsPassable()
    {
        if (_entities.Count == 0)
        {
            return true;
        }
        return _entities.All(entity => entity.Has<Passable>());
    }

    public bool Has<T>(out ICellEntity entity) where T : ICellComponent
    {
        entity = _entities.FirstOrDefault(entity => entity.Has<T>());
        return entity != null;
    }

    public void Put(ICellEntity value)
    {
        _entities.Add(value);
    }

    public void Remove(ICellEntity entity)
    {
        _entities.Remove(entity);
    }

    public bool TryGetFirst<T>(out T value) where T : ICellComponent
    {
        var matchingEntity = _entities.FirstOrDefault(entity => entity.Has<T>());
        foreach (var entity in _entities)
        {
            if (entity.TryGet(out value))
            {
                return true;
            }
        }

        value = default;
        return false;
    }

    public bool TryGetAll<T>(out IReadOnlyDictionary<ICellEntity, T> entities) where T : ICellComponent
    {
        Dictionary<ICellEntity, T> result = new();
        foreach (var entity in _entities)
        {
            if (!entity.TryGet<T>(out var value))
            {
                continue;
            }

            result.Add(entity, value);
        }

        entities = result;
        return result.Count > 0;
    }
}