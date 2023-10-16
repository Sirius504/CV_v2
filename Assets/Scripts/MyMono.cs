using System;
using UnityEngine;

public class MyMono : MonoBehaviour
{
    public event Action<MonoBehaviour> OnDestroyEvent;

    private void Awake()
    {
        Injector.Instance.RequestResolve(this);
    }

    private void OnDestroy()
    {
        OnDestroyEvent?.Invoke(this);
    }
}