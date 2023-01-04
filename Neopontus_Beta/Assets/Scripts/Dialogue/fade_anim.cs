using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class fade_anim : MonoBehaviour
{
    public SpriteRenderer black;
    static public fade_anim instance;

    private Color color;

    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);

    public Animator animator;

    /*private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);         // 맵을 이동해도 오브젝트를 파괴시키지 않는다
        }
        else
        {
            Destroy(this.gameObject);
        }
    }*/
    void Update()
    {
        
        
    }
    public void FadeOut(float _speed = 0.02f)
    {

        StartCoroutine(FadeOutCoroutine(_speed));
    }
    IEnumerator FadeOutCoroutine(float _speed)
    {
        color = black.color;
        
        while (color.a < 1f)
        {
            color.a += _speed;
            black.color = color;
            yield return waitTime;
        }

    }
    public void FadeIn(float _speed = 0.02f)
    {
        StartCoroutine(FadeInCoroutine(_speed));
    }
    IEnumerator FadeInCoroutine(float _speed)
    {
        color = black.color;

        while (color.a > 0f)
        {
            color.a -= _speed;
            black.color = color;
            yield return waitTime;
        }
        Debug.Log("FadeIn 애니메이션 실행ㅇ중!!");
    }

}
