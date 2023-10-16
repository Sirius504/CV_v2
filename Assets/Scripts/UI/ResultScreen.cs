using TMPro;
using UnityEngine;

public class ResultScreen : MonoBehaviour, IInitializable
{
    [SerializeField] private WinLoseConditions _winLose;
    [SerializeField] private TextMeshProUGUI _resultText;

    public InitOrder InitOrder => InitOrder.UI;

    public void Init()
    {
        _winLose.OnGameStateChange += OnGameStateChange;
        OnGameStateChange(_winLose.GameState);
    }

    private void OnDestroy()
    {
        _winLose.OnGameStateChange -= OnGameStateChange;
    }

    private void OnGameStateChange(GameState newState)
    {
        gameObject.SetActive(newState != GameState.Playing);
        _resultText.text = newState switch
        {
            GameState.Playing => "",
            GameState.Win => "You Won!",
            GameState.Lose => "Game Over...",
            _ => throw new System.NotImplementedException(),
        };
    }
}