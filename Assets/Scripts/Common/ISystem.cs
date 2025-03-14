﻿using System.Collections.Generic;
using UnityEngine;

public enum SystemsStartOrder
{
    Injector = 0,
    Initializer = 1,
    Updater = 2,

    Default = 3
}


public interface ISystem
{
    public SystemsStartOrder ResolutionOrder { get; }
    public void Register(IEnumerable<MonoBehaviour> entities);
}