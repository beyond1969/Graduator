using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingletonMgr<T> : MonoBehaviour where T : SingletonMgr<T> {
    private static T instance = null;
    protected static bool initiated = false;

    protected abstract bool Init();

    public static T Instance
    {
        get
        {
            if(initiated) return instance;
            
            return null;
        }
    }

    public static bool CreateMgr()
    {
        if(initiated) return false;
        else
        {
            instance = new GameObject("[Manager]" + typeof(T).ToString(), typeof(T)).GetComponent<T>();

            initiated = true;
            instance.Init();
            DontDestroyOnLoad(instance.gameObject);
        }

        return true;
    }
}
