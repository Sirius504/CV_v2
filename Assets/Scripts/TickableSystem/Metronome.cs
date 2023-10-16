using System.Collections.Generic;
using UnityEngine;

public class Metronome : MonoBehaviour, IInitializable
{
    [SerializeField] private float _tickDuration = 1f;

    private uint _currentTick = 0;
    private float _beginningTime;

    private HashSet<ITickable> _tickables;

    public InitOrder InitOrder => InitOrder.System;
    public void Init()
    {
        _beginningTime = Time.time;
        _currentTick = 0;

        _tickables = new HashSet<ITickable>();
    }

    public void UpdateManual()
    {
        var ticksPassed = Mathf.FloorToInt((Time.time - _beginningTime) / _tickDuration);
        if (ticksPassed > _currentTick)
        {
            Tick();
        }
    }

    public void Add(ITickable tickable)
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
        foreach(var tickable in _tickables)
        {
            tickable.OnTick(_currentTick);
        }
    }

    private void OnTickableDestroy(MonoBehaviour mb)
    {
        Remove((ITickable)mb);
    }
}