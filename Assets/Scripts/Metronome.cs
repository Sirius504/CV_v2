using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Metronome : MonoBehaviour, IInitializable
{
    [SerializeField] private float _tickDuration = 1f;

    private uint _currentTick = 0;
    private float _beginningTime;

    private HashSet<ITickable> _tickables;

    public int InitOrder => 1;
    public void Init(IEnumerable<MonoBehaviour> monoBehaviours)
    {
        _beginningTime = Time.time;
        _currentTick = 0;

        _tickables = new HashSet<ITickable>();
        foreach (var tickable in monoBehaviours.OfType<ITickable>())
        {
            _tickables.Add(tickable);
            tickable.OnDestroyEvent += OnTickableDestroy;
        }
    }

    public void UpdateManual()
    {
        var ticksPassed = Mathf.FloorToInt((Time.time - _beginningTime) / _tickDuration);
        if (ticksPassed > _currentTick)
        {
            Tick();
        }
    }

    private void Tick()
    {
        _currentTick++;
        foreach(var tickable in _tickables)
        {
            tickable.OnTick(_currentTick);
        }
    }

    private void OnTickableDestroy(MonoBehaviour mb)
    {
        var tickable = (ITickable)mb;
        _tickables.Remove(tickable);
        tickable.OnDestroyEvent -= OnTickableDestroy;
    }
}