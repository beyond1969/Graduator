﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Next_scene : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.anyKeyDown)
        {
            Application.LoadLevel("1_stage");
        }
	}
}
