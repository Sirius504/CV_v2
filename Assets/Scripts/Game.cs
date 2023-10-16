using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class Game : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private Level _level;
    [SerializeField] private Metronome _metronome;

    private List<ITickable> _tickables;

    void Start()
    {
        var monoBehaviours = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);

        // initializables
        foreach (var initializable in monoBehaviours
            .OfType<IInitializable>()
            .OrderBy(i => i.InitOrder))
        {
            initializable.Init();
        }

        // tickables
        _tickables = new List<ITickable>();
        _tickables.AddRange(monoBehaviours.OfType<ITickable>());
        _metronome.OnTick += OnTick;

        // cell habitants
        foreach (var cellHabitant in monoBehaviours.OfType<ICellHabitant>())
        {
            var mb = (MonoBehaviour)cellHabitant;
            var cellPosition = _level.WorldToCell(mb.transform.position);
            _level.Add(cellHabitant, cellPosition);
            mb.transform.position = _level.CellToWorld(cellPosition);
        }
    }

    // Update is called once per frame
    void Update()
    {
        _player.UpdateManual();
        _metronome.UpdateManual();
    }

    private void OnDestroy()
    {
        _metronome.OnTick -= OnTick;
    }

    private void OnTick(uint tick)
    {
        foreach(var tickable in _tickables)
        {
            tickable.OnTick(tick);
        }
    }
}
