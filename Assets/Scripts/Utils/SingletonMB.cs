using UnityEngine;
using System;

public class SingletonMB<T> : MonoBehaviour where T : class
{
    /// <summary>
    /// Makes the object singleton not be destroyed automatically when loading a new scene.
    /// </summary>
    public bool DontDestroy = false;

    static SingletonMB<T> instance;
    public static T Instance
    {
        get { return instance as T; }
    }

    protected virtual void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
        {
            instance = this;
            if (DontDestroy)
                DontDestroyOnLoad(gameObject);
            Initialize();
        }
    }

    protected virtual void Initialize()
    {
    }

    protected virtual void Shutdown()
    {
    }

    protected virtual void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
            Shutdown();
        }
    }

    protected virtual void OnApplicationQuit()
    {
        if (instance == this)
        {
            instance = null;
            Shutdown();
        }
    }
}