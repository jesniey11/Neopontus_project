using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ending : MonoBehaviour
{

	public GameObject go;

	private void OnTriggerStay2D(Collider2D collision)
	{
		go.SetActive(true);
	}

}