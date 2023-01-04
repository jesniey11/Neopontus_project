using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DD_L : MonoBehaviour
{
    static public DD_L instance;
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
