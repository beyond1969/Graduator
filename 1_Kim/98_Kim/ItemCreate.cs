using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCreate : MonoBehaviour {

    private Vector3 item_initPos = new Vector3(0, 1, 13);
    GameObject boss;

    void Start()
    {
        boss = GameObject.FindGameObjectWithTag("Boss");
    }

    // stage 번호를 입력으로 받아서 각 스테이지별로 다른 아이템이 출현하게 할 예정
	public void StageClear(int stageNum)
    {
        CameraMovement cameraMovement = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMovement>();
        GameObject boos = Instantiate(GameObject.FindGameObjectWithTag("Boss")) as GameObject;
        boos.SetActive(false);
        cameraMovement.setCamTargetBoss(boos.transform);
        Destroy(boss);
        Vector3 itemPos1 = item_initPos + new Vector3(0, 1, 0);
        Vector3 itemPos2 = item_initPos + new Vector3(3, 1, 0);
        Vector3 itemPos3 = item_initPos + new Vector3(-3, 1, 0);
        GameObject item1 = null;
        GameObject item2 = null;
        GameObject item3 = null;
        // 여기서 아이템 분화 - start
        if (stageNum == 1)
        {
            item1 = Instantiate(Resources.Load("Prefabs/Calculator"), itemPos1, Quaternion.identity) as GameObject;
            item1.AddComponent<ItemStatus>();
            item1.GetComponent<ItemStatus>().setItemKind(1);
            item1.GetComponent<ItemStatus>().setValue(10.0f);
            item1.GetComponent<ItemStatus>().setObjectName("Calculator");
            item2 = Instantiate(Resources.Load("Prefabs/Eraser"), itemPos2, Quaternion.identity) as GameObject;
            item2.AddComponent<ItemStatus>();
            item2.GetComponent<ItemStatus>().setItemKind(2);
            item2.GetComponent<ItemStatus>().setValue(1.0f);
            item2.GetComponent<ItemStatus>().setObjectName("Eraser");
            item3 = Instantiate(Resources.Load("Prefabs/Tape dispenser"), itemPos3, Quaternion.identity) as GameObject;
            item3.AddComponent<ItemStatus>();
            item3.GetComponent<ItemStatus>().setItemKind(3);
            item3.GetComponent<ItemStatus>().setValue(1.0f);
            item3.GetComponent<ItemStatus>().setObjectName("Tape dispenser");
        }
        // 여기서 아이템 분화 - end
        item1.transform.localScale = new Vector3(5, 5, 5);
        item1.AddComponent<Rigidbody>().useGravity = false;
        item1.AddComponent<BoxCollider>().isTrigger = true;
        item1.GetComponent<BoxCollider>().size = new Vector3(0.2f, 0.2f, 0.2f);
        item1.GetComponent<BoxCollider>().center = new Vector3(0.0f, 0.0f, 0.0f);
        item1.GetComponent<ItemStatus>().setStageNum(stageNum);
        item2.transform.localScale = new Vector3(5, 5, 5);
        item2.AddComponent<Rigidbody>().useGravity = false;
        item2.AddComponent<BoxCollider>().isTrigger = true;
        item2.GetComponent<BoxCollider>().size = new Vector3(0.2f, 0.2f, 0.2f);
        item2.GetComponent<BoxCollider>().center = new Vector3(0.0f, 0.0f, 0.0f);
        item2.GetComponent<ItemStatus>().setStageNum(stageNum);
        item3.transform.localScale = new Vector3(5, 5, 5);
        item3.AddComponent<Rigidbody>().useGravity = false;
        item3.AddComponent<BoxCollider>().isTrigger = true;
        item3.GetComponent<BoxCollider>().size = new Vector3(0.2f, 0.2f, 0.2f);
        item3.GetComponent<BoxCollider>().center = new Vector3(0.0f, 0.0f, 0.0f);
        item3.GetComponent<ItemStatus>().setStageNum(stageNum);
        item1.tag = "Item";
        item2.tag = "Item";
        item3.tag = "Item";
        StartCoroutine(RoatationTimer(item1, 0.01f, 1.0f));
        StartCoroutine(RoatationTimer(item2, 0.01f, 1.0f));
        StartCoroutine(RoatationTimer(item3, 0.01f, 1.0f));
    }

    IEnumerator RoatationTimer(GameObject obj, float time, float angle)
    {
        while (true)
        {
            yield return new WaitForSeconds(time);
            obj.transform.Rotate(0, angle, 0);
        }
    }
}
