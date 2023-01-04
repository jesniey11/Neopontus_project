using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{

    static public DatabaseManager instance;
    // Start is called before the first frame update

    
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
    }

    public string[] var_name;
    public float[] var;

    public string[] switch_name;
    public bool[] switches;

    public List<Item> itemList = new List<Item>();

    private PlayerStat thePlayerStat;
    public GameObject prefabs_Floating_Text;
    public GameObject parent;

    private void FloatText(int number, string color)
    {
        Vector3 vector = thePlayerStat.transform.position;
        vector.y += 60;

        GameObject clone = Instantiate(prefabs_Floating_Text, vector, Quaternion.Euler(Vector3.zero));
        clone.GetComponent<FloatingText>().text.text = number.ToString();
        if (color == "RED")
            clone.GetComponent<FloatingText>().text.color = Color.red;
        else if (color == "BLUE")
            clone.GetComponent<FloatingText>().text.color = Color.blue;
        clone.GetComponent<FloatingText>().text.fontSize = 25;
        clone.transform.SetParent(parent.transform);
    }

    public void UseItem(int _itemID)
    {
        switch (_itemID) {
            case 10001:
                if (thePlayerStat.hp >= thePlayerStat.currentHP + 5)
                    thePlayerStat.currentHP += 5;
                else
                    thePlayerStat.currentHP = thePlayerStat.hp;
                FloatText(50, "GREEN");
                break;
            case 10002:
                if (thePlayerStat.mp >= thePlayerStat.currentMP + 5)
                    thePlayerStat.currentMP += 5;
                else
                    thePlayerStat.currentMP = thePlayerStat.mp;
                FloatText(50, "BLUE");
                break;

        }
    }

    void Start()
    {
        thePlayerStat = FindObjectOfType<PlayerStat>();
        itemList.Add(new Item(10001, "파란색 음료수", "체력을 __ 회복시켜주는 음료수", Item.ItemType.Use));
        itemList.Add(new Item(20001, "장비 예시템", "장비 예시템, 때리면 아프다", Item.ItemType.Equip, 3));
        itemList.Add(new Item(20101, "장비 예시템", "장비 예시템, 이게 머야..", Item.ItemType.Equip, 3));
        itemList.Add(new Item(30001, "퀘스트 예시템", "퀘스트 예시템, 퀘스트에 쓰인다", Item.ItemType.Quest));
        itemList.Add(new Item(40001, "기타 예시템", "기타 예시템, 모르겟닿ㅎ", Item.ItemType.ETC));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
