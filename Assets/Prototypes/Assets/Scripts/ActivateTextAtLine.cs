using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateTextAtLine : MonoBehaviour {

    public TextAsset theText;

    public int startLine;
    public int endLine;

    public TextBoxManager theTextManager;

    public bool requireButtonPress;
    public bool waitForPress;
    public bool destroyWhenActivated;

    public string activatedByTag;

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
