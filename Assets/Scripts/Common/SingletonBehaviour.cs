using UnityEngine;

public abstract class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T>
{
    private static T instance;

    protected new virtual bool DontDestroyOnLoad => false;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = CreateSingletonInstance();
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {
        SingletonCheck();

        if (DontDestroyOnLoad)
        {
            GameObject.DontDestroyOnLoad(instance);
        }

        Debug.Log($"Singleton Awake: {gameObject.name} - {GetType()} - {GetInstanceID()}");
    }

#if UNITY_EDITOR
    protected virtual void Reset()
    {
        SingletonCheck();
    }

    protected virtual void OnValidate()
    {
        SingletonCheck();
    }
#endif

    private static T CreateSingletonInstance()
    {
        var existingObject = FindObjectOfType<T>();
        if (existingObject != null)
            return existingObject;
        var gameObject = new GameObject(typeof(T).Name);
        return gameObject.AddComponent<T>();
    }

    private void SingletonCheck()
    {
        if (instance != null && instance != this)
        {
            Debug.LogWarning($"Trying to instantiate singleton of type {typeof(T)}, but it already exists on game object \"{instance.gameObject.name}\".");
            DestroyImmediate(this);
            return;
        }

        instance = this as T;
    }
}