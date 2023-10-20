using System.Linq;
using System.Reflection;
using UnityEngine;

public class ForceAwakener : SingletonBehaviour<ForceAwakener>
{
    [SerializeField] private ResultScreen _screen;

    protected override void Awake()
    {
        var attribute = _screen.GetType().GetCustomAttribute<ForceAwakeAttribute>();
        var monoBehaviours = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (var monoBehaviour in monoBehaviours)
        {
            var methods = monoBehaviour.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var attributed = methods.Where(method => method.GetCustomAttribute<ForceAwakeAttribute>() != null);
            foreach (var methodInfo in attributed)
            {
                methodInfo.Invoke(monoBehaviour, null);
            }
        }
    }
}