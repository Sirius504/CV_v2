using System.Linq;
using UnityEngine;

public class Spawner : MonoEntity, IInjectable<Level>, ITickable
{
    private Level _level;
    [SerializeField] private int _spawnPeriod = 3;
    [SerializeField] private Enemy[] _enemies;

    public void Inject(Level level)
    {
        _level = level;
    }

    public void OnTick(uint tick)
    {
        if (tick % _spawnPeriod == 0)
        {
            SpawnWave();
        }
    }

    private bool IsEligibleCell(ICellInfo cell)
    {
        if (!cell.IsEmpty()) return false;
        if (cell.Position.x != 0 && cell.Position.x != _level.Size.x - 1) return false;
        return true;
    }

    private void SpawnWave()
    {
        var amount = Random.Range(1, 4);
        Debug.Log($"Spawning {amount}...");
        for (int i = 0; i < amount; i++)
        {
            var eligibleCells = _level.Cells.Where(cell => IsEligibleCell(cell));
            if (!eligibleCells.Any()) break;

            var cell = eligibleCells.ElementAt(Random.Range(0, eligibleCells.Count()));
            var prefab = _enemies[Random.Range(0, _enemies.Length)];
            Instantiate(prefab, _level.CellToWorld(cell.Position), Quaternion.identity);
            Debug.Log($"Spawned {prefab.name}.");
        }
    }
}
