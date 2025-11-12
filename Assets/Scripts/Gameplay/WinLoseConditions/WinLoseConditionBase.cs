using UnityEngine;
using VContainer;

public abstract class WinLoseConditionBase : MonoBehaviour
{
    [Inject] protected Level _level;
    public abstract GameState GetState();
}
