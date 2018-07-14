using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MgrInitializer : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        BulletMgr.CreateMgr();
	}
}
