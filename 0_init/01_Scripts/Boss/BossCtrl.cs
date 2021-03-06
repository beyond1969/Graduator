﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BossCtrl : MonoBehaviour {
    //public BossFSM fsm;

    private Transform m_targetCharacter;

    public float moveSpeed = 0.1f;
    //Rigidbody rigdBody;
    Transform xForm;

    private bool m_isAttack;

    public BossFSM fsm;
    Slider BossHealth;

    // Use this for initialization
    void Start () {
        m_targetCharacter = GameObject.FindGameObjectWithTag("Player").transform;
        //rigdBody = GetComponent<Rigidbody>();
        //fsm = GetComponent<BossFSM>();
        m_isAttack = false;
        fsm = GetComponent<BossFSM>();
        BossHealth = GameObject.Find("BossHealthBar").GetComponent<Slider>();
    }
	
	// Update is called once per frame
	void Update () {
        if(!m_isAttack)
        {
            fsm.setState(EEnemyState.Idle);
            if (BossHealth.value > 80)
            {
                StartCoroutine(Pattern1());
                m_isAttack = true;
            }
            if (BossHealth.value <= 80 && BossHealth.value > 60)
            {
                StartCoroutine(Pattern4());
                m_isAttack = true;
            }
            if (BossHealth.value <= 60 && BossHealth.value > 40)//WASD만 아님댐
            {
                StartCoroutine(Pattern5());
                m_isAttack = true;
            }
            if (BossHealth.value <= 40)//WASD만 아님댐
            {
                StartCoroutine(PatternCurve());
                m_isAttack = true;
            }
        }
        else
        {
            StartCoroutine(BossMoveChasing());
        }
    }

IEnumerator Pattern1()
{
    bool reverse = false;
    for (int i = 0; i < 10; i++)
    {
        if (i % 2 == 0)
            reverse = false;
        else
            reverse = true;

        for (int j = 0; j < 7; j++)
        {
            SfxManager.instance.playShot();
            Bullet bullet = BulletMgr.Instance.PopBullet();
            Quaternion rotQt;

            if (reverse)
                rotQt = Quaternion.Euler(0f, 35f - 10 * j, 0f);
            else
                rotQt = Quaternion.Euler(0f, -35f + 10 * j, 0f);

            Vector3 moveVec = rotQt * transform.forward;

            bullet.Init(transform.position + Vector3.up * 0.48f + transform.forward * 0.3f,
                moveVec, 8f, 20f, EBulletType.LINEAR, EBulletShooter.ENEMY);

            yield return new WaitForSeconds(0.2f);
        }
    }

    m_isAttack = false;
    yield return null;
}
IEnumerator Pattern4()
    {   
        //패턴짜는법
        for (int i = 0; i < 10; i++)    //돌리고싶은만큼 돌린다
        {
            SfxManager.instance.playShot();
            for (int j = 0; j < 20; j++)
            {
                Bullet bullet = BulletMgr.Instance.PopBullet();

                Quaternion rotQt = Quaternion.Euler(0f, 5f * i + 18f * (float)j, 0f);
                Vector3 moveVec = rotQt * transform.forward;    //이동방향 벡터
                
                //첫번째 Init인자는 시작위치,
                bullet.Init(transform.position + Vector3.up * 0.48f + moveVec * 0.3f,
                    moveVec, 5f, 20f, EBulletType.LINEAR, EBulletShooter.ENEMY);  //Init으로 기본 속성 설정해줌
                //이동방향, 탄속, 거리, 총알타입

                //yield return null;  //한 프레임마다 끊어주려면 씀
            }

            yield return new WaitForSeconds(0.2f); //루프 시간 간격
        }
        m_isAttack = false; //패턴 안겹치게 할라고 만듬
        yield return null;
    }

    IEnumerator Pattern5()
    {
        //패턴짜는법
        for (int i = 0; i < 10; i++)    //돌리고싶은만큼 돌린다
        {
            SfxManager.instance.playShot();
            Bullet bullet = BulletMgr.Instance.PopBullet();

            //첫번째 Init인자는 시작위치,
            bullet.Init(transform.position + Vector3.up * 0.48f + transform.forward * 0.3f,
                (m_targetCharacter.position - transform.position), 7f, 10f, EBulletType.CHASING, m_targetCharacter, EBulletShooter.ENEMY);  //Init으로 기본 속성 설정해줌
            //이동방향, 탄속, 거리, 총알타입, 적 트랜스폼
            yield return new WaitForSeconds(0.3f); //루프 시간 간격
        }

        m_isAttack = false; //패턴 안겹치게 할라고 만듬
        yield return null;
    }

    IEnumerator PatternCurve()
    {
        //패턴짜는법
        Vector3 pos = m_targetCharacter.position;   //seed 위치
        float randAngle;

        for (int i = 0; i < 10; i++)    //돌리고싶은만큼 돌린다
        {
            SfxManager.instance.playShot();
            Bullet bullet = BulletMgr.Instance.PopBullet();

            randAngle = Random.Range(0f, 360f);

            //첫번째 Init인자는 시작위치,
            bullet.Init(transform.position + Vector3.up * 0.48f + transform.forward * 0.3f,
                (m_targetCharacter.position - transform.position),  //의미 x,
                11f, 10f,   //탄속, 거리는 의미 x
                EBulletType.CURVE,
                pos + Random.Range(0, 3.5f) * (Vector3.forward * Mathf.Cos(randAngle) + Vector3.right * Mathf.Sin(randAngle)),  //타겟 위치
                EBulletShooter.ENEMY);
            yield return new WaitForSeconds(0.07f); //루프 시간 간격
        }

        m_isAttack = false; //패턴 안겹치게 할라고 만듬
        yield return null;
    }

    // 보스가 플레이어를 따라감
    IEnumerator BossMoveChasing()
    {
        Vector3 direction = (m_targetCharacter.position - transform.position).normalized;
        float step = moveSpeed * Time.deltaTime / 3;

        // 방향
        transform.forward = direction;

        // 이동
        transform.position = new Vector3(transform.position.x + (direction.x * step),
            transform.position.y,
            transform.position.z + (direction.z * step));

        fsm.setState(EEnemyState.Move);
        yield return null;
    }
}
