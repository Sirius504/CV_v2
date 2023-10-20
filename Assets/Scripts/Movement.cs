using System;
using UnityEngine;

public class Movement : MonoEntity, IInjectable<Metronome, Level>, IUpdatable, IInitializable
{
    [SerializeField] private Enemy _enemy;
    [Range(0f, 1f)]
    [SerializeField] private float _animationSpeedTickPercent = 0.2f;

    private Metronome _metronome;
    private Level _level;

    private float _movementStartTime;

    private Vector2Int _oldPosition;

    private Vector3 _targetWorldPosition;
    private Vector3 _startWorldPosition;

    public UpdateOrder UpdateOrder => UpdateOrder.Animation;

    public InitOrder InitOrder => InitOrder.Animation;

    public void Inject(Metronome metronome, Level level)
    {
        _metronome = metronome;
        _level = level;
    }

    public void Init()
    {
        _enemy.OnMove += OnMove;
        _oldPosition = _level.WorldToCell(transform.position);
        _targetWorldPosition = _startWorldPosition = _level.CellToWorld(_oldPosition);
        transform.position = _startWorldPosition;
    }

    private void OnMove(Vector2Int newPosition)
    {
        _oldPosition = _level.WorldToCell(transform.position);
        _startWorldPosition = _level.CellToWorld(_oldPosition);
        _targetWorldPosition = _level.CellToWorld(newPosition);
        _movementStartTime = Time.time;
    }

    public void UpdateManual()
    {
        if (_targetWorldPosition == transform.position)
        {
            return;
        }

        var animationLength = _metronome.TickDuration * _animationSpeedTickPercent;
        var lerpFactor = (Time.time - _movementStartTime) / animationLength;
        transform.position = Vector3.Lerp(_startWorldPosition, _targetWorldPosition, Easing(lerpFactor));
    }

    private float Easing(float lerpFactor)
    {
        return Mathf.Clamp01(1f - (float)Math.Pow(2f, -10f * lerpFactor));
    }
}