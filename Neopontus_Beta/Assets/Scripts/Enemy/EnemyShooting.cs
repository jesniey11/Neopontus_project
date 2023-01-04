using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public string enemyName;
    public float speed;

    public int currentHP;
    public int hp;
    public int exp;

    public GameObject bulletObjA;

    SpriteRenderer spriteRenderer;
    Rigidbody2D rigid;
     public Sprite[] sprites;

    public float maxShotDelay;
    public float curShotDelay;

    //public GameObject bulletObjs;

    public static EnemyShooting instance;

    public PlayerController thePlayer; //인스턴스 추가
    public PlayerStat thePlayerStat;

    public int patternIndex;
    public int curPatternCount;
    public int[] maxPatternCount;
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        //rigid.AddForce(Vector2.up, ForceMode2D.Impulse);
        //rigid.velocity = Vector2.down *speed*0.3f; //지금은 종스크롤이나 횡스크롤로 바꿔줍시다

    }
    void Start()
    {
        currentHP = hp;
    }

    void Think()
    {
        patternIndex = patternIndex == 3 ? 0 : patternIndex + 1;
        curPatternCount = 0;

        switch (patternIndex) {
            case 0:
                FireForward();
                break;
            case 1:
                FireShot();
                break;
            case 2:
                FireArc();
                break;
            case 3:
                FireAround();
                break;

        }
    }
    void FireForward()
    {
        //앞으로 네알 발사
        GameObject bulletR = Instantiate(bulletObjA, transform.position + Vector3.right * 0.5f, transform.rotation);
        GameObject bulletL = Instantiate(bulletObjA, transform.position + Vector3.left * 0.5f, transform.rotation);

        Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();

        Vector3 dirVecR = thePlayer.transform.position - (transform.position + Vector3.right * 0.5f);
        Vector3 dirVecL = thePlayer.transform.position - (transform.position + Vector3.left * 0.5f);
        rigidR.AddForce(dirVecR.normalized * 10, ForceMode2D.Impulse);
        rigidL.AddForce(dirVecL.normalized * 10, ForceMode2D.Impulse);

        GameObject bulletRr = Instantiate(bulletObjA, transform.position + Vector3.right * 0.1f, transform.rotation);
        GameObject bulletLl = Instantiate(bulletObjA, transform.position + Vector3.left * 0.1f, transform.rotation);

        Rigidbody2D rigidRr = bulletRr.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidLl = bulletLl.GetComponent<Rigidbody2D>();

        Vector3 dirVecRr = thePlayer.transform.position - (transform.position + Vector3.right * 0.1f);
        Vector3 dirVecLl = thePlayer.transform.position - (transform.position + Vector3.left * 0.1f);
        rigidRr.AddForce(dirVecRr.normalized * 10, ForceMode2D.Impulse);
        rigidLl.AddForce(dirVecLl.normalized * 10, ForceMode2D.Impulse);

        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
            Invoke("FireForward", 2);
        else
            Invoke("Think", 3);
    }
    void FireShot() 
    {
        //플레이어 방향으로 샷건
        for (int index = 0; index < 5; index++)
        {
            GameObject bulletR = Instantiate(bulletObjA, transform.position + Vector3.right * 0.3f, transform.rotation);

            Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();


            Vector2 dirVecR = thePlayer.transform.position - (transform.position + Vector3.right * 0.3f);
            Vector2 ranVec = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(0f, 2f));
            dirVecR += ranVec;
            rigidR.AddForce(dirVecR.normalized * 15, ForceMode2D.Impulse);
        }

        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
            Invoke("FireShot", 3.5f);
        else
            Invoke("Think", 3);
    }
    void FireArc() 
    {
        //부채모양발사
        GameObject bulletR = Instantiate(bulletObjA, transform.position + Vector3.right * 0.3f, transform.rotation);
        bulletR.transform.position = transform.position;
        bulletR.transform.rotation = Quaternion.identity;

        Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();

        Vector2 dirVecR = new Vector2(Mathf.Cos(Mathf.PI*10*curPatternCount/ maxPatternCount[patternIndex]), -1);
        rigidR.AddForce(dirVecR.normalized * 10, ForceMode2D.Impulse);

        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
            Invoke("FireArc", 0.15f);
        else
            Invoke("Think", 3);
    }
    void FireAround() 
    {
        //원형발사
        int roundNumA = 10;
        for (int index = 0; index < roundNumA; index++)
        {
            GameObject bulletR = Instantiate(bulletObjA, transform.position + Vector3.right * 0.3f, transform.rotation);
            bulletR.transform.position = transform.position;
            bulletR.transform.rotation = Quaternion.identity;

            Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();

            Vector2 dirVecR = new Vector2(Mathf.Cos(Mathf.PI * 2 * index /roundNumA), Mathf.Sin(Mathf.PI * 2 * index / roundNumA));
            rigidR.AddForce(dirVecR.normalized * 2, ForceMode2D.Impulse);
        }

        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
            Invoke("FireAround", 0.7f);
        else
            Invoke("Think", 3);
    }

    void Update()
    {
        thePlayer = FindObjectOfType< PlayerController > ();
        thePlayerStat = FindObjectOfType<PlayerStat>();
        Fire();
        Reload();
        if (enemyName == "Boss")
            return;
    }

    void Fire()
    {
        if (curShotDelay < maxShotDelay)
            return;
        if (enemyName == "Boss")
            Think();

        if (enemyName == "S")
        {
            GameObject bulletR = Instantiate(bulletObjA, transform.position + Vector3.right * 0.3f, transform.rotation);
            GameObject bulletL = Instantiate(bulletObjA, transform.position + Vector3.left * 0.3f, transform.rotation);
            
            Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
            Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();

            Vector3 dirVecR = thePlayer.transform.position - (transform.position + Vector3.right * 0.3f);
            Vector3 dirVecL = thePlayer.transform.position - (transform.position + Vector3.left * 0.3f);
            rigidR.AddForce(dirVecR.normalized * 10, ForceMode2D.Impulse);
            rigidL.AddForce(dirVecL.normalized * 10, ForceMode2D.Impulse);
        }
       /*else if (enemyName == "L")
        {
            GameObject bullet = Instantiate(bulletObjA, transform.position, transform.rotation);
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

            Vector3 dirVec = thePlayer.transform.position - transform.position;
            rigid.AddForce(dirVec.normalized * 10, ForceMode2D.Impulse);
        }*/

        curShotDelay = 0;
    }

    void Reload()
    {
        curShotDelay += Time.deltaTime;
    }

    public int OnHit(int _dmg) //체력에서 입은 데미지 만큼 빼주기
    {
        int dmg = _dmg;
        currentHP -= dmg;
        spriteRenderer.sprite = sprites[1];
        Invoke("ReturnSprite", 0.1f);
        if (currentHP <= 0)
        {
            Destroy(this.gameObject);
            PlayerStat.instance.currentEXP += exp;
        }

        return dmg;
    }

    

    void ReturnSprite()
    {
        spriteRenderer.sprite = sprites[0];
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BorderBullet")//BorderBullet이라는 컬라이더 벽에 닿으면 사라짐, 태그 필요
            Destroy(gameObject);
        /*else if (collision.gameObject.tag == "PlayerBullet") //14:20 태그 참고
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            OnHit(bullet.dmg);

            Destroy(collision.gameObject);

        }*/
        else if (collision.gameObject.tag == "PlayerBullet")
        {
            //int platk = collision.gameObject.GetComponent<PlayerStat>().atk;
            currentHP -= thePlayerStat.atk;
            spriteRenderer.sprite = sprites[1];
            Invoke("ReturnSprite", 0.1f);
            if (currentHP <= 0)
            {
                Destroy(this.gameObject);
                PlayerStat.instance.currentEXP += exp;
            }
        }
        
    }
}
