using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cng_7MinE_7MaxE : MonoBehaviour
{
    public FadeScript theFade;
    private PlayerController thePlayer;
    private GameManager theGM;

    // Start is called before the first frame update
    void Start()
    {
        theGM = FindObjectOfType<GameManager>();
        theFade = FindObjectOfType<FadeScript>();
        thePlayer = FindObjectOfType<PlayerController>();
    }

    public void SceneChange_P()
    {
        StartCoroutine(CngscCoroutine());
    }
    IEnumerator CngscCoroutine()
    {
        //hpBar.SetActive(true);
        theFade.FadeOut(); // ���̵� �´� �޼ҵ�� �ٲ��ֱ�
                           //theAudio.Play(click_sound);
        yield return new WaitForSeconds(2f);
        Color color = thePlayer.GetComponent<SpriteRenderer>().color;
        color.a = 1f;
        thePlayer.GetComponent<SpriteRenderer>().color = color;
        //thePlayer.currentMapName = "main";
        thePlayer.currentSceneName = "7D_MaxE_Scene";

        theGM.CngSc();//fade�� Ÿ��Ʋ ������ 13�� ����!!


        SceneManager.LoadScene("7D_MaxE_Scene");
    }
   
    // Update is called once per frame
    void Update()
    {
        
    }
}