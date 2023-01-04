using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anim_play : MonoBehaviour
{

    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public GameObject go;

    private void Animplay()
    {
        //go.SetActive(true);
        animator.Play("Anim_Wake");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
