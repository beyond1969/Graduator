using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCtrlWithStick : MonoBehaviour {

    public JoyStick joyStick;
    public CharcterFSM fsm;

    public float moveSpeed = 8f;
    Rigidbody rigdBody;
    Transform xForm;

    Vector3 moveVec;
    float horizontalMove;
    float verticalMove;
    float sin45;
    float cos45;
    
    // Use this for initialization
    void Start () {
        rigdBody = GetComponent<Rigidbody>();
        fsm = GetComponent<CharcterFSM>();

        sin45 = Mathf.Sin(45f * Mathf.PI / 180f);
        cos45 = Mathf.Cos(45f * Mathf.PI / 180f);
    }
	
	// Update is called once per frame
	void Update () {
        horizontalMove = joyStick.GetHorizontalValue();
        verticalMove = joyStick.GetVerticalValue();

        moveVec.Set(horizontalMove * cos45 - verticalMove * sin45, 0, horizontalMove * sin45 + verticalMove * cos45);
        moveVec = moveVec.normalized;

        if(Input.GetMouseButtonDown(1))
        {
            fsm.setState(EUnitState.RangeAtk);
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            fsm.setState(EUnitState.Attack1);
            return;
        }

        if (Mathf.Abs(horizontalMove) < Mathf.Epsilon  && Mathf.Abs(verticalMove) < Mathf.Epsilon)
        {
            fsm.setState(EUnitState.Idle);
        }
        else
        {
            //move
            rigdBody.MovePosition(transform.position + moveVec * moveSpeed * Time.deltaTime);
            fsm.setState(EUnitState.Run);

            //Rotation
            Quaternion newRot = Quaternion.LookRotation(moveVec);
            rigdBody.MoveRotation(newRot);
        }
	}
}
