using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscnotTrue : MonoBehaviour
{
    public GameObject go;
    public bool activated;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape)) //ESCŰ
        {
            activated = !activated;

            if (activated)
            {
                //theOrder.NotMove(); //���� �߰�
                go.SetActive(false);
                //theAudio.Play(call_sound);
            }
            else
            {

                go.SetActive(true);
                //theAudio.Play(cancel_sound);
                //theOrder.Move(); //���� �߰�
            }
        }
        

    }
}
