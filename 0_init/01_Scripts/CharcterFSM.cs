using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EUnitState
{
    Idle,
    Run,
    Attack1,
    Attack2,
    Attack3,
    RangeAtk,
}

public class CharcterFSM : MonoBehaviour {

    public EUnitState currentState = EUnitState.Idle;

    Animation anim;
    public float lerpTime = 0.3f;
    public float moveSpeed = 5f;

    Dictionary<string, float> animNameList = new Dictionary<string, float>();

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animation>();

        foreach (AnimationState aniState in anim)
        {
            //Debug.Log(aniState.clip.name + " , " + aniState.clip.length);

            animNameList.Add(aniState.clip.name, aniState.clip.length);

            if (aniState.clip.name == "RunL")
            {
                aniState.speed = 2f;
            }
            if (aniState.clip.name == "Atk1L")
            {
                aniState.speed = 2.5f;
            }
            if (aniState.clip.name == "Atk2L")
            {
                aniState.speed = 2f;
            }
            if (aniState.clip.name == "Atk3L")
            {
                aniState.speed = 2f;
            }
            if (aniState.clip.name == "RangeAtkL")
            {
                aniState.speed = 3f;
            }
        }

        setState(EUnitState.Idle);
    }

    public void setState(EUnitState state, object param1 = null)
    {
        if (currentState == state)
        {
            return;
        }

        switch (state)
        {
            case EUnitState.Idle: StartCoroutine("idle"); break;
            case EUnitState.Attack1: StartCoroutine("attack1"); break;
            case EUnitState.Attack2: StartCoroutine("attack2"); break;
            case EUnitState.Attack3: StartCoroutine("attack3"); break;
            case EUnitState.RangeAtk: StartCoroutine("rangeAtk"); break;
            case EUnitState.Run: StartCoroutine("run"); break;
            default:
                break;
        }

        currentState = state;
    }

    IEnumerator idle()
    {
        Debug.Log(">>>>> OnEnter : idle ");
        anim.CrossFade("IdleL", 0.2f);
        yield return null;

        while (currentState == EUnitState.Idle)
        {

            yield return null;
        }

        // onExit
        Debug.Log("OnExit : idle ");
    }

    IEnumerator attack1()
    {
        Debug.Log(">>>>> OnEnter : atk1 ");
        float elapsedTime = 0f;
        anim.CrossFade("Atk1L", 0.2f);
        yield return null;

        while (currentState == EUnitState.Attack1)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime > animNameList["Atk1L"] / 2.5f)
            {
                setState(EUnitState.Idle);
            }

            yield return null;
        }

        // onExit
        Debug.Log("OnExit : attack ");
    }

    IEnumerator attack2()
    {
        Debug.Log(">>>>> OnEnter : atk2 ");
        float elapsedTime = 0f;
        anim.CrossFade("Atk2L", 0.2f);
        yield return null;

        while (currentState == EUnitState.Attack1)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime > animNameList["Atk2L"])
            {
                setState(EUnitState.Idle);
            }

            yield return null;
        }

        // onExit
        Debug.Log("OnExit : attack ");
    }

    IEnumerator attack3()
    {
        Debug.Log(">>>>> OnEnter : atk3 ");
        float elapsedTime = 0f;
        anim.CrossFade("Atk3L", 0.2f);
        yield return null;

        while (currentState == EUnitState.Attack1)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime > animNameList["Atk3L"])
            {
                setState(EUnitState.Idle);
            }

            yield return null;
        }

        // onExit
        Debug.Log("OnExit : attack ");
    }

    IEnumerator rangeAtk()
    {
        Debug.Log(">>>>> OnEnter : rangeAtk ");
        float elapsedTime = 0f;
        anim.CrossFade("RangeAtkL", 0.2f);
        yield return null;

        while (currentState == EUnitState.RangeAtk)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime > animNameList["RangeAtkL"] / 3f)
            {
                setState(EUnitState.Idle);
            }

            yield return null;
        }

        // onExit
        Debug.Log("OnExit : idle ");
    }

    IEnumerator run()
    {
        Debug.Log(">>>>> OnEnter : run ");

        anim.CrossFade("RunL", lerpTime);
        
        yield return null;

        while (currentState == EUnitState.Run)
        {
            yield return null;
        }

        // onExit
        Debug.Log("OnExit : run ");
    }
}
