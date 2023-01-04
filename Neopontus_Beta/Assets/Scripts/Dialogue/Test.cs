using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[System.Serializable]
public class Dialogue
{
    [TextArea]
    public string dialogue;
    public Sprite cg;
}

public class Test : MonoBehaviour
{
    public static Test instance;
    private PlayerController theOrder;
    public dibar thediBar;

    [SerializeField] private SpriteRenderer sprite_StandingCG;
    [SerializeField] public SpriteRenderer sprite_DialogueBox;
    [SerializeField] private Text txt_Dialogue;

    public bool isDialogue = false;

    

    private int count = 0;

    [SerializeField] private Dialogue[] dialogue;

    void Start()
    {
        theOrder = FindObjectOfType<PlayerController>();
        thediBar = FindObjectOfType<dibar>();
        
    }

    public void ShowDialogue()
    {
            
            OnOff(true);
            count = 0;
            NextDialogue();
        
    }

    public void OnOff(bool _flag)
    {
        sprite_DialogueBox.gameObject.SetActive(_flag);
        sprite_StandingCG.gameObject.SetActive(_flag);
        txt_Dialogue.gameObject.SetActive(_flag);
        isDialogue = _flag;

        
    }



    private void NextDialogue()
    {
        txt_Dialogue.text = dialogue[count].dialogue;
        sprite_StandingCG.sprite = dialogue[count].cg;
        count++;
    }

    void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {


            if (isDialogue)
            {
                thediBar.ReVal_ON();
                theOrder.NotMove();
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    if (count < dialogue.Length)
                    {
                        NextDialogue();
                        //theDiBar.Ditest();
                    }
                    else
                    {
                        OnOff(false);
                        //theDiBar.DitestOff();

                    }
                }
                //else thediBar.ReVal_OFF();
            }
            else if (!isDialogue)
            {
                theOrder.Move();
                thediBar.ReVal_ON();
            }

        }
    }
    
}