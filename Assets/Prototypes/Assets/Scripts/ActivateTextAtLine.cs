using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateTextAtLine : MonoBehaviour {

    public TextAsset theText;

    public int startLine;
    public int endLine;

    public TextBoxManager theTextManager;

    public bool requireButtonPress;
    public bool destroyWhenActivated;
<<<<<<< HEAD
    private bool waitForPress = false;
    //public bool tagTriggersText = true; //I'm thinking of trying to get it to trigger with the collider's name instead of tag. Is this feasible?
=======

    public string activatedByTag;
>>>>>>> fcd092c27a787f485ab4c4d273777d8a18f63002

    private bool textStarted; // If the text box has been activated, player should scroll through it before they get to end.
    private Player_Movement playerMover;

    // Use this for initialization
    void Start () {
        theTextManager = FindObjectOfType<TextBoxManager>();
        textStarted = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (waitForPress && Input.GetKeyDown(KeyCode.Return) && !textStarted)
        {
            textStarted = true;

            theTextManager.ReloadScript(theText);
            theTextManager.currentLine = startLine;
            theTextManager.endAtLine = endLine;
            theTextManager.EnableTextBox();

            if (destroyWhenActivated)
            {
                Destroy(gameObject);
            }
        }

        if(theTextManager.currentLine > theTextManager.endAtLine)
        {
            textStarted = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(activatedByTag))
        {
            /*
            if(activatedByTag == "Player")
            {
                playerMover = other.GetComponent<Player_Movement>();
                playerMover.stopMoving = true;
            }
            */

            if(requireButtonPress)
            {
                waitForPress = true;
                return;
            }

            theTextManager.ReloadScript(theText);
            theTextManager.currentLine = startLine;
            theTextManager.endAtLine = endLine;
            theTextManager.EnableTextBox();

            if(destroyWhenActivated)
            {
                Destroy(gameObject);
            }
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(activatedByTag))
        {
            waitForPress = false;
        }
    }
}
