using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRay : MonoBehaviour
{
	private PlayerController theOrder;

	//����׼�
	GameObject scanObject;
	int direction;
	float detect_range = 0.1f;
	Rigidbody2D rigid;
	BoxCollider2D collider_;

	// Start is called before the first frame update
	void Start()
    {
		theOrder = FindObjectOfType<PlayerController>();
		rigid = GetComponent<Rigidbody2D>();
		collider_ = GetComponent<BoxCollider2D>();
		//Debug.Log(rigid);
	}

    // Update is called once per frame
    void Update()
    {
		theOrder.NotMove();
		if (Input.GetButton("Horizontal"))
		{
			if (Input.GetAxisRaw("Horizontal") < 0)//(Input.GetAxisRaw("Horizontal") == -1)
			{
				//spriteRenderer.flipX = true;
				this.GetComponent<SpriteRenderer>().flipX = true;
				direction = -1;

			}
			else
			{
				//spriteRenderer.flipX = false;
				this.GetComponent<SpriteRenderer>().flipX = false;
				direction = 1;

			}
		}


	}

	//����׼�
	private void FixedUpdate()
	{
		//Debug.Log(rigid.position);
		//����׼�
		Debug.DrawRay(rigid.position, new Vector2(direction * detect_range, 0), new Color(0, 0, 1));

		
		//Layer�� Object�� ��ü�� rayHit_detect�� ���� 
		RaycastHit2D rayHit_detect = Physics2D.Raycast(rigid.position, new Vector2(direction, 0), detect_range, LayerMask.GetMask("Object"));
		//Debug.Log(rigid.position.GetType());
		//Debug.Log(detect_range.GetType());
		Debug.Log(rigid.position);


		//�����Ǹ� scanObject�� ������Ʈ ���� 
		if (rayHit_detect.collider != null)
		{
			scanObject = rayHit_detect.collider.gameObject;
			Debug.Log(scanObject.name);

		}
		else
		{
			scanObject = null;
			
		}

	}
}
