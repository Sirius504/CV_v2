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

public class WinLoseConditions : MonoBehaviour, IInitializable
{
    [SerializeField] private Level _level;

    private GameState _gameState;
    public GameState GameState
    {
        get => _gameState;
        private set
        {
            _gameState = value;
            OnGameStateChange?.Invoke(_gameState);
        }
    }
    public event Action<GameState> OnGameStateChange;
    public InitOrder InitOrder => InitOrder.System;


    public void Init()
    {
        GameState = GameState.Playing;
    }

    public void UpdateManual()
    {
        if (GameState != GameState.Playing)
        {
            return;
        }

        var entities = _level.Entities;
        if (!entities.OfType<Enemy>().Any())
        {
            GameState = GameState.Win;
            return;
        }

        if (!entities.OfType<Dog>().Any()
            || !entities.OfType<Player>().Any())
        {
            GameState = GameState.Lose;
            return;
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
}