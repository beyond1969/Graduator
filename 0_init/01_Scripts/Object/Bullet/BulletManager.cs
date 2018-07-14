using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : ManagerTemplate<BulletManager> {
    public int defaultAmount = 10;
    public PooledObj[] bulletPool;
    public int[] poolAmount;
    
}
