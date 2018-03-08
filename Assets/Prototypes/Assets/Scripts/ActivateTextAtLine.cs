using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateTextAtLine : MonoBehaviour {

	/**
	 * Oh boy this one is getting messy.  
	 * 
	 */


	public string NPCName;
    public TextAsset theText;

    public int startLine;
    public int endLine;

    public TextBoxManager theTextManager;
	public GameObject talkBubble;

    public bool requireButtonPress;
    public bool destroyWhenActivated;

    private bool waitForPress = false;
    public bool tagTriggersText = true; //I'm thinking of trying to get it to trigger with the collider's name instead of tag. Is this feasible?

    public string activatedByTag;
    public string activatedByName;

    private bool textStarted; // If the text box has been activated, player should scroll through it before they get to end.
    private Player_Movement playerMover;

    // Use this for initialization
    void Start () {
        theTextManager = FindObjectOfType<TextBoxManager>();
        textStarted = false;

		if (talkBubble != null) {
			talkBubble.SetActive (false);
		}

	}
	
	// Update is called once per frame
	void Update () {
		if (waitForPress && Input.GetKeyDown(KeyCode.Return) && !textStarted)
        {
            textStarted = true;

            this.Activate();

            if (destroyWhenActivated)
            {
                Destroy(gameObject);
            }
        }
			
		//Refactor to take into account the queue structure!!!
        if(theTextManager.currentLine > theTextManager.endAtLine)
        {
            textStarted = false;
        }

		//Normally it's stupid that I'm even making this a seperate if-block, but in the short/medium term I want to get rid of the condition of the previous if-block and change what flips textStarted
		if (textStarted == false && talkBubble != null) {
			talkBubble.SetActive (false);
		}
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("ActivateTextAtLine's OnTriggerEnter triggered by: " + other.name);
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
		Debug.Log ("entered ActivateTextAtLine's Activate() method");

		if (talkBubble != null) {
			Debug.Log ("talkBubble should activate");
			talkBubble.SetActive (true);
			Debug.Log (talkBubble.name + " active?");
		}

		theTextManager.SetNPCName (NPCName);
        theTextManager.ReloadScript(theText);
        theTextManager.currentLine = startLine;
        theTextManager.endAtLine = endLine;
        theTextManager.EnableTextBox();
    }
}
