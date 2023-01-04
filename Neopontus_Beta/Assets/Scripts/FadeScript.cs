using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FadeScript : MonoBehaviour
{
    public SpriteRenderer black;
    static public FadeScript instance;

    private Color color;

    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);

    private void Awake()
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
    }

    //수정전
    /*public Image Panel;                                 // 페이드로 사용 될 이미지 오브젝트
    public float fadeOverTime = 2.0f;                   // 페이드 지속 시간 (NextStage에서 씬 지연에도 사용)
    public float waitFade = 1.5f;                       // 페이드 인/아웃 전환 대기 시간 (검은 화면)

    private float timeAlpha = 0.0f;                     // 시간에 따라 변화하는 알파값 (0: 투명, 1: 불투명)

    static public FadeScript instance;

    private void Awake()
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
    }

    public void Fade()                                  // 외부에서 호출할 함수
    {
        StartCoroutine(FadeFlow());                     // 코루틴 시작
    }

    IEnumerator FadeFlow()
    {
        Panel.gameObject.SetActive(true);               // 이미지 오브젝트 활성화

        timeAlpha = 0.0f;
        Color alpha = Panel.color;

        while (alpha.a < 1.0f)                          // 페이드 아웃
        {
            timeAlpha += Time.deltaTime / fadeOverTime;
            alpha.a = Mathf.Lerp(0, 1, timeAlpha);
            Panel.color = alpha;
            yield return null;
        }

        timeAlpha = 0f;
        yield return new WaitForSeconds(waitFade);      // 페이드 아웃/인 전환 대기 (검은 화면)

        while (alpha.a > 0.0f)                          // 페이드 인
        {
            timeAlpha += Time.deltaTime / fadeOverTime;
            alpha.a = Mathf.Lerp(1, 0, timeAlpha);
            Panel.color = alpha;
            yield return null;
        }
        yield return new WaitForSeconds(10f);
        Panel.gameObject.SetActive(false);               // 이미지 오브젝트 비활성화
        yield return new WaitForSeconds(10f);
    }*/

}
