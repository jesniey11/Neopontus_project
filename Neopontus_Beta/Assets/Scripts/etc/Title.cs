using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{

	private FadeScript theFade; // 페이드
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
		theFade.FadeOut(); // 페이드 맞는 메소드로 바꿔주기
						//theAudio.Play(click_sound);
		yield return new WaitForSeconds(2f);
		Color color = thePlayer.GetComponent<SpriteRenderer>().color;
		color.a = 1f;
		thePlayer.GetComponent<SpriteRenderer>().color = color;
		//thePlayer.currentMapName = "main";
		thePlayer.currentSceneName = "1D_Main_Scene";

		theGM.CngSc();//fade는 타이틀 구현의 13분 참조!!


		SceneManager.LoadScene("1D_Main_Scene");
		thePlayer.Move();
	}

	public void ExitGame()
	{
		//theAudio.Play(click_sound);
		Application.Quit();
	}
}