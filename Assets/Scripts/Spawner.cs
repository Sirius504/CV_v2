using System.Linq;
using UnityEngine;

public class Spawner : MonoEntity, IInjectable<Level, LevelGrid>, ITickable
{
    private Level _level;
    private LevelGrid _levelGrid;
    [SerializeField] private int _spawnPeriod = 3;
    [SerializeField] private Enemy[] _enemies;
    [SerializeField] private bool _enabled = true;
    [SerializeField] private Timer _roundTimer;

    public void Inject(Level level, LevelGrid levelGrid)
    {
        _level = level;
        _levelGrid = levelGrid;
        _roundTimer.OnElapsed += () => _enabled = false;
    }

    public void OnTick(uint tick)
    {
        if (!_enabled) return;

        if (tick % _spawnPeriod == 0)
        {
            SpawnWave();
        }
    }

    private bool IsEligibleCell(ICellInfo cell)
    {
        if (!cell.IsEmpty()) return false;
        if (cell.Position.x != 0 && cell.Position.x != _levelGrid.Size.x - 1) return false;
        return true;
    }

    private void SpawnWave()
    {
        var amount = Random.Range(1, 4);
        for (int i = 0; i < amount; i++)
        {
            var eligibleCells = _level.Cells.Where(cell => IsEligibleCell(cell));
            if (!eligibleCells.Any()) break;

            var cell = eligibleCells.ElementAt(Random.Range(0, eligibleCells.Count()));
            var prefab = _enemies[Random.Range(0, _enemies.Length)];
            Instantiate(prefab, _levelGrid.CellToWorld(cell.Position), Quaternion.identity);
        }
    }
}
