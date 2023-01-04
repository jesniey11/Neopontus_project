using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMain : MonoBehaviour
{
    static public PlayerMain instance;

    PlayerController playerCtrl;

    public AudioClip clip; //민지: 공격소리

    void Awake() { playerCtrl = GetComponent<PlayerController>(); }

    //포탈 이동 시 파괴 금지
    void Start()
    {
        if (instance == null) {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else{ 
            Destroy(this.gameObject);
        }
 
    }

    // Update is called once per frame
    void Update()
    {

        if (!playerCtrl.activeSts) { return; }

        float joyMv = Input.GetAxis("Horizontal");



        playerCtrl.ActionMove(joyMv);


        if (Input.GetButtonDown("Jump")) { playerCtrl.ActionJump(); }


        if (Input.GetKeyDown(KeyCode.Q))
        {
            playerCtrl.ActionAttack();

            SoundManager.instance.SFXPlay("attack", clip); //민지:공격소리
        }
    }
}
