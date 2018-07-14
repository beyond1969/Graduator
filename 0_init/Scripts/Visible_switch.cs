using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Visible_switch : MonoBehaviour {
    private GameObject obj;
    private RawImage img;
    private bool visible;
    private float timer;
    private float waitingTime;

    // Use this for initialization
    void Start()
    {
        obj = GameObject.Find("Game_start");
        img = obj.GetComponent<RawImage>();
        visible = false;
        timer = 0.0f;
        waitingTime = 0.8f;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > waitingTime)
        {
            img.enabled = visible;
            visible = !visible;
            timer = 0.0f;
        }
    }
}
