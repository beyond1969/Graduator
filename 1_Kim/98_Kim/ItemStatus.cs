using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemStatus : MonoBehaviour {
    /*
     * item_kind는 아이템 종류를 정수값으로 저장함
     * value 는 해당 아이템이 어느정도의 수치 상승을 가져다 주는지 저장함
     * item_kind의 값 :
     * 1 = 체력 최대치 증가
     * 2 = 대미지 증가
     * 3 = 이동 속도 증가
     */
    private int stageNum;
    private int item_kind;
    private float value;
    private string objectName;
    CharacterStatus characterStatus;
    GameObject[] items;

    void Start()
    {
        characterStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterStatus>();
        items = GameObject.FindGameObjectsWithTag("Item");
    }
	
    public int getItemKind()
    {
        return this.item_kind;
    }

    public float getValue()
    {
        return this.value;
    }

    public string getObjectName()
    {
        return this.objectName;
    }

    public void setItemKind(int kind)
    {
        this.item_kind = kind;
    }

    public void setValue(float v)
    {
        this.value = v;
    }

    public void setObjectName(string name)
    {
        this.objectName = name;
    }

    public void setStageNum(int stageNum)
    {
        this.stageNum = stageNum;
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player")
        {
            Texture2D icon = null;
            string path = "Items/" + objectName;
            icon = Resources.Load(path, typeof(Texture2D)) as Texture2D;
            Image item = null;
            if(stageNum == 1)
                item = GameObject.Find("Item1").GetComponent<Image>();
            else if (stageNum == 2)
                item = GameObject.Find("Item2").GetComponent<Image>();
            else if (stageNum == 3)
                item = GameObject.Find("Item3").GetComponent<Image>();

            if (this.item_kind == 1)
            {
                characterStatus.setMaxHp(characterStatus.getMaxHp() + this.value);
                Slider playerHp = GameObject.Find("HealthBar").GetComponent<Slider>();
                playerHp.maxValue = characterStatus.getMaxHp();
                playerHp.value = playerHp.maxValue;
                Text playertext = GameObject.Find("Canvas").transform.Find("HealthText").GetComponent<Text>();
                playertext.text = playerHp.value.ToString();
            }
            else if(this.item_kind == 2)
            {
                characterStatus.setDamage(characterStatus.getDamage() + this.value);
            }
            else if(this.item_kind == 3)
            {
                characterStatus.setMoveSpeed(characterStatus.getMoveSpeed() + this.value);
            }
            Sprite s_icon = Sprite.Create(icon, new Rect(0, 0, 128, 128), new Vector2(0, 0));
            //s_icon.name = "icon_item1";
            item.sprite = s_icon;
            item.color = new Color(1,1,1,1);
            // 아이템 획득 효과음 및 아이템 획득 관련 요소 추가
            foreach (GameObject obj in items)
            {
                Destroy(obj);
            }
            // 임시 스테이지 넘기기
            Application.LoadLevel("2_stage");
        }
    }
}
