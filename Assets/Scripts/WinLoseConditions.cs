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

public class WinLoseConditions : MonoComponent, IInitializable, IUpdatable
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
        var player = entities.FirstOrDefault(entity => entity.Has<Player>());
        var dog = entities.FirstOrDefault(entity => entity.Has<Dog>());

        var enemiesExist = entities.Where(entity => entity.Has<Enemy>()).Any();

        if (player == null || dog == null)
        {
            GameState = GameState.Lose;
            return;
        }

        if (!_roundTimer.IsRunning && !enemiesExist)
        {
            GameState = GameState.Win;
            return;
        }
        GameState = GameState.Playing;
    }

    public void Restart()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}