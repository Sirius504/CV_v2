using System;
using UnityEngine;

public enum AnimationType
{
    Flat,
    TickPercent
}

[RequireComponent(typeof(ICellHabitant))]
public class Movement : MonoEntity, IInjectable<Metronome, Level>, IUpdatable
{
    private ICellHabitant _entity;
    [SerializeField] private AnimationType _animationType;
    [Range(0f, 1f)]
    [SerializeField] private float _animationTime = 0.2f;

    private Metronome _metronome;
    private Level _level;

    private float _movementStartTime;

    private Vector2Int _cellPosition;
    private Vector3 _targetWorldPosition;
    private Vector3 _startWorldPosition;

    public UpdateOrder UpdateOrder => UpdateOrder.Animation;
        

    public void Inject(Metronome metronome, Level level)
    {
        _metronome = metronome;
        _level = level;
        _entity = GetComponent<ICellHabitant>();
    }

    public void UpdateManual()
    {
        var newCellPosition = _level.GetEntityPosition(_entity);
        if (_cellPosition != newCellPosition)
        {
            _movementStartTime = Time.time;
            _startWorldPosition = _level.CellToWorld(_cellPosition);
            _targetWorldPosition = _level.CellToWorld(newCellPosition);
            _cellPosition = newCellPosition;
        }

        var animationTime = _animationType switch
        {
            AnimationType.Flat => _animationTime,
            AnimationType.TickPercent => _animationTime * _metronome.TickDuration,
            _ => throw new NotImplementedException()
        };
        var lerpFactor = Mathf.Clamp01((Time.time - _movementStartTime) / animationTime);
        transform.position = Vector3.Lerp(_startWorldPosition, _targetWorldPosition, Easing(lerpFactor));
    }

    private float Easing(float lerpFactor)
    {
        return Mathf.Clamp01(1f - (float)Math.Pow(2f, -10f * lerpFactor));
    }
}