﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EBulletShooter
{
    NONE,
    PLAYER,
    ENEMY,
}

public enum EBulletType
{
    NONE,
    LINEAR,
    CURVE,
    CHASING,
    BOUNCING,
}

public class Bullet : MonoBehaviour {
    private Transform m_targetCharacter;

    EBulletShooter m_bulletShooter;
    protected EBulletType m_bulletType;
    protected BulletMgr m_parentMgr;
    public float m_bulletSpd;
    public float m_bulletRange;
    public float m_moveLen;

    public Vector3 m_moveVec = Vector3.zero;

    //곡사용 멤버변수
    public List<Vector3> m_curvePoints = new List<Vector3>();
    private int m_curveSegment;
    private bool m_isAsterisk;
    private Vector3 m_asteriskPos;
    private GameObject obj_asterisk;

    // 세갈레 총알 발사용
    public bool m_isTriple;
    public int m_recursive; // 재귀 회수
    public float len;       // 탄막 이동 거리

    // 체력 처리용
    Slider playerHealth;
    Slider bossHealth;
    float pHp;      // 플레이어 체력
    float bHp;      // 보스 체력
    const float defaultBossDamage = 10.0f;  // 플레이어가 받는 기본 대미지
    float playerDamage;               // 플레이어가 주는 대미지
    ItemCreate itemCreate;
    CharacterStatus characterStatus;

    void Start()
    {
        playerHealth = GameObject.Find("HealthBar").GetComponent<Slider>();
        bossHealth = GameObject.Find("BossHealthBar").GetComponent<Slider>();
        pHp = playerHealth.value;
        bHp = bossHealth.value;
        characterStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterStatus>();
        playerDamage = characterStatus.getDamage();
        itemCreate = GameObject.Find("ItemCreateMgr").GetComponent<ItemCreate>();
    }

    void Update()
    {
        MoveByType();
    }

    public void Init()
    {
        m_bulletShooter = EBulletShooter.NONE;
        m_bulletType = EBulletType.NONE;
        m_moveLen = 0;
        m_targetCharacter = null;
    }

    public void Init(Vector3 startPos, Vector3 moveVec, float bSpd, float range, EBulletType type, EBulletShooter shooter)
    {
        transform.position = startPos;
        m_moveVec = moveVec;
        m_bulletSpd = bSpd;
        m_bulletShooter = shooter;
        m_bulletRange = range;
        m_bulletType = type;
        m_moveLen = 0;
    }

    public void Init(Vector3 startPos, Vector3 moveVec, float bSpd, float range, EBulletType type, Transform target, EBulletShooter shooter)
    {
        //유도
        m_targetCharacter = target;

        transform.position = startPos;
        m_moveVec = moveVec;
        m_bulletSpd = bSpd;
        m_bulletRange = range;
        m_bulletShooter = shooter;
        m_bulletType = type;
        m_moveLen = 0;
    }

    public void Init(Vector3 startPos, Vector3 moveVec, float bSpd, float range, EBulletType type, Vector3 target, EBulletShooter shooter)
    {
        //곡사용
        transform.position = startPos;
        m_moveVec = moveVec;
        m_bulletSpd = bSpd;
        m_bulletRange = range;  //곡사일경우 Length비례 Range(총 보간 time)이 정해짐
        m_bulletShooter = shooter;
        m_bulletType = type;
        m_moveLen = 0;

        if (type == EBulletType.CURVE)   //시작지점 조정 추가필요
        {
            Vector3 center = (target + transform.position) / 2f;

            m_moveVec = (target - transform.position);

            m_bulletRange = m_moveVec.magnitude;


            Quaternion rotZAxis = Quaternion.Euler(0, 0, Random.Range(-80f, 80f));
            Quaternion rotToward = Quaternion.LookRotation(m_moveVec.normalized);


            if (m_curvePoints.Count > 0) //전에 사용된 control point가 있을 경우
            {
                m_curvePoints[0].Set(0f, 0f, -0.5f);
                m_curvePoints[1].Set(0f, 0f, -0.5f);
                m_curvePoints[2].Set(0f, 0f, -0.5f);

                m_curvePoints[3].Set(0f, 0.65f, -0.3f);
                m_curvePoints[4].Set(0f, 0.1f, 0.4f);

                m_curvePoints[5].Set(0f, 0f, 0.5f);
                m_curvePoints[6].Set(0f, 0f, 0.5f);
                m_curvePoints[7].Set(0f, 0f, 0.5f);
            }
            else //없을 경우
            {
                m_curvePoints.Add(new Vector3(0f, 0f, -0.5f));
                m_curvePoints.Add(new Vector3(0f, 0f, -0.5f));
                m_curvePoints.Add(new Vector3(0f, 0f, -0.5f));

                m_curvePoints.Add(new Vector3(0f, 0.25f, -0.3f));
                m_curvePoints.Add(new Vector3(0f, 0.1f, 0.35f));

                m_curvePoints.Add(new Vector3(0f, 0f, 0.5f));
                m_curvePoints.Add(new Vector3(0f, 0f, 0.5f));
                m_curvePoints.Add(new Vector3(0f, 0f, 0.5f));
            }

            for (int i = 0; i < m_curvePoints.Count; i++)
            {
                m_curvePoints[i] *= m_bulletRange;  //scale
                m_curvePoints[i] = rotZAxis * m_curvePoints[i];  //rotate
                m_curvePoints[i] = rotToward * m_curvePoints[i];  //rotate
                m_curvePoints[i] += center; //translate
            }

            m_curveSegment = 0;
        }
    }

    private void MoveByType()
    {
        switch (m_bulletType)
        {
            case EBulletType.NONE:
                    m_parentMgr.PushBullet(this);
                break;
            case EBulletType.LINEAR:
                {
                    Vector3 delta = m_moveVec * m_bulletSpd * Time.deltaTime;

                    if (m_moveLen > m_bulletRange)
                    {
                        if (m_isTriple && m_recursive > 0)
                        {
                            Bullet bulletUp = BulletMgr.Instance.PopBullet();
                            Bullet bulletLeft = BulletMgr.Instance.PopBullet();
                            Bullet bulletRight = BulletMgr.Instance.PopBullet();
                            bulletUp.m_isTriple = true;
                            bulletLeft.m_isTriple = true;
                            bulletRight.m_isTriple = true;
                            bulletUp.m_recursive = m_recursive - 1;
                            bulletLeft.m_recursive = m_recursive - 1;
                            bulletRight.m_recursive = m_recursive - 1;
                            bulletUp.len = len;
                            bulletLeft.len = len;
                            bulletRight.len = len;

                            Vector3 upDir = transform.position + new Vector3(0f, 0f, bulletUp.len);
                            Vector3 LeftDiagDir = transform.position + new Vector3(-(float)System.Math.Sqrt(System.Math.Pow((double)bulletLeft.len / 2, (double)2)), 0f, -(float)System.Math.Sqrt(System.Math.Pow((double)bulletLeft.len / 2, (double)2)));
                            Vector3 RightDiagDir = transform.position + new Vector3((float)System.Math.Sqrt(System.Math.Pow((double)bulletRight.len / 2, (double)2)), 0f, -(float)System.Math.Sqrt(System.Math.Pow((double)bulletRight.len / 2, (double)2)));

                            //첫번째 Init인자는 시작위치,
                            bulletUp.Init(transform.position,
                                (upDir - transform.position).normalized,  //의미 x,
                                3f, len,   //탄속, 거리는 의미 x
                                EBulletType.LINEAR,
                                EBulletShooter.ENEMY);
                            bulletLeft.Init(transform.position,
                                (LeftDiagDir - transform.position).normalized,  //의미 x,
                                3f, len,   //탄속, 거리는 의미 x
                                EBulletType.LINEAR,
                                EBulletShooter.ENEMY);
                            bulletRight.Init(transform.position,
                                (RightDiagDir - transform.position).normalized,  //의미 x,
                                3f, len,   //탄속, 거리는 의미 x
                                EBulletType.LINEAR,
                                EBulletShooter.ENEMY);
                        }
                        m_isTriple = false;
                        m_parentMgr.PushBullet(this);
                    }
                    transform.position += delta;
                    m_moveLen += m_bulletSpd * Time.deltaTime;
                }
                break;
            case EBulletType.CURVE:
                {
                    m_moveLen += Time.deltaTime * m_bulletSpd / 2.5f;    //시간변화량
                    if (m_moveLen >= m_bulletRange / (float)(m_curvePoints.Count - 3))
                    {
                        m_moveLen = 0f;
                        m_curveSegment++;
                    }
                    if (m_curveSegment >= (m_curvePoints.Count - 3))    // 곡사 탄막 제거되는 구문
                    {
                        m_moveLen = 0f;
                        m_curveSegment = 0;
                        m_parentMgr.PushBullet(this); // 이때 제거
                        if (m_isAsterisk)
                        {
                            Destroy(obj_asterisk);
                        }
                    }

                    B_Spline_Move();
                }
                break;
            case EBulletType.CHASING:
                {
                    Vector3 delta = m_moveVec * m_bulletSpd * Time.deltaTime;

                    if (m_moveLen > m_bulletRange) m_parentMgr.PushBullet(this);

                    transform.position += delta;
                    m_moveLen += m_bulletSpd * Time.deltaTime;

                    m_moveVec = (m_targetCharacter.position - transform.position).normalized;
                }
                break;
            case EBulletType.BOUNCING:
                {
                    m_parentMgr.PushBullet(this);
                }
                break;
        }
    } 

    private void B_Spline_Move()
    {
        float interval = m_bulletRange / (float)(m_curvePoints.Count - 3);

        float t = m_moveLen / interval;
        float t_sqr = Mathf.Pow(t, 2);
        float t_cub = Mathf.Pow(t, 3);

        float B0 = (1f / 6f) * Mathf.Pow(1f - t, 3f);
        float B1 = (1f / 6f) * (3f * t_cub - 6f * t_sqr + 4f);
        float B2 = (1f / 6f) * (-3f * t_cub + 3f * t_sqr + 3f * t + 1f);
        float B3 = (1f / 6f) * t_cub;

        transform.position = (B0 * m_curvePoints[m_curveSegment]
                            + B1 * m_curvePoints[m_curveSegment + 1]
                            + B2 * m_curvePoints[m_curveSegment + 2]
                            + B3 * m_curvePoints[m_curveSegment + 3]);
    }

    void OnTriggerEnter(Collider col)
    {
        switch(m_bulletShooter)
        {
            case EBulletShooter.PLAYER:
                if(col.transform.tag == "Boss")
                {
                    m_parentMgr.PushBullet(this);
                    if(bossHealth.value > 0.0f)
                    {
                        SfxManager.instance.playHurt();
                        bossHealth.value -= playerDamage;
                    }
                    else
                    {
                        // 클리어
                        playerHealth.value = characterStatus.getMaxHp();
                        pHp = playerHealth.value;
                        Text playertext = GameObject.Find("Canvas").transform.Find("HealthText").GetComponent<Text>();
                        playertext.text = pHp.ToString();
                        // 1 스테이지 복제 보스 제거
                        GameObject[] arr_copies = GameObject.FindGameObjectsWithTag("CopyBoss");
                        if(arr_copies != null)
                        {
                            foreach(GameObject c in arr_copies)
                            {
                                c.GetComponent<BossThreadPattern>().setPattern(false);
                                c.SetActive(false);
                            }
                        }
                        // 복제 보스 제거 끝
                        itemCreate.StageClear(1);
                    }
                }
                break;
            case EBulletShooter.ENEMY:
                if (col.transform.tag == "Player")
                {
                    m_parentMgr.PushBullet(this);
                    if(pHp > 0.0f)
                    {
                        SfxManager.instance.playHurt();
                        playerHealth.value -= defaultBossDamage;
                        pHp -= defaultBossDamage;
                        Text playertext = GameObject.Find("Canvas").transform.Find("HealthText").GetComponent<Text>();
                        playertext.text = pHp.ToString();
                    }
                    if(pHp <= 0.0f)
                    {
                        // 게임 오버
                        Text gameOver = GameObject.Find("Canvas").transform.Find("GameOver").GetComponent<Text>();
                        gameOver.gameObject.SetActive(true);
                        Time.timeScale = 0;
                    }
                }
                break;
            default:
                break;
        }


    }

    public void SetBT(EBulletType type) { m_bulletType = type; }
    public EBulletType GetBT() { return m_bulletType; }
    public void SetBS(float spd) { m_bulletSpd = spd; }
    public void SetRange(float range) { m_bulletRange = range; }
    public void SetParentMgr(BulletMgr mgr) { m_parentMgr = mgr; }
    public void SetMoveVec(Vector3 v) { m_moveVec = v; }
    public void SetStartPos(Vector3 v) { transform.position = v; }
    public void SetTarget(Transform target) { m_targetCharacter = target; }
    public void SetAsterisk(Vector3 p) {
        m_asteriskPos = new Vector3(p.x - 9.8f, -3.9f, p.z - 1.6f); m_isAsterisk = true;
        obj_asterisk = Instantiate(Resources.Load("Prefabs/PointerAsterisk"), m_asteriskPos, Quaternion.identity) as GameObject;
        
    }
}
