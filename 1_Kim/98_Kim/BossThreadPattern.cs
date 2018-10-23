using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossThreadPattern : MonoBehaviour {

    private Transform m_targetCharacter;
    private bool m_isAttack;
    private bool m_isCreate;

	// Use this for initialization
	void Start () {
        m_targetCharacter = GameObject.FindGameObjectWithTag("Player").transform;
        m_isAttack = false;
        m_isCreate = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (m_isCreate)
        {
            StartCoroutine(BossDirection());
            if (!m_isAttack)
            {
                StartCoroutine(Pattern());
                m_isAttack = true;
            }
        }
    }
    
    IEnumerator Pattern()
    {
            for (int j = 0; j < 10; j++)
            {
                SfxManager.instance.playShot();
                Bullet bullet = BulletMgr.Instance.PopBullet();

                Vector3 moveVec = (transform.forward - new Vector3(0, transform.forward.y - m_targetCharacter.forward.y, 0));

                bullet.Init(new Vector3(transform.position.x, m_targetCharacter.position.y * 1.5f, transform.position.z) + transform.forward * 0.3f,
                    moveVec, 8f, 20f, EBulletType.LINEAR, EBulletShooter.ENEMY);

                yield return new WaitForSeconds(1.0f);
            }
        m_isAttack = false;
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

    public void setPattern(bool b)
    {
        m_isCreate = b;
    }
}
