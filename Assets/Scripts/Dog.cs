using System;
using UnityEngine;

public class Dog : MonoBehaviour, ICellHabitant
{
    public event Action<MonoBehaviour> OnDestroyEvent;

    private void OnDestroy()
    {
        OnDestroyEvent?.Invoke(this);
    }
}
