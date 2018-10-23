using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterHealth : MonoBehaviour {

    public Slider healthBar;
    public const float defaultDamage = 10.0f;
    public bool isDamaged;

	// Use this for initialization
	void Start () {
        isDamaged = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if (!isDamaged)
        {
            if (other.gameObject.name == "BaseSphereBullet(Clone)" &&
                healthBar.value > 0)
            {
                healthBar.value -= defaultDamage;
            }
            else if(healthBar.value <= 0)
            {
                // Game Over
            }
            isDamaged = !isDamaged;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (isDamaged)
        {
            isDamaged = !isDamaged;
        }
    }
}
