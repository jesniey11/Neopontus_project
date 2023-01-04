using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{

	private FadeScript theFade; // ���̵�
								//private AudioManager theAudio;

	//public string click_sound;	
	public string thisScene;

	private PlayerController thePlayer;

	private GameManager theGM;
	public GameObject hpBar;

	void Start()
	{
		theFade = FindObjectOfType<FadeScript>();
		//theAudio = FindObjectOfType<AudioManager>();
		thePlayer = FindObjectOfType<PlayerController>();
		theGM = FindObjectOfType<GameManager>();
		thePlayer.currentSceneName = "Title";
	}

	public void StartGame()
	{
		thePlayer.NotMove();
		StartCoroutine(GameStartCoroutine());
	}
	IEnumerator GameStartCoroutine()
	{
		//hpBar.SetActive(true);
		theFade.FadeOut(); // ���̵� �´� �޼ҵ�� �ٲ��ֱ�
						//theAudio.Play(click_sound);
		yield return new WaitForSeconds(2f);
		Color color = thePlayer.GetComponent<SpriteRenderer>().color;
		color.a = 1f;
		thePlayer.GetComponent<SpriteRenderer>().color = color;
		//thePlayer.currentMapName = "main";
		thePlayer.currentSceneName = "1D_Main_Scene";

		theGM.CngSc();//fade�� Ÿ��Ʋ ������ 13�� ����!!


		SceneManager.LoadScene("1D_Main_Scene");
		thePlayer.Move();
	}

	public void ExitGame()
	{
		//theAudio.Play(click_sound);
		Application.Quit();
	}
}