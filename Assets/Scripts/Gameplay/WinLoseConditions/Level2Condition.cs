using UnityEngine;

public class Level2Condition : WinLoseConditionBase
{
    [SerializeField] private Timer _roundTimer;
    public override GameState GetState()
    {
        _level.TryGetFirstWithComponent<Player>(out var player);
        _level.TryGetFirstWithComponent<Dog>(out var dog);
        _level.TryGetFirstWithComponent<Door>(out var door);

        var enemiesExist = _level.TryGetFirstWithComponent<Enemy>(out var _);

        if (player == null || dog == null)
        {
            return GameState.Lose;
        }

        var playerPosition = _level.GetEntityPosition(player);
        var doorPosition = _level.GetEntityPosition(door);
        var doorReached = playerPosition == doorPosition;

        if (!_roundTimer.IsRunning && !enemiesExist && doorReached)
        {
            return GameState.Win;
        }
        return GameState.Playing;
    }
}
