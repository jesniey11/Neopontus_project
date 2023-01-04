using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_1114 : MonoBehaviour
{
    public dibar thediBar;
    // Start is called before the first frame update
    void Start()
    {
        thediBar = FindObjectOfType<dibar>();
    }

    // Update is called once per frame
    void Update()
    {
        thediBar.ReVal_OFF();
    }
}
