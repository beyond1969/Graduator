using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BossCtrl_First : MonoBehaviour {
    //public BossFSM fsm;

    private Transform m_targetCharacter;
    private GameObject[] arr_threadBoss;

    public float moveSpeed = 1.0f;
    //Rigidbody rigdBody;
    Transform xForm;

    private bool m_isAttack;
    private bool m_isMove;
    private float m_moveLength;
    private bool m_isThreadOn;

    Slider BossHealth;

    // 곡사 제어 변수
    private bool m_isCurveDelay;

    // 세갈래 제어 변수
    private bool m_isMoveCenterDone;
    private bool m_isMovingCenter;

    // Use this for initialization
    void Start () {
        m_targetCharacter = GameObject.FindGameObjectWithTag("Player").transform;
        //rigdBody = GetComponent<Rigidbody>();
        //fsm = GetComponent<BossFSM>();
        m_isAttack = false;
        BossHealth = GameObject.Find("BossHealthBar").GetComponent<Slider>();
        arr_threadBoss = GameObject.FindGameObjectsWithTag("CopyBoss");
    }
	
	// Update is called once per frame
	void Update () {
        if (!m_isMovingCenter)
            StartCoroutine(BossDirection());
        else if (m_isMovingCenter)
            StartCoroutine(BossCenterDirection());
        if(!m_isAttack)
        {
            if (BossHealth.value > 50)
            {
                StartCoroutine(PatternPointer());
                m_isAttack = true;
            }
            if (BossHealth.value <= 50 && BossHealth.value > 30)
            {
                if (!m_isMoveCenterDone)
                {
                    m_isMovingCenter = true;
                    StartCoroutine(BossMoveCenter());
                }
                else
                    StartCoroutine(PatternRecursive());
                m_isAttack = true;
            }
            if (BossHealth.value <= 30)
            {
                if(!m_isThreadOn)
                    PatternThreadInit();
                StartCoroutine(PatternThread());
                m_isAttack = true;
            }
        }
        if(m_isMove)
        {
            m_moveLength = 0.0f;
            StartCoroutine(BossMoveChasing());
            m_isMove = false;
        }
    }
    IEnumerator PatternPointer()
    {
        //패턴짜는법
        for (int i = 0; i < 5; i++)    //돌리고싶은만큼 돌린다
        {
            Vector3 pos = m_targetCharacter.position;   //seed 위치

            SfxManager.instance.playShot();
            Bullet bullet = BulletMgr.Instance.PopBullet();
            bullet.SetAsterisk(pos);

                //첫번째 Init인자는 시작위치,
                bullet.Init(transform.position + Vector3.up * 0.48f + transform.forward * 0.3f,
                    (m_targetCharacter.position - transform.position),  //의미 x,
                    11f, 10f,   //탄속, 거리는 의미 x
                    EBulletType.CURVE,
                    pos,  //타겟 위치
                    EBulletShooter.ENEMY);

            yield return new WaitForSeconds(0.7f); //루프 시간 간격
        }

        //m_isAttack = false; //패턴 안겹치게 할라고 만듬
        m_isMove = true;
        yield return null;
    }

    IEnumerator PatternRecursive()
    {
        // 세 갈래로 발사되는 탄막 생성
        SfxManager.instance.playShot();
        Bullet bulletUp = BulletMgr.Instance.PopBullet();
        Bullet bulletLeft = BulletMgr.Instance.PopBullet();
        Bullet bulletRight = BulletMgr.Instance.PopBullet();
        bulletUp.m_isTriple = true;
        bulletLeft.m_isTriple = true;
        bulletRight.m_isTriple = true;
        bulletUp.m_recursive = 3;
        bulletLeft.m_recursive = 3;
        bulletRight.m_recursive = 3;
        bulletUp.len = 5.0f;
        bulletLeft.len = 5.0f;
        bulletRight.len = 5.0f;

        Vector3 upDir = transform.position + new Vector3(0f, 0f, bulletUp.len);
        Vector3 LeftDiagDir = transform.position + new Vector3(-(float)System.Math.Sqrt(System.Math.Pow((double)bulletLeft.len / 2, (double)2)), 0f, -(float)System.Math.Sqrt(System.Math.Pow((double)bulletLeft.len / 2, (double)2)));
        Vector3 RightDiagDir = transform.position + new Vector3((float)System.Math.Sqrt(System.Math.Pow((double)bulletRight.len / 2, (double)2)), 0f, -(float)System.Math.Sqrt(System.Math.Pow((double)bulletRight.len / 2, (double)2)));

        //첫번째 Init인자는 시작위치,
        bulletUp.Init(transform.position + transform.forward * 0.3f,
            (upDir - transform.position).normalized,  //의미 x,
            3f, bulletUp.len,   //탄속, 거리는 의미 x
            EBulletType.LINEAR,
            EBulletShooter.ENEMY);
        bulletLeft.Init(transform.position + transform.forward * 0.3f,
            (LeftDiagDir - transform.position).normalized,  //의미 x,
            3f, bulletLeft.len,   //탄속, 거리는 의미 x
            EBulletType.LINEAR,
            EBulletShooter.ENEMY);
        bulletRight.Init(transform.position + transform.forward * 0.3f,
            (RightDiagDir - transform.position).normalized,  //의미 x,
            3f, bulletRight.len,   //탄속, 거리는 의미 x
            EBulletType.LINEAR,
            EBulletShooter.ENEMY);

        yield return new WaitForSeconds(0.5f); //루프 시간 간격

        m_isAttack = false; //패턴 안겹치게 할라고 만듬
        //m_isMove = true;
        yield return null;
    }

    void PatternThreadInit()
    {
        // 첫 실행일때만 구동
        if (!m_isThreadOn)
        {
            m_isThreadOn = true;
            // 분신 생성
            foreach(GameObject c in arr_threadBoss){
                c.transform.position = new Vector3(c.transform.position.x, 2.5f, c.transform.position.z);
                c.gameObject.GetComponent<BossThreadPattern>().setPattern(true);   // 복제 보스 패턴 시작
            }
            // 분신 총알 발사하게 만들기

        }
    }
    IEnumerator PatternThread()
    {
        bool reverse = false;
        for (int i = 0; i < 2; i++)
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

                Vector3 moveVec = rotQt * (transform.forward - new Vector3(0, transform.forward.y - m_targetCharacter.forward.y, 0));

                bullet.Init(new Vector3(transform.position.x, m_targetCharacter.position.y * 1.5f, transform.position.z) + transform.forward * 0.3f,
                    moveVec, 8f, 20f, EBulletType.LINEAR, EBulletShooter.ENEMY);

                yield return new WaitForSeconds(0.2f);
            }
        }

        m_isMove = true;

        yield return null;
    }

    IEnumerator Pattern1()
    {
        bool reverse = false;
        for (int i = 0; i < 2; i++)
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

                Vector3 moveVec = rotQt * (transform.forward - new Vector3(0,transform.forward.y - m_targetCharacter.forward.y, 0));

                bullet.Init(new Vector3(transform.position.x, m_targetCharacter.position.y * 1.5f, transform.position.z) + transform.forward * 0.3f,
                    moveVec, 8f, 20f, EBulletType.LINEAR, EBulletShooter.ENEMY);

                yield return new WaitForSeconds(0.2f);
            }
        }
    
        m_isMove = true;

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

    // 보스 바라보는 방향 고정
    IEnumerator BossDirection()
    {
        Vector3 bossAdjustYpos = new Vector3(transform.position.x, m_targetCharacter.position.y, transform.position.z);
        Vector3 direction = (m_targetCharacter.position - bossAdjustYpos).normalized;
        transform.forward = direction;

        yield return null;
    }

    IEnumerator BossCenterDirection()
    {
        Vector3 bossAdjustYpos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        Vector3 direction = (new Vector3(0.5f, transform.position.y, 13f) - bossAdjustYpos).normalized;
        transform.forward = direction;

        yield return null;
    }

    // 보스가 플레이어를 따라감
    IEnumerator BossMoveChasing()
    {
        while(m_moveLength < 10.0f)
        {
            float step = moveSpeed * Time.deltaTime / 2f;
            m_moveLength += step;
            // 이동
            transform.position += new Vector3((transform.forward.x * step),
                0,
                (transform.forward.z * step));
            yield return new WaitForSeconds(0.001f);

        }
        m_isAttack = false;

        yield return null;
    }
    IEnumerator BossMoveCenter()
    {
        while(Vector3.Distance(transform.position, new Vector3(0.5f, 0f, 13f)) >= 5f){
            float step = moveSpeed * Time.deltaTime / 2f;
            transform.position += new Vector3(transform.forward.x * step,
                0,
                transform.forward.z * step);
            yield return new WaitForSeconds(0.001f);
        }
        m_isMovingCenter = false;
        m_isAttack = false;
        m_isMoveCenterDone = true;

        yield return null;
    }
}
