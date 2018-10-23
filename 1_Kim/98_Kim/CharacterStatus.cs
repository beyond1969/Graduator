using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatus : MonoBehaviour {
    /*
     * 캐릭터의 스테이터스를 보관한다.
     * 딴곳에서 불러와서 쓰면 좋을듯
     */
    private float m_maxHp;
    private float m_damage;
    private float m_moveSpeed;

	// Use this for initialization
	void Start () {
        m_maxHp = 100.0f;
        m_damage = 30.0f;
        m_moveSpeed = 5.0f;
	}

    // getter
    public float getMaxHp()
    {
        return this.m_maxHp;
    }
    public float getDamage()
    {
        return this.m_damage;
    }
    public float getMoveSpeed()
    {
        return this.m_moveSpeed;
    }

    // setter
    public void setMaxHp(float maxHp)
    {
        this.m_maxHp = maxHp;
    }
    public void setDamage(float damage)
    {
        this.m_damage = damage;
    }
    public void setMoveSpeed(float moveSpeed)
    {
        this.m_moveSpeed = moveSpeed;
    }
}
