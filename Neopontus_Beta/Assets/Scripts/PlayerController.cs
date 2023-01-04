using UnityEngine;
using System.Collections;

using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class PlayerController : BaseCharacterController {

	public string currentSceneName; // !!!!!!!!
	private SaveLoad theSaveLoad;

	public bool notMove = false;
	public PlayerController thePlayer;

	private bool attacking = false;
	public float attackDelay;
	private float currentAttackDelay;

	//애니메이션 해시 이름 추가
	public readonly static int ANISTS_DEAD = Animator.StringToHash("Base Layer.Player_Dead");
	//저장데이터 파라미터
	public static float nowHpMax = 0;
	public static float nowHp = 0;
	//public static int score


	static public PlayerController instance;
	//현재 있는 씬 위치
	public string currentMapName;


	//페이드인아웃
	public static float startFadeTime = 2.0f;


	//포탈 이동 시 파괴 금지
	protected override void Start()
	{
		base.Start();
		thePlayer = GetComponent<PlayerController>();
		//zFoxFadeFilter.instance.FadeIn(Color.black, startFadeTime);
		startFadeTime = 2.0f;

		if (instance == null)
		{
			DontDestroyOnLoad(this.gameObject);
			instance = this;
		}
		else
		{
			Destroy(this.gameObject);
		}

		theSaveLoad = FindObjectOfType<SaveLoad>(); 

	}

	// === 외부 파라미터（Inspector 표시） =====================
	public float 	initHpMax = 20.0f;
	[Range(0.1f,100.0f)] public float 	initSpeed = 12.0f;

	//추가 코드
	//[System.NonSerialized] public string currentSceneName;
	// === 내부 파라미터 ======================================
	int 			jumpCount			= 0;
	bool			breakEnabled		= true;
	float 			groundFriction		= 0.0f;

	//서브카메라
	//[System.NonSerialized] public int comboCount = 0;
	//캐시
	//Image hudHpBar;
	//내부파라미터
	//float comboTimer = 0.0f;

	//기본 파라미터_9단원 추가
	[System.NonSerialized] public Vector3 enemyActiveZonePointA;
	[System.NonSerialized] public Vector3 enemyActiveZonePointB;
	[System.NonSerialized] public float groundY = 0.0f;

	// === 코드(지원 함수) ===============================
	public static GameObject GetGameObject()
	{
		return GameObject.FindGameObjectWithTag("Player");
	}
	public static Transform GetTranform()
	{
		return GameObject.FindGameObjectWithTag("Player").transform;
	}
	public static PlayerController GetController()
	{
		return GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
	}
	public static Animator GetAnimator()
	{
		return GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
	}

	// === 코드（Monobehaviour기본 기능 구현） ================
	protected override void Awake () {
		base.Awake ();

		// 파라미터 초기화
		speed = initSpeed;
		SetHP(initHpMax,initHpMax);

		//서브카메라 캐시
		//hudHpBar = GameObject.Find("HUD_HPBar").GetComponent<Image>();
	}


	//서브카메라에서 추가
	protected override void Update() {
		base.Update();

		//상태표시
		//hudHpBar.SetPosition(1, new Vector3(5.0f * (hp / hpMax), 0.0f, 0.0f));

		if (!notMove && !attacking)
		{
			if (Input.GetKeyDown(KeyCode.Q))
			{
				currentAttackDelay = attackDelay;
				attacking = true;
				animator.SetBool("Attacking", true);
			}
		}
		if (attacking)
		{
			currentAttackDelay -= Time.deltaTime;
			if (currentAttackDelay <= 0)
			{
				animator.SetBool("Attacking", false);
				attacking = false;
			}
		}

		
	}

	

	protected override void FixedUpdateCharacter () {
		// 착지했는지 검사
		if (jumped && !notMove && !attacking) {
			if ((grounded && !groundedPrev) || 
				(grounded && Time.fixedTime > jumpStartTime + 1.0f)) {
				animator.SetTrigger ("Idle");
				jumped = false;
				jumpCount = 0;
			}
		} 
		if (!jumped && !notMove && !attacking) {
			jumpCount = 0;
		}

		// 캐릭터 방향
		transform.localScale = new Vector3 (basScaleX * dir, transform.localScale.y, transform.localScale.z);

		// 점프 도중에 가로 이동 감속
		if (jumped && !grounded) {
			if (breakEnabled) {
				breakEnabled = false;
				speedVx *= 0.9f;
			}
		}

		// 이동 정지(감속) 처리
		if (breakEnabled) {
			speedVx *= groundFriction;
		}

		// 카메라_9단원 삭제
		//=====Camera.main.transform.position = transform.position - Vector3.forward;
	}

	// === 코드（기본 액션） =============================
	public override void ActionMove(float n) {
		if (!activeSts && !notMove && !attacking)
		{
			return;
		}

		//레이캐스트힛
		/*bool checkCollisionFlag = MovingObject.instance.CheckCollision();
		if (checkCollisionFlag)
			NotMove(); */
		
		// 초기화
		float dirOld = dir;
		breakEnabled = false;

		// 애니메이션 지정
		
		float moveSpeed = Mathf.Clamp(Mathf.Abs(n), -1.0f, +1.0f);
		animator.SetFloat("MovSpeed", moveSpeed);
		//animator.speed = 1.0f + moveSpeed;
		

		// 이동 체크
		if (n != 0.0f && !notMove && !attacking) { //여기요
			// 이동
			dir 	  = Mathf.Sign(n);
			moveSpeed = (moveSpeed < 0.5f) ? (moveSpeed * (1.0f / 0.5f)) : 1.0f;
			speedVx   = initSpeed * moveSpeed * dir;
		} 
		else  //여기요
		{
			// 이동 정지
			breakEnabled = true;
		}
		
		// 그 시점에서 돌아보기 검사
		if (dirOld != dir)
		{		//여기요 
			breakEnabled = true;
		}
	}

	public void ActionJump() {
		switch(jumpCount) {
		case 0 :
			if (grounded && !notMove && !attacking) {
				animator.SetTrigger ("Jump");
				GetComponent<Rigidbody2D>().velocity = Vector2.up * 30.0f;
				jumpStartTime = Time.fixedTime;
				jumped = true;
				jumpCount ++;
			}
			break;
		case 1 :
			if (!grounded && !notMove && !attacking) {
				animator.Play("Player_Jump",0,0.0f);
				GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x,20.0f);
				jumped = true;
				jumpCount ++;
			}
			break;
		}
	}

	public void ActionAttack() 
	{ animator.SetTrigger("Attacking"); }//Attack_A

	public void ActionDamage(float damage) {
		if (!activeSts) {
			return;
		}
		animator.SetTrigger("DMG_A");
		speedVx = 0;
		GetComponent<Rigidbody2D>().gravityScale = gravityScale;

		if (jumped) { damage *= 1.5f; }
		if (SetHP(hp - damage, hpMax)) {
			Dead(true); //사망
        }
	}

	public override void Dead(bool gameOver) {
		AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
		if (!activeSts || stateInfo.nameHash == ANISTS_DEAD) { return; }
		base.Dead(gameOver);
		SetHP(0, hpMax);
		Invoke("GameOver", 3.0f);

		//GameObject.Find("HUD_Dead").GetComponent<MeshRenderer>().enabled = true;
 	}

	public void GameOver()
	{
		//PlayerController.score = 0;
		//SceneManager.LoadScene(Application.loadedLevelName);
	}

	public override bool SetHP(float _hp, float _hpMax)
	{
		if (_hp > _hpMax)
		{
			_hp = _hpMax;
		}
		nowHp = _hp;
		nowHpMax = _hpMax;
		return base.SetHP(_hp, _hpMax);
	}

	public void NotMove() {
		thePlayer.notMove = true;
	}

	public void Move() {
		thePlayer.notMove = false;
	}

}


