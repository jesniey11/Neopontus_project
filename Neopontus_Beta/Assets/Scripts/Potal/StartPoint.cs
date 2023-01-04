using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour
{
    public string startPoint;
    private PlayerController thePlayer;
    private CameraFollow theCamera;

    // Start is called before the first frame update
    void Start()
    {
        theCamera = FindObjectOfType<CameraFollow>();
        thePlayer = FindObjectOfType<PlayerController>();
        if (startPoint == thePlayer.currentSceneName)
        {
            theCamera.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, theCamera.transform.position.z);
            thePlayer.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, thePlayer.transform.position.z);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
