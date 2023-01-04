using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CngSc_Park : MonoBehaviour
{
    /*public void SceneChange_P() {
        SceneManager.LoadScene("Scene_Potal");
    }*/
    // Start is called before the first frame update
    void Start()
    {

    }

    public void SceneChange_P()
    {
       SceneManager.LoadScene("1D_Park_Scene");
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene("Scene_Potal");
        }*/
    }
}