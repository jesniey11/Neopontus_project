using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;

public class SaveLoad : MonoBehaviour
{
    [System.Serializable]
    public class Data
    {
        public float playerX;
        public float playerY;
        public float playerZ;

        public int playerLv;
        public int playerHP;
        public int playerMP;

        public int playerCurrentHP;
        public int playerCurrentMP;
        public int playerCurrentEXP;

        public int playerATK;
        public int playerDEF;

        public List<int> playerItemInventory;
        public List<int> playerItemInventoryCount;
        public List<int> playerEuqipItem;

        public string mapName;
        public string sceneName;

        public List<bool> swList;
        public List<string> swNameList;
        public List<string> varNameList;
        public List<float> varNumberList;

    }

    private PlayerController thePlayer;
    private PlayerStat thePlayerStat;
    private DatabaseManager theDatabase;
    private Inventory theInven;
    private Equipment theEquip;

    public Data data;

    private Vector3 vector;

    private FadeScript theFade;

    public void CallSave()
    {
        theDatabase = FindObjectOfType<DatabaseManager>();
        thePlayer = FindObjectOfType<PlayerController>();
        thePlayerStat = FindObjectOfType<PlayerStat>();
        theEquip = FindObjectOfType<Equipment>();
        theInven = FindObjectOfType<Inventory>();
        theFade = FindObjectOfType<FadeScript>();

        theFade.FadeOut();

        data.playerX = thePlayer.transform.position.x;
        data.playerX = thePlayer.transform.position.y;
        data.playerX = thePlayer.transform.position.z;

        data.playerLv = thePlayerStat.character_Lv;
        data.playerHP = thePlayerStat.hp;
        data.playerMP = thePlayerStat.mp;
        data.playerCurrentHP = thePlayerStat.currentHP;
        data.playerCurrentMP = thePlayerStat.currentMP;
        data.playerCurrentEXP = thePlayerStat.currentEXP;
        data.playerATK = thePlayerStat.atk;
        data.playerDEF = thePlayerStat.def;

        data.mapName = thePlayer.currentMapName;
        data.sceneName = thePlayer.currentSceneName;

        Debug.Log("���� ������ ����");

        for (int i = 0; i < theDatabase.var_name.Length; i++)
        {
            data.varNameList.Add(theDatabase.var_name[i]);
            data.varNumberList.Add(theDatabase.var[i]);
        }
        for (int i = 0; i < theDatabase.switch_name.Length; i++)
        {
            data.swNameList.Add(theDatabase.switch_name[i]);
            data.swList.Add(theDatabase.switches[i]);
        }

        List<Item> itemList = theInven.SaveItem();

        for (int i = 0; i < itemList.Count; i++)
        {
            Debug.Log("�κ��丮�� ������ ���� �Ϸ� : " + itemList[i].itemID);
            data.playerItemInventory.Add(itemList[i].itemID);
            data.playerItemInventoryCount.Add(itemList[i].itemCount);
        }

        for (int i = 0; i < theEquip.equipItemList.Length; i++)
        {
            data.playerEuqipItem.Add(theEquip.equipItemList[i].itemID);
            Debug.Log("���� ������ ���� �Ϸ� : " + theEquip.equipItemList[i].itemID);
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.dataPath + "/SaveFile.dat");

        bf.Serialize(file, data);
        file.Close();

        Debug.Log(Application.dataPath + "�� ��ġ�� ����");

        theFade.FadeIn();
    }



    public void CallLoad()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.dataPath + "/SaveFile.dat", FileMode.Open);

        if (file != null && file.Length > 0)
        {
            data = (Data)bf.Deserialize(file);

            theDatabase = FindObjectOfType<DatabaseManager>();
            thePlayer = FindObjectOfType<PlayerController>();
            thePlayerStat = FindObjectOfType<PlayerStat>();
            theEquip = FindObjectOfType<Equipment>();
            theInven = FindObjectOfType<Inventory>();

            thePlayer.currentMapName = data.mapName;
            thePlayer.currentSceneName = data.sceneName;

            vector.Set(data.playerX, data.playerY, data.playerZ);
            thePlayer.transform.position = vector;

            thePlayerStat.character_Lv = data.playerLv;
            thePlayerStat.hp = data.playerHP;
            thePlayerStat.mp = data.playerMP;
            thePlayerStat.currentHP = data.playerCurrentHP;
            thePlayerStat.currentMP = data.playerCurrentMP;
            thePlayerStat.currentEXP = data.playerCurrentEXP;
            thePlayerStat.atk = data.playerATK;
            thePlayerStat.def = data.playerDEF;

            theDatabase.var = data.varNumberList.ToArray();
            theDatabase.var_name = data.varNameList.ToArray();
            theDatabase.switches = data.swList.ToArray();
            theDatabase.switch_name = data.swNameList.ToArray();

            for (int i = 0; i < theEquip.equipItemList.Length; i++)
            {
                for (int x = 0; x < theDatabase.itemList.Count; x++)
                {
                    if (data.playerEuqipItem[i] == theDatabase.itemList[x].itemID)
                    {
                        theEquip.equipItemList[i] = theDatabase.itemList[x];
                        Debug.Log("���� ������ �ε� : " + theEquip.equipItemList[i].itemID);
                        break;
                    }
                }
            }

            List<Item> itemList = new List<Item>();

            for (int i = 0; i < data.playerItemInventory.Count; i++)
            {
                for (int x = 0; x < theDatabase.itemList.Count; x++)
                {
                    if (data.playerItemInventory[i] == theDatabase.itemList[x].itemID)
                    {
                        itemList.Add(theDatabase.itemList[x]);
                        Debug.Log("�κ� ������ �ε� : " + theDatabase.itemList[x].itemID);
                        break;
                    }
                }
            }

            for (int i = 0; i < data.playerItemInventoryCount.Count; i++)
            {
                itemList[i].itemCount = data.playerItemInventoryCount[i];

            }
            theInven.LoadItem(itemList);
            //theEquip.ShowTxT();

            GameManager theGM = FindObjectOfType<GameManager>();
            theGM.LoadStart();

            SceneManager.LoadScene(data.sceneName);
        }
        else
        {
            Debug.Log("����� ���̺� ���� ����");        
        }
        file.Close();

        //Debug.Log(Application.dataPath + "�� ��ġ�� ����");
    }
    IEnumerator WaitCoroutine()
    {
        yield return new WaitForSeconds(2f);

        GameManager theGM = FindObjectOfType<GameManager>();
        theGM.LoadStart();

        SceneManager.LoadScene(data.sceneName);
    }
}
