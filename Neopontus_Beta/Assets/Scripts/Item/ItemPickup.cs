using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{

    public int itemID;
    public int _count;
    
    //public string pickUpSound;

    private bool OnTrigger = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnTrigger = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        OnTrigger = false;
    }

    private void Update()
    {
        if (OnTrigger && Input.GetKeyDown(KeyCode.Z))
        {
            //AudioManager.instance.Play(pickUpSound);
            Inventory.instance.GetAnItem(itemID, _count);
            Destroy(this.gameObject);
        }
    }


    /*private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKeyDown(KeyCode.Z))
        { //Input.GetKeyDown(KeyCode.Z) (Input.GetMouseButtonDown(0))
            //AudioManager.instance.Play(pickUpSound);  //오디오 매니저 없음!!(제작 안함)
            Inventory.instance.GetAnItem(itemID, _count);
            Destroy(this.gameObject);

        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }*/
}
