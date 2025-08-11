using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    [SerializeField] private Vector2Int _gridSize;
    [SerializeField] private Grid _grid;
    public Vector2Int Size => _gridSize;

    public Vector2Int WorldToCell(Vector3 position)
    {
        Debug.LogWarning($"Unity Grid Instance ID: {_grid.GetInstanceID()}");
        var cellSpace = (Vector2Int)_grid.WorldToCell(position);
        cellSpace.Clamp(Vector2Int.zero, Size);
        return cellSpace;
    }

    public Vector3 CellToWorld(Vector2Int position)
    {
        return _grid.GetCellCenterWorld((Vector3Int)position);
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

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        for (int column = 0; column < _gridSize.x; column++)
            for (int row = 0; row < _gridSize.y; row++)
            {
                Gizmos.color = Color.cyan;
                var min = _grid.CellToWorld(new Vector3Int(column, row, 0));
                var max = _grid.CellToWorld(new Vector3Int(column, row, 0)) + _grid.cellSize;

                DrawLeft(column, min);
                DrawBottom(row, min);
                if (column == _gridSize.x - 1) DrawRight(max);
                if (row == _gridSize.y - 1) DrawTop(max);
            }
    }

    private void DrawLeft(int column, Vector3 min)
    {
        Gizmos.color = column == 0 ? Color.blue : Color.cyan;
        Gizmos.DrawLine(min, min + Vector3.up * _grid.cellSize.y);
    }

    private void DrawBottom(int row, Vector3 min)
    {
        Gizmos.color = row == 0 ? Color.blue : Color.cyan;
        Gizmos.DrawLine(min, min + Vector3.right * _grid.cellSize.x);
    }

    private void DrawRight(Vector3 max)
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(max, max + Vector3.down * _grid.cellSize.y);
    }

    private void DrawTop(Vector3 max)
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(max, max + Vector3.left * _grid.cellSize.x);
    }
#endif
}
