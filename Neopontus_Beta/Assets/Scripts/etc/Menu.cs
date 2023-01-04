using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

	public static Menu instance;

	public string startPoint; // 위치지정

	public GameObject go; // 메뉴 전체 활성화 비활성화
						  //public AudioManager theAudio;
	public Title theTitle;

	public string call_sound;
	public string cancel_sound;

	public PlayerController theOrder;

	public GameObject[] gos;

	public bool activated;

	private GameManager theGM;
	private FadeScript theFade;

	private void Awake()
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

	void Start()
	{
		theOrder = FindObjectOfType<PlayerController>(); //임의 추가
		theGM = FindObjectOfType<GameManager>();
		theFade = FindObjectOfType<FadeScript>();
		theTitle = FindObjectOfType<Title>();


	}
	public void Exit()
	{
		Application.Quit();
	}

	public void Continue()
	{
		activated = false;
		go.SetActive(false);
		theOrder.Move(); //임의 추가
						 //theAudio.Play(cancel_sound);	
	}

	public void GoToTitle() // 25:30
	{
		theOrder.NotMove();
		for (int i = 0; i < gos.Length; i++)
		{
			Destroy(gos[i]);
		}
		StartCoroutine(GoToTitleCoroutine());
	}
	IEnumerator GoToTitleCoroutine()
	{
		//hpBar.SetActive(true);
		theFade.FadeOut(); // 페이드 맞는 메소드로 바꿔주기
						   //theAudio.Play(click_sound);
		yield return new WaitForSeconds(2f);
		Color color = theOrder.GetComponent<SpriteRenderer>().color;
		color.a = 1f;
		theOrder.GetComponent<SpriteRenderer>().color = color;
		//thePlayer.currentMapName = "main";
		theOrder.currentSceneName = "Title";

		theGM.CngSc();//fade는 타이틀 구현의 13분 참조!!

		/*if (startPoint == theOrder.currentSceneName)
		{
			theOrder.transform.position = this.transform.position;
		}*/
		SceneManager.LoadScene("Title");
	}

	void Update()
	{
		//Debug.Log(theOrder.currentSceneName);
		if (theOrder.currentSceneName != "Title") //if (theTitle.thisScene != "Title")
		{
			
			if (Input.GetKeyDown(KeyCode.Escape)) //ESC키
			{
				
				activated = !activated;

				if (activated)
				{
					theOrder.NotMove(); //임의 추가
					go.SetActive(true);
					//theAudio.Play(call_sound);
				}
				else
				{

					go.SetActive(false);
					//theAudio.Play(cancel_sound);
					theOrder.Move(); //임의 추가
				}//여기까지 해서 스크립트를 해당 캔버스에 추가! go : menu(object), theOrder에 ordermanager..?
			}
		}
		else go.SetActive(false);
	}
}
