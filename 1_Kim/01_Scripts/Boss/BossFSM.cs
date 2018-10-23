using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EEnemyState
{
    Idle,
    Attack,
    Move,
}

public class BossFSM : MonoBehaviour
{
    public EEnemyState currentState = EEnemyState.Idle;
    Animation anim;
    public float lerpTime = 0.3f;
    public float moveSpeed = 0.1f;

    Dictionary<string, float> animNameList = new Dictionary<string, float>();

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animation>();

        foreach (AnimationState aniState in anim)
        {
            animNameList.Add(aniState.clip.name, aniState.clip.length);

            if (aniState.clip.name == "RunL")
            {
                aniState.speed = 1f;
            }
            if (aniState.clip.name == "RangeAtkL")
            {
                aniState.speed = 3f;
            }
        }

        setState(EEnemyState.Idle);
    }

    public void setState(EEnemyState state, object param1 = null)
    {
        if (currentState == state)
        {
            return;
        }

        switch (state)
        {
            case EEnemyState.Idle: StartCoroutine("idle"); break;
            case EEnemyState.Attack: StartCoroutine("attack"); break;
            case EEnemyState.Move: StartCoroutine("move"); break;
        }

        currentState = state;
    }

    IEnumerator idle()
    {
        anim.CrossFade("IdleL", 0.2f);
        yield return null;

        while (currentState == EEnemyState.Idle)
        {

            yield return null;
        }
    }
    IEnumerator attack()
    {
        float elapsedTime = 0f;
        anim.CrossFade("RangeAtkL", 0.2f);
        yield return null;

        while (currentState == EEnemyState.Attack)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime > animNameList["RangeAtkL"] / 3f)
            {
                setState(EEnemyState.Idle);
            }

            yield return null;
        }

    }
    IEnumerator move()
    {
        anim.CrossFade("RunL", lerpTime);

        yield return null;

        while (currentState == EEnemyState.Move)
        {
            yield return null;
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
