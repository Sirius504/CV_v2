using System.Collections.Generic;
using UnityEngine;

public interface IInitializable
{
    public int InitOrder { get; }
    public void Init();
}