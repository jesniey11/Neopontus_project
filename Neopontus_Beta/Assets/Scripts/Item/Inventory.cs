using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    private ShowEquip theSE;

    //public bool notInv = false;
    //public Inventory theInv;
    private DatabaseManager theDatabase;
    
    private PlayerController theOrder;
    /*�κ��丮 �����
    private AudioManager theAudio;
    public string key_sound;
    public string enter-sound;
    public string cancel_sound;
    public string open_sound;
    public string beep_sound;
    */
    private OkOrCancel theOOC;



    private InventorySlot[] slots; //�κ��丮 ���Ե�

    private List<Item> inventoryItemList; // �÷��̾ ������ ������ ����Ʈ
    private List<Item> inventoryTabList; // ������ �ۿ� ���� �ٸ��� ������ ������ ����Ʈ

    public Text Description_Text; //�ο�����
    public string[] tabDescription; //�� �ο�����

    public Transform tf; // slot �θ�ü

    public GameObject go;//�κ��丮 Ȱ��ȭ ��Ȱ��ȭ   
    public GameObject[] selectedTabImages;
    public GameObject go_OOC; //������ Ȱ��ȭ ��Ȱ��ȭ
    public GameObject prefab_Floating_Text;


    private int selectedItem; //���õ� ������
    private int selectedTab; //���õ� ��

    private bool activated; //�κ��丮 Ȱ��ȭ�� true
    private bool tabActivated;// �� Ȱ��ȭ�� true
    private bool itemActivated; //������ Ȱ��ȭ�� true
    private bool stopKeyInput; //��� Ű�Է� ����(�Һ��� �� ���ƴϿ� ���ý� Ű�Է� ����)
    private bool preventExec; //�ߺ����� ����

    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);

    private Equipment theEquip;

    public List<Item> SaveItem()
    {
        return inventoryItemList;
    }
    public void LoadItem(List<Item> _itemList)
    {
        inventoryItemList = _itemList;
    }

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        go.SetActive(false);
        Description_Text.text = " ";
        //theAudio = FindObjectOfType<AudioManager>();
        theOrder = FindObjectOfType<PlayerController>();

        theDatabase = FindObjectOfType<DatabaseManager>();

        theOOC = FindObjectOfType<OkOrCancel>();
        theEquip = FindObjectOfType<Equipment>();

        theSE = FindObjectOfType<ShowEquip>();

        inventoryItemList = new List<Item>();
        inventoryTabList = new List<Item>();
        slots = tf.GetComponentsInChildren<InventorySlot>();

    }
    public void EquipToInventory(Item _item)
    {
        inventoryItemList.Add(_item);
    }


    public void GetAnItem(int _itemID, int _count = 1)
    {
        for (int i = 0; i < theDatabase.itemList.Count; i++){ //�����ͺ��̽� ������ �˻�
            if (_itemID == theDatabase.itemList[i].itemID) { //������ ���̽��� ������ �߰�

                var clone = Instantiate(prefab_Floating_Text, PlayerController.instance.transform.position, Quaternion.Euler(Vector3.zero));
                clone.GetComponent<FloatingText>().text.text = theDatabase.itemList[i].itemName + " " + _count + "�� ȹ�� +";
                clone.transform.SetParent(this.transform);

                for (int j = 0; j < inventoryItemList.Count; j++) {//����ǰ�� ���� �������� �ִ��� �˻�
                    if (inventoryItemList[j].itemID ==  _itemID) { // ����ǰ�� ���� �������� �ִ�-> ������ ����
                        if (inventoryItemList[j].itemType == Item.ItemType.Use)
                        {
                            inventoryItemList[j].itemCount += _count;
                            //return;
                        }
                        else {
                            inventoryItemList.Add(theDatabase.itemList[j]);
                            return;
                            //break;
                        }
                    }
                }
                inventoryItemList.Add(theDatabase.itemList[i]); //����ǰ�� �ش� �������� ���� ��� �߰�
                inventoryItemList[inventoryItemList.Count - 1].itemCount = _count;
                return;
            }
        }
        Debug.LogError("�����ͺ��̽��� �ش� ID���� ���� �������� �������� �ʽ��ϴ�."); //�����ͺ��̽��� ItemID����
    }

    public void ShowTab()//�� Ȱ��ȭ
    {
        RemoveSlot();
        SelectedTab();
    }
    public void RemoveSlot() //�κ��丮 ���� �ʱ�ȭ
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].RemoveItem();
            slots[i].gameObject.SetActive(false);
        }
    }
    public void SelectedTab() //���õ� ���� �����ϰ� �ٸ� ��� ���� �÷� ���İ� 0���� ����
    {
        StopAllCoroutines();
        Color color = selectedTabImages[selectedTab].GetComponent<Image>().color;
        color.a = 0f;
        for (int i = 0; i < selectedTabImages.Length; i++)
        {
            selectedTabImages[i].GetComponent<Image>().color = color;
        }
        Description_Text.text = tabDescription[selectedTab];
        StartCoroutine(SelectedTabEffectCoroutine());
    }

    IEnumerator SelectedTabEffectCoroutine() // ���õ� �� ��¦�� ȿ��
    {
        while (tabActivated) {
            Color color = selectedTabImages[0].GetComponent<Image>().color;
            while (color.a < 0.5f)
            {
                color.a += 0.03f;
                selectedTabImages[selectedTab].GetComponent<Image>().color = color;
                yield return waitTime;
            }
            while (color.a > 0f)
            {
                color.a -= 0.03f;
                selectedTabImages[selectedTab].GetComponent<Image>().color = color;
                yield return waitTime;
            }

            yield return new WaitForSeconds(0.3f);
        } 
    }

    public void ShowItem()
    {
        inventoryTabList.Clear();
        RemoveSlot();
        selectedItem = 0;

        switch (selectedTab) //�ǿ� ���� ������ �з�, �װ��� �κ��丮 �� ����Ʈ�� �߰�
        {
            case 0:
                for (int i = 0; i < inventoryItemList.Count; i++)
                {
                    if (Item.ItemType.Use == inventoryItemList[i].itemType)
                        inventoryTabList.Add(inventoryItemList[i]);
                }
                break;
            case 1:
                for (int i = 0; i < inventoryItemList.Count; i++)
                {
                    if (Item.ItemType.Equip == inventoryItemList[i].itemType)
                        inventoryTabList.Add(inventoryItemList[i]);
                }
                break;
            case 2:
                for (int i = 0; i < inventoryItemList.Count; i++)
                {
                    if (Item.ItemType.Quest == inventoryItemList[i].itemType)
                        inventoryTabList.Add(inventoryItemList[i]);
                }
                break;
            case 3:
                for (int i = 0; i < inventoryItemList.Count; i++)
                {
                    if (Item.ItemType.ETC == inventoryItemList[i].itemType)
                        inventoryTabList.Add(inventoryItemList[i]);
                }
                break;
                
        }

        for (int i = 0; i < inventoryTabList.Count; i++) //�κ��丮 �� ����Ʈ�� ������ �κ��丮 ���Կ� �߰�
        {
            slots[i].gameObject.SetActive(true);
            slots[i].Additem(inventoryTabList[i]);
        }

        SelectedItem();
    }//������ Ȱ��ȭ
    public void SelectedItem() //���� ���������� �ٸ� �� �÷� ���İ� 0����
    {
        StopAllCoroutines();
        if (inventoryTabList.Count > 0) {
            Color color = slots[0].selected_Item.GetComponent<Image>().color;
            color.a = 0f;
            for (int i = 0; i < inventoryTabList.Count; i++)
                slots[i].selected_Item.GetComponent<Image>().color = color;
            
            Description_Text.text = inventoryTabList[selectedItem].itemDescription;
            StartCoroutine(SelectedItemEffectCoroutine());
        }
        else
            Description_Text.text = "�ش� Ÿ���� �������� �����ϰ� ���� ����";
    }

    IEnumerator SelectedItemEffectCoroutine() //���õ� ������ ��¦��
    {

        while (itemActivated)
        {
            Color color = slots[0].GetComponent<Image>().color;
            while (color.a < 0.5f)
            {
                color.a += 0.3f;
                slots[selectedItem].selected_Item.GetComponent<Image>().color = color;
                //yield return waitTime;
                yield return new WaitForSeconds(0.08f);
            }
            while (color.a > 0f)
            {
                color.a -= 0.3f;
                slots[selectedItem].selected_Item.GetComponent<Image>().color = color;
                //yield return waitTime;
                yield return new WaitForSeconds(0.08f);

            }

            yield return new WaitForSeconds(0.3f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!stopKeyInput)
        {
            if (Input.GetKeyDown(KeyCode.I)) //IŰ ������ �κ��丮 Ȱ��ȭ
            {
                theOrder.NotMove();
                activated = !activated;

                if (activated)
                {
                    //theAudio.Play(open_sound);
                    theOrder.NotMove();
                    go.SetActive(true);
                    selectedTab = 0;
                    tabActivated = true;
                    itemActivated = false;
                    ShowTab();
                    

                }
                else
                {
                    //theAudio.Play(cancel_sound);
                    StopAllCoroutines();
                    go.SetActive(false);
                    tabActivated = false;
                    itemActivated = false;
                    Description_Text.text = " ";

                    theOrder.Move();
                    
                }
            }

            if (activated)
            {
                if (tabActivated)
                {
                    if (Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        if (selectedTab < selectedTabImages.Length - 1)
                            selectedTab++;
                        else
                            selectedTab = 0;
                        //theAudio.Play(key_sound);
                        SelectedTab();
                    }
                    else if (Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        if (selectedTab > 0)
                            selectedTab--;
                        else
                            selectedTab = selectedTabImages.Length - 1;
                        //theAudio.Play(key_sound);
                        SelectedTab();
                    }
                    else if (Input.GetKeyDown(KeyCode.Z))
                    {
                        //theAudio.Play(enter_sound);
                        Color color = selectedTabImages[selectedTab].GetComponent<Image>().color;
                        color.a = 0.25f;
                        selectedTabImages[selectedTab].GetComponent<Image>().color = color;
                        itemActivated = true;
                        tabActivated = false;
                        preventExec = true;
                        ShowItem();
                    }

                } //��Ȱ��ȭ�� Ű�Է� ó��

                else if (itemActivated) //������ Ȱ��ȭ�� Ű�Է� ó��
                {
                    if (inventoryTabList.Count > 0)
                    {
                        if (Input.GetKeyDown(KeyCode.DownArrow))
                        {
                            if (selectedItem < inventoryTabList.Count - 1)
                                selectedItem += 2;
                            else
                                selectedItem %= 2;
                            //theAudio.Play(key_sound);
                            SelectedItem();
                        }
                        else if (Input.GetKeyDown(KeyCode.UpArrow))
                        {
                            if (selectedItem > 1)
                                selectedItem -= 2;
                            else
                                selectedItem = inventoryTabList.Count - 1 - selectedItem;
                            //theAudio.Play(key_sound);
                            SelectedItem();
                        }
                        else if (Input.GetKeyDown(KeyCode.RightArrow))
                        {
                            if (selectedItem < inventoryTabList.Count - 1)
                                selectedItem++;
                            else
                                selectedItem--;
                            //theAudio.Play(key_sound);
                            SelectedItem();
                        }
                        else if (Input.GetKeyDown(KeyCode.LeftArrow))
                        {
                            if (selectedItem > 0)
                            {
                                selectedItem--;
                                //Debug.Log("0���� ��Ŭ��");
                            }

                            else
                            {
                                selectedItem = inventoryTabList.Count - 1;
                                //Debug.Log("0���� ������");
                            }

                            //theAudio.Play(key_sound);
                            SelectedItem();
                        }
                        else if (Input.GetKeyDown(KeyCode.Z) && !preventExec)
                        {
                            if (selectedTab == 0)
                            {
                                //theAudio.Play(enter_sound);


                                stopKeyInput = true;//��...��....����....
                                StartCoroutine(OOCCoroutine("���", "���"));
                                //������ ���ǰų� ���� ������ ȣ��


                            }
                            else if (selectedTab == 1)
                            {
                                //�������
                                //theAudio.Play(enter_sound);
                                stopKeyInput = true;
                                StartCoroutine(OOCCoroutine("����", "���"));
                            }
                            else if (selectedTab == 2)
                            {
                                //�������
                                //theAudio.Play(enter_sound);
                                
                                    stopKeyInput = true;
                                    StartCoroutine(OOCCoroutine("�ٱ�", "����"));
                                
                            }
                            else 
                            {//������ ���
                             //theAudio.Play(beep_sound);
                            }

                        }
                    }

                    
                    if (Input.GetKeyDown(KeyCode.X)) 
                    {
                        //theAudio.Play(cancel_sound);
                        StopAllCoroutines();
                        itemActivated = false;
                        tabActivated = true;
                        ShowTab();
                    }
                }

                if (Input.GetKeyUp(KeyCode.Z)) //�ߺ����� ����
                    preventExec = false;
            }
        }
    }
    IEnumerator OOCCoroutine(string _up, string _down)
    {

        go_OOC.SetActive(true);
        theOOC.ShowTwoChoice(_up, _down);
        yield return new WaitUntil(() => !theOOC.activated);
        if (theOOC.GetResult())
        {
            for (int i = 0; i < inventoryItemList.Count; i++)
            {
                if (inventoryItemList[i].itemID == inventoryItemList[selectedItem].itemID)
                {
                    if (selectedTab == 0)
                    {
                        theDatabase.UseItem(inventoryItemList[i].itemID);

                        if (inventoryItemList[i].itemCount > 1)
                            inventoryItemList[i].itemCount--;
                        else
                            inventoryItemList.RemoveAt(i);

                        // theAudio.Play();//�۸Դ� ���带 �־��ָ� ����

                        ShowItem();
                        break;
                    }
                    else if (selectedTab == 1)
                    {
                        theEquip.EquipItem(inventoryItemList[i]);
                        inventoryItemList.RemoveAt(i);
                        ShowItem();
                        break;
                    }
                    else if (selectedTab == 2)
                    {
                        
                        if(theSE.OnTrigger == true)
                        {
                            theEquip.EquipItem(inventoryItemList[i]);
                            inventoryItemList.RemoveAt(i);
                            ShowItem();
                            break;
                        }
                        else if (theSE.OnTrigger == false)
                        {
                            //�ټ��� ����
                            var clone = Instantiate(prefab_Floating_Text, PlayerController.instance.transform.position, Quaternion.Euler(Vector3.zero));
                            clone.GetComponent<FloatingText>().text.text = "�� �� �ִ� ����� ����;;";
                            break;
                        }
                        
                    }
                }
            }
        }

        stopKeyInput = false;
        go_OOC.SetActive(false);
    }
}
