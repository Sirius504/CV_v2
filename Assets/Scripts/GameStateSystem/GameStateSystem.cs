using System.Collections.Generic;

public class GameStateSystem : SystemBase<GameStateSystem, IGameStateChangeReceiver>
{
    protected override void RegisterMany(IEnumerable<IGameStateChangeReceiver> entities)
    {
        throw new System.NotImplementedException();
    }
}

