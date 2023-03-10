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
    /*인벤토리 오디오
    private AudioManager theAudio;
    public string key_sound;
    public string enter-sound;
    public string cancel_sound;
    public string open_sound;
    public string beep_sound;
    */
    private OkOrCancel theOOC;



    private InventorySlot[] slots; //인벤토리 슬롯들

    private List<Item> inventoryItemList; // 플레이어가 소지한 아이템 리스트
    private List<Item> inventoryTabList; // 선택한 템에 따라 다르게 보여질 아이템 리스트

    public Text Description_Text; //부연설명
    public string[] tabDescription; //탭 부연설명

    public Transform tf; // slot 부모객체

    public GameObject go;//인벤토리 활성화 비활성화   
    public GameObject[] selectedTabImages;
    public GameObject go_OOC; //선택지 활성화 비활성화
    public GameObject prefab_Floating_Text;


    private int selectedItem; //선택된 아이템
    private int selectedTab; //선택된 탭

    private bool activated; //인벤토리 활성화시 true
    private bool tabActivated;// 탭 활성화시 true
    private bool itemActivated; //아이템 활성화시 true
    private bool stopKeyInput; //상시 키입력 제한(소비할 때 에아니오 선택시 키입력 방지)
    private bool preventExec; //중복실행 제한

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
        for (int i = 0; i < theDatabase.itemList.Count; i++){ //데이터베이스 아이템 검색
            if (_itemID == theDatabase.itemList[i].itemID) { //데이테 베이스에 아이템 발견

                var clone = Instantiate(prefab_Floating_Text, PlayerController.instance.transform.position, Quaternion.Euler(Vector3.zero));
                clone.GetComponent<FloatingText>().text.text = theDatabase.itemList[i].itemName + " " + _count + "개 획득 +";
                clone.transform.SetParent(this.transform);

                for (int j = 0; j < inventoryItemList.Count; j++) {//소지품에 같은 아이템이 있는지 검색
                    if (inventoryItemList[j].itemID ==  _itemID) { // 소지품에 같은 아이템이 있다-> 개수만 증가
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
                inventoryItemList.Add(theDatabase.itemList[i]); //소지품에 해당 아이템이 없을 경우 추가
                inventoryItemList[inventoryItemList.Count - 1].itemCount = _count;
                return;
            }
        }
        Debug.LogError("데이터베이스에 해당 ID값을 가진 아이템이 존재하지 않습니다."); //데이터베이스에 ItemID없음
    }

    public void ShowTab()//탭 활성화
    {
        RemoveSlot();
        SelectedTab();
    }
    public void RemoveSlot() //인벤토리 슬롯 초기화
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].RemoveItem();
            slots[i].gameObject.SetActive(false);
        }
    }
    public void SelectedTab() //선택된 탭을 제외하고 다른 모든 탭의 컬러 알파값 0으로 조정
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

    IEnumerator SelectedTabEffectCoroutine() // 선택된 탭 반짝임 효과
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

        switch (selectedTab) //탭에 따른 아이템 분류, 그것을 인벤토리 탭 리스트에 추가
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

        for (int i = 0; i < inventoryTabList.Count; i++) //인벤토리 탭 리스트의 내용을 인벤토리 슬롯에 추가
        {
            slots[i].gameObject.SetActive(true);
            slots[i].Additem(inventoryTabList[i]);
        }

        SelectedItem();
    }//아이템 활성화
    public void SelectedItem() //선택 아이템제외 다른 탭 컬러 알파값 0으로
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
            Description_Text.text = "해당 타입의 아이템을 소유하고 있지 않음";
    }

    IEnumerator SelectedItemEffectCoroutine() //선택된 아이템 반짝임
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
            if (Input.GetKeyDown(KeyCode.I)) //I키 누르면 인벤토리 활성화
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

                } //탭활성화시 키입력 처리

                else if (itemActivated) //아이템 활성화시 키입력 처리
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
                                //Debug.Log("0보다 ㄲ클때");
                            }

                            else
                            {
                                selectedItem = inventoryTabList.Count - 1;
                                //Debug.Log("0보다 작을때");
                            }

                            //theAudio.Play(key_sound);
                            SelectedItem();
                        }
                        else if (Input.GetKeyDown(KeyCode.Z) && !preventExec)
                        {
                            if (selectedTab == 0)
                            {
                                //theAudio.Play(enter_sound);


                                stopKeyInput = true;//음...음....으음....
                                StartCoroutine(OOCCoroutine("사용", "취소"));
                                //물약을 마실거냐 같은 선택지 호출


                            }
                            else if (selectedTab == 1)
                            {
                                //장비장착
                                //theAudio.Play(enter_sound);
                                stopKeyInput = true;
                                StartCoroutine(OOCCoroutine("장착", "취소"));
                            }
                            else if (selectedTab == 2)
                            {
                                //장비장착
                                //theAudio.Play(enter_sound);
                                
                                    stopKeyInput = true;
                                    StartCoroutine(OOCCoroutine("줄까", "말까"));
                                
                            }
                            else 
                            {//비프음 출력
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

                if (Input.GetKeyUp(KeyCode.Z)) //중복실행 방지
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

                        // theAudio.Play();//템먹는 사운드를 넣어주면 좋음

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
                            //줄수가 없다
                            var clone = Instantiate(prefab_Floating_Text, PlayerController.instance.transform.position, Quaternion.Euler(Vector3.zero));
                            clone.GetComponent<FloatingText>().text.text = "줄 수 있는 사람이 없다;;";
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
