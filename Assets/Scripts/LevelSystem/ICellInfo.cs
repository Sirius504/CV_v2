using System;
using System.Collections.Generic;
using UnityEngine;

public interface ICellInfo
{
    public Vector2Int Position { get; }
    public IReadOnlyCollection<ICellHabitant> Contents { get; }

    public bool IsEmpty();
    public bool Has<T>();
    public bool TryGet<T>(out T value) where T : ICellHabitant;
}