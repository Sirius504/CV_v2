using System;
using System.Linq;
using UnityEngine;
using VContainer;
using Random = UnityEngine.Random;

public class Spawner : MonoEntity, IInitializable, ITickable
{
    [Inject]
    private Level _level;
    [Inject]
    private LevelGrid _levelGrid;

    [SerializeField] private int _spawnPeriod = 3;
    [SerializeField] private Enemy[] _enemies;
    [SerializeField] private bool _enabled = true;
    [SerializeField] private Timer _roundTimer;
    private Action _timerElapsedHandler;

    public InitOrder InitOrder => InitOrder.System;

    public void Init()
    {
        _timerElapsedHandler = () => _enabled = false;
        _roundTimer.OnElapsed += _timerElapsedHandler;
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

    protected override void OnDestroy()
    {
        base.OnDestroy();
        _roundTimer.OnElapsed -= _timerElapsedHandler;
    }
}
