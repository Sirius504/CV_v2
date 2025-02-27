using ActionBehaviour;
using System;

public interface IActioning
{
    public event Action<ActionInfo> OnAction;
}