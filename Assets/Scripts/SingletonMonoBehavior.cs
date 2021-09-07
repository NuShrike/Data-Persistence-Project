using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingletonMonoBehavior<T> : MonoBehaviour
{
    public static T Instance { get; private set; }

    private void Awake()
    {
        if (null == Instance) {
            Instance = (T)(object)this;
            DontDestroyOnLoad(gameObject);

            LoadSessionState();
        }
        else {
            Destroy(gameObject, 2);
        }
    }

    // override in subclass to customize session-data loading behavior
    protected virtual void LoadSessionState()
    {
    }
}
