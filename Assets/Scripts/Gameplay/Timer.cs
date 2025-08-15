using System;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private float _startTime;
    private bool _isRunning;

    [SerializeField] private float _timeInSeconds;
    public float TotalTime => _timeInSeconds;
    public float TimeRemaining => Math.Max(0f, _timeInSeconds - (Time.time - _startTime));

    public bool IsRunning => _isRunning;

    public event Action OnElapsed;

    public void TimerStart()
    {
        _startTime = Time.time;
        _isRunning = true;
    }

    private void Update()
    {
        if (!_isRunning)
        {
            return;
        }

        if (TimeRemaining > 0f)
        {
            return;
        }

        OnElapsed?.Invoke();
        _isRunning = false;
    }
}