using System.Collections.Generic;
using UnityEngine;

public interface IInitializable
{ 
    public int InitOrder { get; }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="monoBehaviours">All MonoBehaviours on scene at the moment of initialization</param>
    public void Init(IEnumerable<MonoBehaviour> monoBehaviours);
}