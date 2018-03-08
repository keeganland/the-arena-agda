using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateTextAtLine : MonoBehaviour {

	/**
	 * Oh boy this one is getting messy.  
	 * TODO:
	 * does it even make sense to have this be ActivateTextAtLine going forward, or should it be simplified into ActivateText or something like that?
	 * since we're currently evolving the text manager to use more complicated stuff than simply an array of strings
	 */


	public string NPCName;
    public TextAsset theText;
	public bool useSpeechBubble;

    public int startLine;
    public int endLine;

    public TextBoxManager theTextManager;
	public GameObject talkBubble;

    public bool requireButtonPress;
    public bool destroyWhenActivated;
    public bool tagTriggersText = true; //I'm thinking of trying to get it to trigger with the collider's name instead of tag. Is this feasible?

    public string activatedByTag;
    public string activatedByName;

	private bool waitForPress = false;
    private bool textWasManuallyActivated; // If the text box has been activated, player should scroll through it before they get to end.
	//man did i manage to confuse myself with the above variable's name!
    private Player_Movement playerMover;

    // Use this for initialization
    void Start () {
        theTextManager = FindObjectOfType<TextBoxManager>();

		textWasManuallyActivated = false;

		if (talkBubble != null) {
			talkBubble.SetActive (false);
		}
	}
	
	// Update is called once per frame
	void Update () {

		if (waitForPress && Input.GetKeyDown(KeyCode.Return) && !textWasManuallyActivated)
        {
			textWasManuallyActivated = true;

            this.Activate();

            if (destroyWhenActivated)
            {
                Destroy(gameObject);
            }
        }
			
		//Refactor to take into account the queue structure!!!
        //if(theTextManager.currentLine > theTextManager.endAtLine)
		if(!theTextManager.getIsActive() || (theTextManager.currentLine > theTextManager.endAtLine))
        {
			textWasManuallyActivated = false;
			//i.e., if the text isn't even activated any more, ofc it's not manually activated
        }

		if (talkBubble != null && useSpeechBubble && (gameObject.name == theTextManager.getLastTriggered())) {
			Debug.Log ("The text box is active, was last triggered by " + theTextManager.getLastTriggered() + ", so let's turn on the talk bubble");
			talkBubble.SetActive (theTextManager.getIsActive() );
		}
    }

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("ActivateTextAtLine's OnTriggerEnter triggered by: " + other.name);
        if (other.name == activatedByName || (tagTriggersText && other.CompareTag(activatedByTag)))
        {
            if(requireButtonPress)
            {
                theTextManager.EnableCue();
                waitForPress = true;
                return;
            }

			this.Activate();

            if(destroyWhenActivated)
            {
				//Having the stuff that activates the text bubble in the update loop causes problems. uh oh. dunno how to fix yet
                Destroy(gameObject);
            }
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        Debug.Log("ActivateTextAtLine's OnTriggerEnter triggered by: " + other.name);
        if (other.CompareTag(activatedByTag))
        {
            theTextManager.DisableCue();
            waitForPress = false;
        }
    }

    private void Activate()
    {
		theTextManager.setLastTriggered (gameObject.name);
		theTextManager.setNPCName (NPCName);
        theTextManager.ReloadScript(theText);
        theTextManager.currentLine = startLine;
        theTextManager.endAtLine = endLine;
        theTextManager.EnableTextBox();
    }
}
