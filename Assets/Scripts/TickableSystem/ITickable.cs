using System;
using UnityEngine;

public interface ITickable : IDestroyable
{
    public void OnTick(uint tick);
}