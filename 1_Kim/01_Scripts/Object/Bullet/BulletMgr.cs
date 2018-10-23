using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMgr : SingletonMgr<BulletMgr> {

    public GameObject m_BaseBullet; //bullet prefab, inspector 창에서 등록가능

    protected Queue<Bullet> m_BulletQ;

    private int m_totalBulletNum;
    
    protected override bool Init()
    {
        m_BaseBullet = GameObject.Find("BaseSphereBullet");

        m_BulletQ = new Queue<Bullet>();

        m_totalBulletNum = 500;

        for(int i = 0; i < m_totalBulletNum; i++)
        {
            Bullet bullet = CreateAndPushBullet();
        }

        return true;
    }

    public Bullet PopBullet()
    {
        Bullet bullet;

        if(m_BulletQ.Count <= 1)
        {
            for(int i = 0; i < m_totalBulletNum / 2; i++)
            {
                CreateAndPushBullet();
            }
            m_totalBulletNum += m_totalBulletNum / 2;
        }

        bullet = m_BulletQ.Dequeue();
        bullet.gameObject.SetActive(true);

        return bullet;
    }

    public void PushBullet(Bullet bullet)
    {
        if (bullet == null) return;

        bullet.gameObject.SetActive(false);

        m_BulletQ.Enqueue(bullet);
    }

    protected Bullet CreateAndPushBullet()
    {
        GameObject obj = Instantiate(m_BaseBullet, Vector3.zero,
            Quaternion.identity) as GameObject;

        obj.transform.parent = gameObject.transform;
        Bullet newBullet = obj.AddComponent<Bullet>();

        newBullet.Init();
        newBullet.SetParentMgr(this);

        obj.SetActive(false);

        m_BulletQ.Enqueue(newBullet);

        return newBullet;
    }
}
