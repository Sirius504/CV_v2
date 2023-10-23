using System;
using UnityEngine;

public interface IDestroyable
{
    public event Action<IDestroyable> OnDestroyEvent;
}