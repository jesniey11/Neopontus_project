using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    static public MovingObject instance;
    //public string currentMapNme; // transferMap 스크립트에 있는 transfreMapName 변수의 값을 저장.

    private BoxCollider2D boxCollider;
    public LayerMask layerMask;
    private bool notCoroutine = false;
    /*public string walkSound_1;
    public string walkSound_2;
    public string walkSound_3;
    public string walkSound_4;

    private AudioManager theAudio;*/

    public float speed;

    public Vector3 vector;

    //public float runSpeed;
    //private float applyRunSpeed;
    //private bool applyRunFlag = false;

    public int walkCount;
    private int currentWalkCount;

    //private bool canMove = true;

    public Animator animator;

    protected bool npcCanMove = true;
    public Queue<string> queue;
    
    // Start is called before the first frame update
    /*void Start()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            boxcollider = GetComponent<Boxcllider2D>();
            animator = GetComponent<Animator>();
            theAudio = FindObjectOfType<AudioManager>();
            instance = this;
        }
        else {
            Destroy(this, gameObject);
        }
    }

    IEnumerator MoveCoroutine() {
        while (Input.GetAxisRaw("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0)
        {
            if (Input.GetKey(KeyCode.LefgShift))
            {
                applyRunSpeed = runSpeed;
                applyRunFlag = true;
            }
            else 
            {
                applyRunSpeed = 0;
                applyRunFlag = false;
            }

            vector.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), transform.position.z);

            if(vector.x !- 0)
                vector.y 0;

            animator.SetFloat("DirX", vector.x);
            animator.SetFloat("DirY", vector.y);

            RaycastHit2D hit;
            //A지점, B지점
            //레이저
            //hit = Null;
            //hit - 방해물

            Vector2 start = transform.position; //A지점, 캐릭터의 현재 위치 값
            Vector2 end = start + new Vector2(vector.x * speed * walkCount, vector.y * speed * walkCount); //B지점, 캐릭터가 이동하고자 하는 위치값

            boxCollider.enabled = false;
            hit = Physics2D.Linecast(start, end, layerMask);
            boxCollider.enabled = true;

            if (hit.transform != null)
                break;

            animator.SetBool("Walking", true);

            int temp = Random.Range(1, 4);
            switch (temp)
            {
                case 1:
                    theAudio.Play(walkSound_1);
                    break;
                case 2:
                    theAudio.Play(walkSound_2);
                    break;
                case 3:
                    theAudio.Play(walkSound_3);
                    break;
                case 4:
                    theAudio.Play(walkSound_4);
                    break;
            }
            
            while(currntWalkCount < walkCount)
            {
                if (vector.x != 0)
                {
                    transform.Translate(vector.x * (speed + applyRunSpeed), 0, 0);
                }
                else if (vector.y != 0)
                {
                    transform.Translate(0.vector.y * (speed + applyRunSpeed), 0);
                }
                if (applyRunFlag)
                    currentWalkCount++;
                currentWalkCount++;
                yield return new WaitForSeconds(0.01f);
            }
            currentWalkCount = 0;
        }
        animator.SetBool("Walking", false);
        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove) {
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                canMove = false;
                StartCoroutine(MoveCoroutine());
            }
        }
    }*/
    
    protected void Move(string _dir, int _frequency = 5)
    {
        queue.Enqueue(_dir);
        if (!notCoroutine)
        {
            notCoroutine = true;
            StartCoroutine(MoveCoroutine(_dir, _frequency));
        }
    }

    IEnumerator MoveCoroutine(string _dir, int _frequency)
    {
        while (queue.Count != 0)
        {
            
            string direction = queue.Dequeue();
            npcCanMove = false;
            vector.Set(0, 0, vector.z);
            switch (_dir)
            {
                case "UP":
                    vector.y = 1f;
                    break;
                case "DOWN":
                    vector.y = -1f;
                    break;
                case "RIGHT":
                    vector.x = 1f;
                    break;
                case "LEFT":
                    vector.x = -1f;
                    break;
            }
            animator.SetFloat("DirX", vector.x);
            animator.SetFloat("DirY", vector.y);
            animator.SetBool("Walking", true);

            while (currentWalkCount < walkCount)
            {

                transform.Translate(vector.x * speed, 0, 0); //vector.y * speed

                currentWalkCount++;
                if (currentWalkCount == walkCount * 0.5f + 2)
                    boxCollider.offset = Vector2.zero;

                yield return new WaitForSeconds(0.01f);
            }
            currentWalkCount = 0;
            if (_frequency != 5)
                animator.SetBool("Walking", false);


            //animator.SetBool("Walking", false);
            //notCoroutine = false;
            npcCanMove = true;
        }
        animator.SetBool("Walking", false);
        notCoroutine = false;
    }


    public bool CheckCollision()
    {
        RaycastHit2D hit;
        //A지점, B지점
        //레이저
        //hit = Null;
        //hit - 방해물

        Vector2 start = transform.position; //A지점, 캐릭터의 현재 위치 값
        Vector2 end = start + new Vector2(vector.x * speed * walkCount, 0); //vector.y * speed * walkCount//B지점, 캐릭터가 이동하고자 하는 위치값

        boxCollider.enabled = false;
        hit = Physics2D.Linecast(start, end, layerMask);
        boxCollider.enabled = true;

        if (hit.transform != null)
            return true;
        return false;
    }
}
