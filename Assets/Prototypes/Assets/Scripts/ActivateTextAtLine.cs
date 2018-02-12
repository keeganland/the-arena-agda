using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateTextAtLine : MonoBehaviour {

    public string activatedByTag;
    public string activatedByName;

    public TextAsset theText;

    public int startLine;
    public int endLine;

    public TextBoxManager theTextManager;

    public bool requireButtonPress;
    public bool waitForPress;
    public bool destroyWhenActivated;
    //public bool tagTriggersText = true; //I'm thinking of trying to get it to trigger with the collider's name instead of tag. Is this feasible?

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
            DisplayText();

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
        //Debug.Log("OnTriggerEnter activated by: " + other.tag);
        if (other.CompareTag(activatedByTag))
        {
            if(requireButtonPress)
            {
                theTextManager.EnableCue();
                waitForPress = true;
                return;
            }

            DisplayText();

            if (destroyWhenActivated)
            {
                Destroy(gameObject);
            }
        } 
    }
    
    void OnTriggerExit(Collider other)
    {
        //Debug.Log("OnTriggerExit activated by: " + other.tag);
        if (other.CompareTag(activatedByTag))
        {
            theTextManager.DisableCue();
            waitForPress = false;
        }
    }

    private void DisplayText()
    {
        theTextManager.ReloadScript(theText);
        theTextManager.currentLine = startLine;
        theTextManager.endAtLine = endLine;
        theTextManager.EnableTextBox();
    }
}
