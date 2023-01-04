using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dibar : MonoBehaviour
{
    public bool refnLS;

    public GameObject DiBar;
    public bool activated;
    //public Test theTest;
    //public bool a;


    // Start is called before the first frame update
    void Start()
    {
        //theTest = FindObjectOfType<Test>();

    }

    // Update is called once per frame
    void Update()
    {
       
    }
    public void ReVal_ON() {
        activated = false;
    }
    public void ReVal_OFF()
    {
        activated = true;
    }
}
