using System;
using UnityEngine;

public enum AnimationType
{
    Flat,
    TickPercent
}

[RequireComponent(typeof(ICellHabitant))]
public class Movement : MonoEntity, IInjectable<Metronome, Level, LevelGrid>, IUpdatable
{
    private ICellHabitant _entity;
    private Metronome _metronome;
    private Level _level;
    private LevelGrid _levelGrid;

    private float _movementStartTime;

    private Vector2Int? _cellPosition;
    private Vector3 _targetWorldPosition;
    private Vector3 _startWorldPosition;

    [SerializeField] private AnimationType _animationType;
    [Range(0f, 1f)]
    [SerializeField] private float _animationTime = 0.2f;

    public event Action<float, float> OnMovementStart;
    public UpdateOrder UpdateOrder => UpdateOrder.Animation;
        

    public void Inject(Metronome metronome, Level level, LevelGrid levelGrid)
    {
        _metronome = metronome;
        _level = level;
        _levelGrid = levelGrid;
        _entity = GetComponent<ICellHabitant>();
    }

    public void UpdateManual()
    {
        var newCellPosition = _level.GetEntityPosition(_entity);
        if (_cellPosition != newCellPosition)
        {
            _movementStartTime = Time.time;
            _startWorldPosition = _levelGrid.CellToWorld(_cellPosition ?? newCellPosition);
            _targetWorldPosition = _levelGrid.CellToWorld(newCellPosition);
            _cellPosition = newCellPosition;
            OnMovementStart?.Invoke(_movementStartTime, _animationTime);
        }

        if (transform.position == _targetWorldPosition)
            return;

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