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
            DontDestroyOnLoad(this.gameObject);         // ���� �̵��ص� ������Ʈ�� �ı���Ű�� �ʴ´�
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

    //������
    /*public Image Panel;                                 // ���̵�� ��� �� �̹��� ������Ʈ
    public float fadeOverTime = 2.0f;                   // ���̵� ���� �ð� (NextStage���� �� �������� ���)
    public float waitFade = 1.5f;                       // ���̵� ��/�ƿ� ��ȯ ��� �ð� (���� ȭ��)

    private float timeAlpha = 0.0f;                     // �ð��� ���� ��ȭ�ϴ� ���İ� (0: ����, 1: ������)

    static public FadeScript instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);         // ���� �̵��ص� ������Ʈ�� �ı���Ű�� �ʴ´�
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void Fade()                                  // �ܺο��� ȣ���� �Լ�
    {
        StartCoroutine(FadeFlow());                     // �ڷ�ƾ ����
    }

    IEnumerator FadeFlow()
    {
        Panel.gameObject.SetActive(true);               // �̹��� ������Ʈ Ȱ��ȭ

        timeAlpha = 0.0f;
        Color alpha = Panel.color;

        while (alpha.a < 1.0f)                          // ���̵� �ƿ�
        {
            timeAlpha += Time.deltaTime / fadeOverTime;
            alpha.a = Mathf.Lerp(0, 1, timeAlpha);
            Panel.color = alpha;
            yield return null;
        }

        timeAlpha = 0f;
        yield return new WaitForSeconds(waitFade);      // ���̵� �ƿ�/�� ��ȯ ��� (���� ȭ��)

        while (alpha.a > 0.0f)                          // ���̵� ��
        {
            timeAlpha += Time.deltaTime / fadeOverTime;
            alpha.a = Mathf.Lerp(1, 0, timeAlpha);
            Panel.color = alpha;
            yield return null;
        }
        yield return new WaitForSeconds(10f);
        Panel.gameObject.SetActive(false);               // �̹��� ������Ʈ ��Ȱ��ȭ
        yield return new WaitForSeconds(10f);
    }*/

}
