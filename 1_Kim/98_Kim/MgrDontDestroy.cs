using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MgrDontDestroy : MonoBehaviour {
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
