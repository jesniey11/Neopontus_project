using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShowEquip : MonoBehaviour
{
    public bool OnTrigger = false;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        OnTrigger = true;
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        OnTrigger = false;
    }
}
