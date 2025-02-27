using System;
using UnityEngine;

public interface ITickable : IDestroyable
{
    public bool ReceiveTickIfCreatedDuringTick => false;
    public void OnTick(uint tick);
}