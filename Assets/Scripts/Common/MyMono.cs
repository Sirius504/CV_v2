using System;
using UnityEngine;

public class MyMono : MonoBehaviour, IInitializable
{
    public event Action<MonoBehaviour> OnDestroyEvent;

    public InitOrder InitOrder => InitOrder.Entity;

    public void Init()
    {
        Injector.Instance.RequestResolve(this);
    }

    private void OnDestroy()
    {
        OnDestroyEvent?.Invoke(this);
    }
}