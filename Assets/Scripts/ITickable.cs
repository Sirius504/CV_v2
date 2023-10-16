using System;
using UnityEngine;

public interface ITickable
{
    public void OnTick(uint tick);

    public event Action<MonoBehaviour> OnDestroyEvent;
}