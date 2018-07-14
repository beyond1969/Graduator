using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxManager : MonoBehaviour {
    public AudioClip getHurt;
    public AudioClip shot;
    AudioSource myAudio;
    public static SfxManager instance;


    void Awake()
    {
        if (SfxManager.instance == null)
            SfxManager.instance = this;
    }

	// Use this for initialization
	void Start () {
        myAudio = GetComponent<AudioSource>();
	}

    public void playHurt()
    {
        myAudio.PlayOneShot(getHurt);
    }
    public void playShot()
    {
        myAudio.PlayOneShot(shot);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
