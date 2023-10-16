using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class LevelExtensions
{
    public static IEnumerable<Vector2Int> GetAdjacent(this Level level, int x, int y)
    {
        return new List<Vector2Int>
            {
                new Vector2Int(x + 1, y),
                new Vector2Int(x, y + 1),
                new Vector2Int(x - 1, y),
                new Vector2Int(x, y - 1),
            }.Where(pos => level.InBounds(pos));
    }

    public static IEnumerable<Vector2Int> GetAdjacent(this Level level, Vector2Int position)
    {
        return GetAdjacent(level, position.x, position.y);
    }

    public static IEnumerable<Vector2Int> GetAdjacent(this Level level, ICellHabitant entity)
    {
        return GetAdjacent(level, level.GetEntityPosition(entity));
    }

    public static IEnumerable<ICellInfo> GetAdjacentCells(this Level level, int x, int y)
    {
        return GetAdjacent(level, x, y).Select(pos => level.GetCell(pos));
    }
    public static IEnumerable<ICellInfo> GetAdjacentCells(this Level level, Vector2Int position)
    {
        return GetAdjacentCells(level, position.x, position.y);
    }
    public static IEnumerable<ICellInfo> GetAdjacentCells(this Level level, ICellHabitant entity)
    {
        return GetAdjacentCells(level, level.GetEntityPosition(entity));
    }
}