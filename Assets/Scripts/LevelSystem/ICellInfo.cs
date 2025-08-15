using System.Collections.Generic;
using UnityEngine;

public interface ICellInfo
{
    public Vector2Int Position { get; }
    public IReadOnlyCollection<ICellEntity> Entities { get; }

    public bool IsEmpty();
    public bool Has<T>(out ICellEntity entity) where T : ICellComponent;
    public bool TryGetFirst<T>(out T value) where T : ICellComponent;

    public bool TryGetAll<T>(out IReadOnlyDictionary<ICellEntity, T> entities) where T : ICellComponent;
}