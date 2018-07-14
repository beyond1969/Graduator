using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    private Transform camTargetCharacter;
    private Transform camTargetBoss;
    private Transform camXForm;

    //카메라 위치조정용 벡터

    private Vector3 viewPos;
    private Vector3 centerPos;
    private Vector3 newCamPos;
    Vector3 CamToBoss;
    Vector3 CamToCharacter;
    Vector3 CamToCenter;

    //고정 카메라 비율값
    private float f_sqrt2 = Mathf.Sqrt(2);
    private float dotMinimum = Mathf.Cos(Mathf.PI / 180f * 25f);
    private float f_tan60 = Mathf.Tan(Mathf.PI / 4f);   //60도는 fov값

    // Use this for initialization
    void Start () {
        camTargetCharacter = GameObject.FindGameObjectWithTag("Player").transform;
        camTargetBoss = GameObject.FindGameObjectWithTag("Boss").transform;
        camXForm = GetComponent<Transform>();
        CameraFix();
    }
	
	// Update is called once per frame
	void Update () {
        CameraFix();
    }

    void CameraFix()
    {
        centerPos = (camTargetCharacter.position + camTargetBoss.position) / 2f;

        float startHeight = 5.5f;
        float camX, camZ;
        while (true)
        {
            camX = camZ = startHeight / f_tan60 * f_sqrt2 / 2f;
            camZ *= -1f;

            viewPos.x = camX; viewPos.y = startHeight; viewPos.z = camZ;

            newCamPos = centerPos + viewPos;

            CamToBoss = (camTargetBoss.position - newCamPos).normalized;
            CamToCharacter = (camTargetCharacter.position - newCamPos).normalized;
            CamToCenter = (centerPos - newCamPos).normalized;

            float dotBC = Vector3.Dot(CamToBoss, CamToCenter);
            float dotCC = Vector3.Dot(CamToCharacter, CamToCenter);

            //cos는 각이 작아질수록 값이 커지므로 카메라의 foward벡터와
            //카메라의 origin으로부터 두 오브젝트가 일정 각도 안에 있도록 하기위해
            if (dotBC > dotMinimum && dotCC > dotMinimum)
            {
                camXForm.position = newCamPos;

                Quaternion rotQuat = Quaternion.LookRotation(CamToCenter);
                camXForm.rotation = rotQuat;
                break;
            }

            startHeight += 0.05f;
        }
    }
}
