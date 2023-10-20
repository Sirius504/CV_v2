using System;
/// <summary>
/// Methods marked with this attribute will be called on Awake during scene launch, even if MonoBehaviour is disabled. You have to handle the "natural" Awake call.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class ForceAwakeAttribute : Attribute
{

}