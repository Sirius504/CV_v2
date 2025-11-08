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
        _level.TryGetFirstWithComponent<Player>(out var player);
        _level.TryGetFirstWithComponent<Dog>(out var dog);
        _level.TryGetFirstWithComponent<Door>(out var door);

        var enemiesExist = _level.TryGetFirstWithComponent<Enemy>(out var _);

        if (player == null || dog == null)
        {
            GameState = GameState.Lose;
            return;
        }

        var playerPosition = _level.GetEntityPosition(player);
        var doorPosition = _level.GetEntityPosition(door);
        Debug.Log($"door: {doorPosition} - player: {playerPosition}");
        var doorReached = playerPosition == doorPosition;

        if (!_roundTimer.IsRunning && !enemiesExist && doorReached)
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