using System;
using System.Collections.Generic;
using UnityEngine;

public interface ICellInfo
{
    public Vector2Int Position { get; }
    public IReadOnlyDictionary<Type, ICellHabitant> Contents { get; }

    public bool IsEmpty();
    public bool Has(Type type);
    public bool TryGet<T>(Type type, out T value) where T : ICellHabitant;
}