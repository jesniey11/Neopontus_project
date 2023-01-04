using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class notLoadScene : MonoBehaviour
{
    public GameObject go;
    public bool activated;
    //public Test theTest;
    //public bool _flag;
    public dibar thedi;
    

    // Start is called before the first frame update
    void Start()
    {
        //theTest = FindObjectOfType<Test>();
        thedi = FindObjectOfType<dibar>();

    }

    // Update is called once per frame
    void Update()
    {

        if (thedi.activated)//if (theTest.sprite_DialogueBox.gameObject)
        {
            go.SetActive(false);
        }

        else
        {

            go.SetActive(true);

        }


    }
}
