using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartSceneWithBoss : MonoBehaviour {
    GameObject player;
    public string str_bossName;        // 툴에서 설정
    public float bossHp;              // 툴에서 설정
    Slider bossHealthBar;
    Text bossName;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        CharacterCtrlWithKM km = player.GetComponent<CharacterCtrlWithKM>();
        km.camUpdate = true;
        bossHealthBar = GameObject.Find("BossHealthBar").GetComponent<Slider>();
        bossName = GameObject.Find("BossName").GetComponent<Text>();
        bossName.text = str_bossName;
        bossHealthBar.maxValue = bossHp;
        bossHealthBar.value = bossHp;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
