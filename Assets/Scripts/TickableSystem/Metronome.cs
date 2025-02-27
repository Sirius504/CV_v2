using System;
using System.Collections.Generic;
using UnityEngine;

public class Metronome : SystemBase<Metronome, ITickable>, IUpdatable, IInitializable
{
    private const int RECURSIVE_TICKABLE_CREATION_LIMIT = 3;

    private uint _currentTick = 0;
    private float _beginningTime;
    private int _generation = 0;

    private List<HashSet<ITickable>> _tickBornTickables;
    private HashSet<ITickable> _tickables;

    [SerializeField] private float _tickDuration = 1f;

    public InitOrder InitOrder => InitOrder.System;
    public float TickDuration => _tickDuration;
    public float BeginningTime => _beginningTime;
    public void Init()
    {
        _beginningTime = Time.time;
        _currentTick = 0;

        _tickables = new HashSet<ITickable>();
        _tickBornTickables = new List<HashSet<ITickable>>();
    }

    public UpdateOrder UpdateOrder => UpdateOrder.Metronome;
    public void UpdateManual()
    {
        var ticksPassed = Mathf.FloorToInt((Time.time - _beginningTime) / _tickDuration);
        if (ticksPassed > _currentTick)
        {
            Tick();
        }
    }

    protected override void RegisterMany(IEnumerable<ITickable> tickables)
    {
        foreach(var tickable in tickables)
        {
            Register(tickable);
        }
    }

    private void Add(ITickable tickable)
    {
        if (_tickables.Add(tickable))
        {
            tickable.OnDestroyEvent += OnTickableDestroy;
        }
        else
        {
            Debug.LogWarning($"Trying to add tickable {((MonoBehaviour)tickable).name} but it was already added.");
        }
    }

    private void Register(ITickable tickable)
    {
        if (_generation > 0)
        {
            // means we registering entity that was created during tick
            if (_tickBornTickables.Count < _generation)
            {
                _tickBornTickables.Add(new HashSet<ITickable>());
            }

            _tickBornTickables[_generation - 1].Add(tickable);
            return;
        }

        Add(tickable);
    }

    public void Remove(ITickable tickable)
    {
        if (_tickables.Contains(tickable))
        {
            _tickables.Remove(tickable);
            tickable.OnDestroyEvent -= OnTickableDestroy;
        }
        else
        {
            throw new KeyNotFoundException($"Can't remove tickable; tickable {((MonoBehaviour)tickable).gameObject.name} was not found.");
        }
    }

    private void Tick()
    {
        _currentTick++;
        _generation++;
        foreach(var tickable in _tickables)
        {
            tickable.OnTick(_currentTick);
        }

        while (_tickBornTickables.Count >= _generation)
        {
            var currentGeneration = _tickBornTickables[_generation - 1];
            _generation++;
            foreach (var tickable in currentGeneration)
            {
                Add(tickable);

                if (!tickable.ReceiveTickIfCreatedDuringTick) continue;
                tickable.OnTick(_currentTick);
            }

            if (_generation > RECURSIVE_TICKABLE_CREATION_LIMIT)
            {
                throw new Exception("Recursive creation of ITickables exceeded limit");
            }
        }
        _tickBornTickables.Clear();
        _generation = 0;
    }

    private void OnTickableDestroy(IDestroyable mb)
    {
        Remove((ITickable)mb);
    }
}