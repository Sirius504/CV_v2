using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CellInfo : ICellInfo
{
    private readonly List<ICellHabitant> _contents;

    public Vector2Int Position { get; }
    public IReadOnlyCollection<ICellHabitant> Contents => _contents;

    public CellInfo(Vector2Int position)
    {
        Position = position;
        _contents = new List<ICellHabitant>();
    }

    public bool IsEmpty()
    {
        return _contents.Count == 0;
    }

    public bool Has<T>()
    {
        return _contents.Any(entity => entity is T);
    }

    public void Put(ICellHabitant value)
    {
        _contents.Add(value);
    }

    public void Remove(ICellHabitant entity)
    {
        _contents.Remove(entity);
    }

    public bool TryGet<T>(out T value) where T : ICellHabitant
    {
        var matchingEntity = _contents.FirstOrDefault(entity => entity is T);
        if (matchingEntity != null)
        {
            value = (T)matchingEntity;
            return true;
        }
        value = default;
        return false;
    }
}