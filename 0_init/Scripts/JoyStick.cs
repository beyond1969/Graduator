using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler {
    private Image bgImg;
    private Image joystickImg;
    private Vector3 inputVec;

	// Use this for initialization
	void Start () {
        bgImg = GetComponent<Image>();
        joystickImg = transform.GetChild(0).GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public virtual void OnDrag(PointerEventData ped)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(bgImg.rectTransform,
            ped.position, ped.pressEventCamera, out pos))
        {
            pos.x = (pos.x / bgImg.rectTransform.sizeDelta.x);
            pos.y = (pos.y / bgImg.rectTransform.sizeDelta.y);

            inputVec = new Vector3(pos.x * 2 + 1, pos.y * 2 - 1, 0);
            inputVec = (inputVec.magnitude > 1.0f) ? inputVec.normalized : inputVec;

            joystickImg.rectTransform.anchoredPosition =
                new Vector3(inputVec.x * bgImg.rectTransform.sizeDelta.x / 3f,
                inputVec.y * bgImg.rectTransform.sizeDelta.y / 3f);
        }
    }

    public virtual void OnPointerDown(PointerEventData ped)
    {
        OnDrag(ped);
    }

    public virtual void OnPointerUp(PointerEventData ped)
    {
        inputVec = Vector3.zero;
        joystickImg.rectTransform.anchoredPosition = Vector3.zero;
    }

    public float GetHorizontalValue()
    {
        return inputVec.x;
    }

    public float GetVerticalValue()
    {
        return inputVec.y;
    }
}
