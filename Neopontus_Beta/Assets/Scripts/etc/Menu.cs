using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

	public static Menu instance;

	public string startPoint; // ��ġ����

	public GameObject go; // �޴� ��ü Ȱ��ȭ ��Ȱ��ȭ
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
		theOrder = FindObjectOfType<PlayerController>(); //���� �߰�
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
		theOrder.Move(); //���� �߰�
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
		theFade.FadeOut(); // ���̵� �´� �޼ҵ�� �ٲ��ֱ�
						   //theAudio.Play(click_sound);
		yield return new WaitForSeconds(2f);
		Color color = theOrder.GetComponent<SpriteRenderer>().color;
		color.a = 1f;
		theOrder.GetComponent<SpriteRenderer>().color = color;
		//thePlayer.currentMapName = "main";
		theOrder.currentSceneName = "Title";

		theGM.CngSc();//fade�� Ÿ��Ʋ ������ 13�� ����!!

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
			
			if (Input.GetKeyDown(KeyCode.Escape)) //ESCŰ
			{
				
				activated = !activated;

				if (activated)
				{
					theOrder.NotMove(); //���� �߰�
					go.SetActive(true);
					//theAudio.Play(call_sound);
				}
				else
				{

					go.SetActive(false);
					//theAudio.Play(cancel_sound);
					theOrder.Move(); //���� �߰�
				}//������� �ؼ� ��ũ��Ʈ�� �ش� ĵ������ �߰�! go : menu(object), theOrder�� ordermanager..?
			}
		}
		else go.SetActive(false);
	}
}
