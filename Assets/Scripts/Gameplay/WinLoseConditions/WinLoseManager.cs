using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;

public enum GameState
{
    Playing,
    Win,
    Lose
}

public class WinLoseManager : MonoComponent, IInitializable, IUpdatable
{
    [Inject] private LevelLoader _levelLoader;
    [SerializeField] private Timer _roundTimer;
    [SerializeField] private WinLoseConditionBase _condition;

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
        GameState = _condition.GetState();
        if (GameState == GameState.Win)
        {
            _levelLoader.LoadLevelAsync(1);
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}