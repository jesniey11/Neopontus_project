using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStat : MonoBehaviour
{
    public GameObject prefabs_Floating_Text;
    public GameObject parent;
    public Slider hpSlider;
    public Slider mpSlider;

    public static PlayerStat instance;

    public int character_Lv;
    public int[] needEXP;
    public int currentEXP;

    public int hp;
    public int currentHP;
    public int mp;
    public int currentMP;

    public int recover_hp; //1초에 회복되는 양
    public int recover_mp;

    public int atk;
    public int def;

    public Animator animator;

    //public string dmgSound;

    void Start()
    {
     
        instance = this;
        currentHP = hp;
        currentMP = mp;
    }
    
    public void Hit(int _enemyAtk)
    {
        int dmg;
        if (def >= _enemyAtk)
            dmg = 1;
        else
            dmg = _enemyAtk - def;

        currentHP -= dmg;

        if (currentHP <= 0)
            Debug.Log("HP < 0,gameover");
        //AudioManager.instance.Play(dmgSound)

        Vector3 vector = this.transform.position;
        vector.y += 60;

        GameObject clone = Instantiate(prefabs_Floating_Text, vector, Quaternion.Euler(Vector3.zero));
        clone.GetComponent<FloatingText>().text.text = dmg.ToString();
        clone.GetComponent<FloatingText>().text.color = Color.red;
        clone.GetComponent<FloatingText>().text.fontSize = 25;
        clone.transform.SetParent(parent.transform);

        StopAllCoroutines();
        StartCoroutine(HitCoroutine());
    }
    
    IEnumerator HitCoroutine()
    {
        Color color = GetComponent<SpriteRenderer>().color;
        color.a = 0;
        GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(0.1f);
        color.a = 1;
        GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(0.1f);
        color.a = 0;
        GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(0.1f);
        color.a = 1;
        GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(0.1f);
        color.a = 0;
        GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(0.1f);
        color.a = 1;
         GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = color;

    //animator. ~~~~ ("Player_DMG_A");
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "EnemyBullet") //if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "EnemyBullet")
        {
            animator.Play("Player_DMG_A", 0, 0.0f);
            //animator.SetTrigger("DMG_A");
            int dmg = collision.gameObject.GetComponent<Bullet>().dmg;
            currentHP -= dmg;

            if (currentHP <= 0)
                Debug.Log("HP < 0,gameover");
            //AudioManager.instance.Play(dmgSound)

            Vector3 vector = this.transform.position;
            vector.y += 60;

            GameObject clone = Instantiate(prefabs_Floating_Text, vector, Quaternion.Euler(Vector3.zero));
            clone.GetComponent<FloatingText>().text.text = dmg.ToString();
            clone.GetComponent<FloatingText>().text.color = Color.red;
            clone.GetComponent<FloatingText>().text.fontSize = 25;
            clone.transform.SetParent(parent.transform);

            StopAllCoroutines();
            StartCoroutine(HitCoroutine());
        }
    }
    void Update()
    {
        hpSlider.maxValue = hp;
        mpSlider.maxValue = mp;

        hpSlider.value = currentHP;
        mpSlider.value = currentMP;

        if (currentEXP >= needEXP[character_Lv])
        {
            character_Lv++;
            hp += (character_Lv * 2);
            mp += (character_Lv + 2);

            currentHP = hp;
            currentMP = mp;
            atk++;
            def++;
        }

    }
}