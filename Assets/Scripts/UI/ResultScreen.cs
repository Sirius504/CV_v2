using TMPro;
using UnityEngine;

public class ResultScreen : MonoEntity, IInitializable
{
    [SerializeField] private WinLoseConditions _winLose;
    [SerializeField] private TextMeshProUGUI _resultText;
    [SerializeField] private Canvas _canvas;

    public InitOrder InitOrder => InitOrder.UI;


    public void Init()
    {
        _winLose.OnGameStateChange += OnGameStateChange;
        OnGameStateChange(_winLose.GameState);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        _winLose.OnGameStateChange -= OnGameStateChange;
    }

    private void OnGameStateChange(GameState newState)
    {
        _canvas.enabled = newState != GameState.Playing;
        _resultText.text = newState switch
        {
            GameState.Playing => "",
            GameState.Win => "You Won!",
            GameState.Lose => "Game Over...",
            _ => throw new System.NotImplementedException(),
        };
    }
}