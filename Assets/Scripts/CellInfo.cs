using System;
using System.Collections.Generic;
using UnityEngine;

public class CellInfo : ICellInfo
{
    private readonly Dictionary<Type, ICellHabitant> _contents;

    public Vector2Int Position { get; }
    public IReadOnlyDictionary<Type, ICellHabitant> Contents => _contents;

    public CellInfo(Vector2Int position)
    {
        Position = position;
        _contents = new Dictionary<Type, ICellHabitant>();
    }

    public bool IsEmpty()
    {
        return _contents.Count == 0;
    }

    public bool Has(Type type)
    {
        return _contents.ContainsKey(type);
    }

    public void Put(Type type, ICellHabitant value)
    {
        _contents.Add(type, value);
    }

    public void Remove(Type type)
    {
        _contents.Remove(type);
    }

    public bool TryGet<T>(Type type, out T value) where T : ICellHabitant
    {
        if (_contents.TryGetValue(type, out var habitant))
        {
            value = (T)habitant;
            return true;
        }
        value = default;
        return false;
    }
}