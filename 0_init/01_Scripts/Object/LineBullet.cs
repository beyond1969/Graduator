using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineBullet : Bullet {
    public Vector3 moveVec = Vector3.forward;
    public float moveSpeed = 10f;
    public float range = 7;
    private float moveLen = 0;



	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {

        moveLen += moveSpeed * Time.deltaTime;

        if(moveLen > range)
        {
            moveLen = 0;
            Destroy(this.gameObject);
        }

        //Quaternion newRot = Quaternion.LookRotation(moveVec);
        //xForm.rotation = newRot;
    }

    public void SetMoveVec(Vector3 v) { moveVec = v; }
    public void SetMoveSpd(float s) { moveSpeed = s; }
}
