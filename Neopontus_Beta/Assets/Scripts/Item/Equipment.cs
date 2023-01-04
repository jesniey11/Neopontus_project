using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Equipment : MonoBehaviour
{
    private PlayerController theOrder;
    //private AudioManager the Audio;
    private PlayerStat thePlayerStat;
    private Inventory theInven;
    private OkOrCancel theOOC;

   // public bool notInv = false;

    //public string open_sound;
    //public string close_sound;

    private const int EQUIP1 = 0, EQUIP2 = 1, EQUIP3 = 2; //, SHILED = 1, AMULT =2, ...~~~ 

    public GameObject equipWeapon;
    public GameObject go;
    public GameObject go_OOC;

    public Text[] text; //stat
    public Image[] img_slots; //장비슬롯 아이콘
    public GameObject go_img;

    //public Image[] big_img_slot;

    public GameObject go_selected_Slot_UI; //  선택한 장비 슬롯 UI

    public Item[] equipItemList; //장착된 장비 리스트

    private int selectedSlot; //선택된 장비 슬롯

    public bool activated = false;
    private bool inputKey = true;

    void Start()
    {
        
        theInven = FindObjectOfType<Inventory>();
        theOrder = FindObjectOfType<PlayerController>();
        //theAudio = FindObjectOfType<AudioManager>();
        thePlayerStat = FindObjectOfType<PlayerStat>();
        theOOC = FindObjectOfType<OkOrCancel>();
    }

    //public void ShowTxT(){} 텍스트 사용 안하므로 함수 구현 안함. 궁금하면 Part33. 9분 참고

    public void EquipItem(Item _item)
    {
        string temp = _item.itemID.ToString();
        temp = temp.Substring(0, 3);
        switch (temp)
        {
            case "200": //무기
                EquipItemCheck(EQUIP1, _item);
                equipWeapon.SetActive(true);
                equipWeapon.GetComponent<SpriteRenderer>().sprite = _item.itemIcon;
                break;
            case "201": //방패
                EquipItemCheck(EQUIP2, _item);
                break;
            case "202": //아뮬렛
                EquipItemCheck(EQUIP3, _item);
                break;
        }
    }

    public void EquipItemCheck(int _count, Item _item)
    {
        if (equipItemList[_count].itemID == 0)
        {
            equipItemList[_count] = _item;
        }
        else
        {
            theInven.EquipToInventory(equipItemList[_count]);
            equipItemList[_count] = _item;
        }
        EquipEffect(_item);
    }

    public void SelectedSlot()
    {
        go_selected_Slot_UI.transform.position = img_slots[selectedSlot].transform.position;

    }

    public void ClearEquip()
    {
        Color color = img_slots[0].color;
        color.a = 0f;

        for (int i = 0; i < img_slots.Length; i++)
        {
            img_slots[i].sprite = null;
            img_slots[i].color = color;
            //big_img_slot[selectedSlot].sprite = null;
            //big_img_slot[selectedSlot].color = color;
        }
    }

    public void ShowEquip()
    {
        Color color = img_slots[0].color;
        color.a = 1f;

        for (int i = 0; i < img_slots.Length; i++)
        {
            if (equipItemList[i].itemID != 0)
            {
                img_slots[i].sprite = equipItemList[i].itemIcon;
                img_slots[i].color = color;
                //big_img_slot[selectedSlot].sprite = equipItemList[selectedSlot].itemIcon;
                //big_img_slot[selectedSlot].color = color;
            }
        }
    }

    private void EquipEffect(Item _item)
    {
        thePlayerStat.atk += _item.atk;
        thePlayerStat.def += _item.def;
        thePlayerStat.recover_hp += _item.recover_hp;
        thePlayerStat.recover_mp += _item.recover_mp;
    }
    private void TakeOffEffect(Item _item)
    {

        thePlayerStat.atk -= _item.atk;
        thePlayerStat.def -= _item.def;
        thePlayerStat.recover_hp -= _item.recover_hp;
        thePlayerStat.recover_mp -= _item.recover_mp;

    }

    void Update()
    {
        if (inputKey)
        {
            //theInven.NotInv();
            if (Input.GetKeyDown(KeyCode.E))
            {
                activated = !activated;
                //theInven.NotInv();

                if (activated)
                {
                    //theInven.NotInv();
                    theOrder.NotMove();
                    //theAudio.Play(open_sound);
                    go_img.SetActive(false);
                    go.SetActive(true);
                    selectedSlot = 0;
                    SelectedSlot();
                    ClearEquip();
                    ShowEquip();
                }
                else
                {
                    //theInven.Inv();
                    theOrder.Move();
                    //theAudio.Play(close_sound);
                    go_img.SetActive(true);
                    go.SetActive(false);
                    ClearEquip();
                    ShowEquip();

                }
            }

            if (activated)
            {
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    if (selectedSlot < img_slots.Length - 1)
                        selectedSlot++;
                    else
                        selectedSlot = 0;
                    //theAudio.Play(//sound);
                    SelectedSlot();

                }
                else if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    if (selectedSlot > 0)
                        selectedSlot--;
                    else
                        selectedSlot = img_slots.Length - 1;
                    //theAudio.Play(//sound);
                    SelectedSlot();

                }
                else if (Input.GetKeyDown(KeyCode.Z))
                {
                    if (equipItemList[selectedSlot].itemID != 0)
                    {
                        //theAudio.Play(//sound);
                        inputKey = false;

                        StartCoroutine(OOCCoroutine("벗기", "취소"));
                    }

                }
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

            theInven.EquipToInventory(equipItemList[selectedSlot]);
            TakeOffEffect(equipItemList[selectedSlot]);
            if (selectedSlot == EQUIP1)
                equipWeapon.SetActive(false);
            equipItemList[selectedSlot] = new Item(0, "", "", Item.ItemType.Equip);
            //theAudio.Play(takeoff_sound);
            ClearEquip();
            ShowEquip();
        }
        inputKey = true;
        go_OOC.SetActive(false);
    }

}
