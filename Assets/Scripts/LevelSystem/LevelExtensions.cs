using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class LevelExtensions
{
    public static IEnumerable<Vector2Int> AdjacentCoords(this LevelGrid grid, int x, int y)
    {
        return new List<Vector2Int>
            {
                new Vector2Int(x + 1, y),
                new Vector2Int(x, y + 1),
                new Vector2Int(x - 1, y),
                new Vector2Int(x, y - 1),
            }.Where(pos => grid.InBounds(pos));
    }

    public static IEnumerable<Vector2Int> GetAdjacent(this LevelGrid grid, Vector2Int position)
    {
        return AdjacentCoords(grid, position.x, position.y);
    }

    public static IEnumerable<Vector2Int> GetAdjacent(this LevelGrid grid, Level level, ICellHabitant entity)
    {
        return GetAdjacent(grid, level.GetEntityPosition(entity));
    }

    public static IEnumerable<ICellInfo> GetAdjacentCells(this LevelGrid grid, Level level, int x, int y)
    {
        return AdjacentCoords(grid, x, y).Select(pos => level.GetCell(pos));
    }
    public static IEnumerable<ICellInfo> GetAdjacentCells(this LevelGrid grid, Level level, Vector2Int position)
    {
        return GetAdjacentCells(grid, level, position.x, position.y);
    }
    public static IEnumerable<ICellInfo> GetAdjacentCells(this LevelGrid grid, Level level, ICellHabitant entity)
    {
        return GetAdjacentCells(grid, level, level.GetEntityPosition(entity));
    }

    public static bool TryGetRandomCell(this Level level, Func<ICellInfo, bool> predicate, out ICellInfo cell)
    {
        var fittingCells = level.Cells.Where(predicate).ToList();
        if (fittingCells.Any())
        {
            cell = fittingCells[UnityEngine.Random.Range(0, fittingCells.Count)];
            return true;
        }
        cell = default;
        return false;
    }

    public static bool IsAdjacent(this Level level, ICellHabitant entity, ICellHabitant target)
    {
        return IsAdjacent(level, level.GetEntityPosition(entity), level.GetEntityPosition(target));
    }

    public static bool IsAdjacent(this Level level, ICellHabitant entity, Vector2Int target)
    {
        return IsAdjacent(level, level.GetEntityPosition(entity), target);
    }

    public static bool IsAdjacent(this Level level, Vector2Int position, Vector2Int target)
    {
        var absX = Mathf.Abs(target.x - position.x);
        var absY = Mathf.Abs(target.y - position.y);
        return absX == 1 && absY == 0
            || absX == 0 && absY == 1;
    }
}