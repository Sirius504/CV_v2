using System.Linq;

public class Enemy : MyMono, ICellHabitant, ITickable
{
    private Level _level;

    public void Init(Level level)
    {
        _level = level;
    }

    private void Update()
    {
        transform.position = _level.CellToWorld(this);
    }

    public void OnTick(uint tick)
    {
        var adjacent = _level.GetAdjacentCells(this)
            .Where(cell => cell.IsEmpty())
            .Select(cell => cell.Position)
            .ToList();
        var target = adjacent[UnityEngine.Random.Range(0, adjacent.Count)];
        _level.Move(this, target);
    }
}