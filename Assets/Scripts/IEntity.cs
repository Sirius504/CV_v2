using System;
using UnityEngine;

public interface ICellHabitant
{
    public event Action<MonoBehaviour> OnDestroyEvent;
}