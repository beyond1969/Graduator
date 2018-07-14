using UnityEngine;
using System.Collections;
using System.Collections.Generic;


//@BJ 20160728
// 매니저 템플릿
// 모든 매니저들은 매니저 탬플릿을 상속받는다.
public abstract class ManagerTemplate<T> : MonoBehaviour where T : ManagerTemplate<T> 
{
    
    public static bool IsInitialized { private set; get; }

    private static T instance = null;
    public static T Instance
    {
        get
        {
            if (IsInitialized == false )
            {
                Debug.LogError("생성 안됨");
                return null;
            }

            return instance;
        }
    }

    public static bool Create()
    {
        if (instance != null)
        {
            Debug.LogError("이미 존재");
            return false;
        }
        else
        {
            instance = GameObject.FindObjectOfType(typeof(T)) as T;
            if (instance != null)
            {
                Debug.LogError("이미 존재");
                return false;
            }

            instance = new GameObject("[Manager]" + typeof(T).ToString(), typeof(T)).GetComponent<T>();
            if (instance == null) {
                Debug.LogError("매니저 생성 실패 : " + typeof(T).ToString());
                return false;
            }

            IsInitialized = true;
            instance.Init();
            DontDestroyOnLoad(instance.gameObject);
        }
        
        return true;
    }

    public virtual void Init() { }

    /// Make sure the instance isn't referenced anymore when the user quit, just in case.
    void OnDestroy()
    {
        IsInitialized = false;
        instance = null;
    }

    void OnApplicationQuit()
    {
        IsInitialized = false;
        instance = null;
    }
}
