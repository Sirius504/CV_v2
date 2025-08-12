using System;
using UnityEngine;

public abstract class MonoEntity : MonoBehaviour, IDestroyable
{
    public event Action<IDestroyable> OnDestroyEvent;

    protected virtual void Awake()
    {
        Debug.Log($"Entity Awake: {gameObject.name} - {GetType()} - {GetInstanceID()}");
        Game.Instance.NotifyCreation(this);
    }

    protected virtual void OnDestroy()
    {
        OnDestroyEvent?.Invoke(this);
    }
}