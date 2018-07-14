using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attack : MonoBehaviour {
    public Animation animation;

	// Use this for initialization
	void Start () {
        animation = GetComponent<Animation>();
        
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animation.Play("Chop Tree");
        }
	}
}
