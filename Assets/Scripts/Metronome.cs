using System;
using UnityEngine;

public class Metronome : MonoBehaviour, IInitializable
{
    [SerializeField] private float _tickDuration = 1f;

    private uint _currentTick = 0;
    private float _beginningTime;

    public Action<uint> OnTick;

    public int InitOrder => 1;
    public void Init()
    {
        _beginningTime = Time.time;
        _currentTick = 0;
    }

    public void UpdateManual()
    {
        var ticksPassed = Mathf.FloorToInt((Time.time - _beginningTime) / _tickDuration);
        if (ticksPassed > _currentTick)
        {
            OnTick?.Invoke(++_currentTick);
        }
    }
}
