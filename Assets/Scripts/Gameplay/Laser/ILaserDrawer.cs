using UnityEngine;

public interface ILaserDrawer : IDestroyable
{
    bool Enabled { get; }
    Vector2Int Source { get; }
    Vector2Int Target { get; }
    float Progress { get; }
}