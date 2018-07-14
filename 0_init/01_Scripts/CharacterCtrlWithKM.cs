using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCtrlWithKM : MonoBehaviour {

    public CharcterFSM fsm;

    public float moveSpeed = 8f;
    Rigidbody rigdBody;

    public Camera cam;  //picking할 카메라

    Vector3 moveVec;
    float horizontalMove;
    float verticalMove;
    float sin45;
    float cos45;

    // Use this for initialization
    void Start()
    {
        rigdBody = GetComponent<Rigidbody>();
        fsm = GetComponent<CharcterFSM>();

        sin45 = Mathf.Sin(45f * Mathf.PI / 180f);
        cos45 = Mathf.Cos(45f * Mathf.PI / 180f);
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal");
        verticalMove = Input.GetAxisRaw("Vertical");

        moveVec.Set(horizontalMove * cos45 - verticalMove * sin45, 0, horizontalMove * sin45 + verticalMove * cos45);
        moveVec = moveVec.normalized;

        if (Input.GetMouseButtonDown(0) && fsm.currentState != EUnitState.RangeAtk)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000f, 1 << 10))
            {
                transform.LookAt(hit.point);
            }

            fsm.setState(EUnitState.Attack1);
            return;
        }

        if (Input.GetMouseButtonDown(1) && fsm.currentState != EUnitState.Attack1)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000f, 1 << 10))
            {
                transform.LookAt(hit.point);
            }

            fsm.setState(EUnitState.RangeAtk);

            Bullet bullet = BulletMgr.Instance.PopBullet();
            
            Vector3 moveVec = transform.forward;    //이동방향 벡터
            
            //첫번째 Init인자는 시작위치,
            bullet.Init(transform.position + Vector3.up * 0.48f + moveVec * 0.3f,
                    moveVec, 8f, 20f, EBulletType.LINEAR, EBulletShooter.PLAYER);  //Init으로 기본 속성 설정해줌

            return;
        }

        if((int)fsm.currentState < 2)
        {
            if (Mathf.Abs(horizontalMove) < Mathf.Epsilon && Mathf.Abs(verticalMove) < Mathf.Epsilon)
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
}
