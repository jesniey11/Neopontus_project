using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtEnemy : MonoBehaviour
{
    public GameObject prefabs_Floating_Text;
    public GameObject parent;
    public GameObject effect;


    //public string atkSound;

    private PlayerStat thePlayerStat;

    void Start()
    {
        thePlayerStat = FindObjectOfType<PlayerStat>();
    }
   

    private void OnTrigger2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy") //태그 새로 생성할지 원래잇는걸로 바꿀지 생각하기
        {
            int dmg = collision.gameObject.GetComponent<EnemyShooting>().OnHit(thePlayerStat.atk);
            //AudioManager.instance.Play(atkSound);
            //currentHP -= dmg;

            Vector3 vector = this.transform.position;

            Instantiate(effect, vector, Quaternion.Euler(Vector3.zero));

            vector.y += 60;

            GameObject clone = Instantiate(prefabs_Floating_Text, vector, Quaternion.Euler(Vector3.zero));
            clone.GetComponent<FloatingText>().text.text = dmg.ToString();
            clone.GetComponent<FloatingText>().text.color = Color.white;
            clone.GetComponent<FloatingText>().text.fontSize = 25;
            clone.transform.SetParent(parent.transform);

        }
    }

    void Update()
    {

    }
}
