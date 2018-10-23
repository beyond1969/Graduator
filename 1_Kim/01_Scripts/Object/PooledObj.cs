using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledObj {
    public string objName = string.Empty;
    public GameObject prefab = null;
    public int poolCount = 10;

    public List<GameObject> unusedList = new List<GameObject>();
}
