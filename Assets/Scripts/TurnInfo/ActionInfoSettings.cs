using System;
using UnityEngine;

public class ActionInfoSettings : SingletonBehaviour<ActionInfoSettings>
{
    [SerializeField]
    private ArrowColors _arrowColors;

    public ArrowColors ArrowColors => _arrowColors;

    public static Color GetColor(Action action)
    {
        return action switch
        {
            Action.Movement => Instance.ArrowColors.Move,
            Action.Attack => Instance.ArrowColors.Attack,
            _ => throw new NotImplementedException()
        };
    }
}

[Serializable]
public class ArrowColors
{
    public Color Attack;
    public Color Move;
}