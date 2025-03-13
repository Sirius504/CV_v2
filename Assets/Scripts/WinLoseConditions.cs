using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Playing,
    Win,
    Lose
}

public class WinLoseConditions : MonoEntity, IInitializable, IUpdatable
{
    [SerializeField] private Level _level;
    [SerializeField] private Timer _roundTimer;

    private GameState _gameState;
    public GameState GameState
    {
        get => _gameState;
        private set
        {
            if (value == _gameState)
                return;
            _gameState = value;
            OnGameStateChange?.Invoke(_gameState);
        }
    }

    public event Action<GameState> OnGameStateChange;
    public InitOrder InitOrder => InitOrder.System;

    public UpdateOrder UpdateOrder => UpdateOrder.System;

    public void Init()
    {
        GameState = GameState.Playing;
        _roundTimer.TimerStart();
    }

    public void UpdateManual()
    {
        var entities = _level.Entities;


        if (!entities.OfType<Dog>().Any()
            || !entities.OfType<Player>().Any())
        {
            GameState = GameState.Lose;
            return;
        }

        if (!_roundTimer.IsRunning && !entities.OfType<Enemy>().Any())
        {
            GameState = GameState.Win;
            return;
        }
        GameState = GameState.Playing;
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
}