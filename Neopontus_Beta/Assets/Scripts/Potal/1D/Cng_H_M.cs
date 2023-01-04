using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Cng_H_M : MonoBehaviour
{

    public fade_anim thefa;
    
    public FadeScript theFade;
    private PlayerController thePlayer;
    private GameManager theGM;

    // Start is called before the first frame update
    void Start()
    {
        theGM = FindObjectOfType<GameManager>();
        theFade = FindObjectOfType<FadeScript>();
        thePlayer = FindObjectOfType<PlayerController>();

        thefa = FindObjectOfType<fade_anim>();
    }

    public void SceneChange_P()
    {
        StartCoroutine(CngscCoroutine());
    }
    IEnumerator CngscCoroutine()
    {
        
        thefa.FadeOut();
        //theFade.FadeOut(); // 페이드 맞는 메소드로 바꿔주기
                           //theAudio.Play(click_sound);
        yield return new WaitForSeconds(2f);
        Color color = thePlayer.GetComponent<SpriteRenderer>().color;
        color.a = 1f;
        thePlayer.GetComponent<SpriteRenderer>().color = color;
        //thePlayer.currentMapName = "main";
        thePlayer.currentSceneName = "1D_Main_Scene";

        theGM.AnimSc();//fade는 타이틀 구현의 13분 참조!!


        SceneManager.LoadScene("1D_Main_Scene");
    }
    // Update is called once per frame
    void Update()
    {

    }
}
