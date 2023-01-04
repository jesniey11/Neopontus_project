using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    //private Bound[] bounds;
    private PlayerController thePlayer;
    private CameraFollow theCamera;

    private Menu theMenu;
    private Test theTest; //Test thetest
    //private fade_anim thefa;
    private Camera cam;
    private Ref theref;


    private FadeScript theFade;

    private fade_anim thefa;

    public void LoadStart()
    {
        StartCoroutine(LoadWaitCoroutine());
    }

    public void CngSc() 
    {
        
            StartCoroutine(CngScWaitCoroutine());
      
    }
    public void AnimSc() //페이드가 애니메이션인것
    {
        
        StartCoroutine(AnimWaitCoroutine());
    }

    IEnumerator LoadWaitCoroutine()
    {
        //theFade.FadeOut();
        yield return new WaitForSeconds(0.5f);

        thePlayer = FindObjectOfType<PlayerController>();
        theCamera = FindObjectOfType<CameraFollow>();

        theMenu = FindObjectOfType<Menu>();
        theTest = FindObjectOfType<Test>(); //?test..?
        cam = FindObjectOfType<Camera>(); // ?? camerfollow ??

        theFade = FindObjectOfType<FadeScript>();

        //thefa = FindObjectOfType<fade_anim>();
        theref = FindObjectOfType<Ref>();

        //theCamera.tragetType = GameObject.Find("Player");

        Color color = thePlayer.GetComponent<SpriteRenderer>().color;
        color.a = 1f;
        thePlayer.GetComponent<SpriteRenderer>().color = color;

        theMenu.GetComponent<Canvas>().worldCamera = cam;
        theTest.GetComponent<Canvas>().worldCamera = cam;

        //thefa.GetComponent<Canvas>().worldCamera = cam;

        theref.GetComponent<Canvas>().worldCamera = cam;

        /*for (int i = 0; i < bounds.Length; i++)
        {
            if (bounds[i].boudName == thePlayer.currnetMapName)
            {
                bounds[i].SetBound();
                break;
            }
        }
        */

        theFade.FadeIn();

    }

    
    IEnumerator CngScWaitCoroutine()
    {
        //theFade.FadeOut();
        
        yield return new WaitForSeconds(0.5f);

        thePlayer = FindObjectOfType<PlayerController>();
        theCamera = FindObjectOfType<CameraFollow>();

        theMenu = FindObjectOfType<Menu>();
        //theTest = FindObjectOfType<Test>(); //?test..?
        cam = FindObjectOfType<Camera>(); // ?? camerfollow ??

        theFade = FindObjectOfType<FadeScript>();

        //thefa = FindObjectOfType<fade_anim>();
        theref = FindObjectOfType<Ref>();

        //theCamera.tragetType = GameObject.Find("Player");

        Color color = thePlayer.GetComponent<SpriteRenderer>().color;
        color.a = 1f;
        thePlayer.GetComponent<SpriteRenderer>().color = color;

        

        theMenu.GetComponent<Canvas>().worldCamera = cam;
        //theTest.GetComponent<Canvas>().worldCamera = cam;

        //thefa.GetComponent<Canvas>().worldCamera = cam;

        theref.GetComponent<Canvas>().worldCamera = cam;

        /*for (int i = 0; i < bounds.Length; i++)
        {
            if (bounds[i].boudName == thePlayer.currnetMapName)
            {
                bounds[i].SetBound();
                break;
            }
        }
        */

        theFade.FadeIn();

        

    }
    IEnumerator AnimWaitCoroutine()
    {
        //theFade.FadeOut();
        yield return new WaitForSeconds(0.5f);

        thePlayer = FindObjectOfType<PlayerController>();
        theCamera = FindObjectOfType<CameraFollow>();

        theMenu = FindObjectOfType<Menu>();
        theTest = FindObjectOfType<Test>(); //?test..?
        cam = FindObjectOfType<Camera>(); // ?? camerfollow ??

        theFade = FindObjectOfType<FadeScript>();

        thefa = FindObjectOfType<fade_anim>();
        theref = FindObjectOfType<Ref>();

        //theCamera.tragetType = GameObject.Find("Player");

        Color color = thePlayer.GetComponent<SpriteRenderer>().color;
        color.a = 1f;
        thePlayer.GetComponent<SpriteRenderer>().color = color;



        theMenu.GetComponent<Canvas>().worldCamera = cam;
        theTest.GetComponent<Canvas>().worldCamera = cam;

        //thefa.GetComponent<Canvas>().worldCamera = cam;

        theref.GetComponent<Canvas>().worldCamera = cam;

        /*for (int i = 0; i < bounds.Length; i++)
        {
            if (bounds[i].boudName == thePlayer.currnetMapName)
            {
                bounds[i].SetBound();
                break;
            }
        }
        */
        
        //thefa.FadeIn();

    }
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
