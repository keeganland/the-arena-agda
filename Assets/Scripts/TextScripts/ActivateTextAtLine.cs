/*
 * 2018-10-13: Replacing this with ActivateText.cs
 * 
 * 
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class ActivateTextAtLine : MonoBehaviour
{

    /*
	 * Oh boy this one is getting messy.  
	 * TODO:
	 * does it even make sense to have this be ActivateTextAtLine going forward, or should it be simplified into ActivateText or something like that?
	 * since we're currently evolving the text manager to use more complicated stuff than simply an array of strings
	 */

    /* Alex : The changes I made are to create more flexibility with PNGs. 
     * 
     * ABSTRACT CLASS : it allows us to have different reaction of NPC's with different "Cues", but also different "Starts, End Dialogues, etc...".
     * 
     * So far we can :  - Have 2 different Text that can be used as Openings, 
     *                  - Have 2 different (or more) Texts after a simple "Yes/No" answer.
     *                  - Reset the text to start from the beginning (Before "Yes/No")
     *                  - Have two different cue (Initial text -> "Yes/No" (or event) -> YesText/NoText -> "Yes/No" -> Any Event)
     *                  - Choose if there will be an event after the text (SetEventAfterText). 
     *                  - Save if we want a different dialogue after the First Introduction (Skip the greetings and go straight to the point)
     */

    SaveManager saveManager;

    public bool useXml;
    public string NPCName;
    public TextAsset theText;
    public TextAsset theXml;
    public bool useSpeechBubble;

    public int startLine; //Alex to Keegan : it actually doesn't work. If your text is 3 lines [0,1,2] but you ask to start line [1] and end line [2], then the textboxManager will real line [0,1] instead of [1,2]
    public int startLineSecondTime; //If it is solved, I suppose my "Save" text will work fine ! (For Bertrand it will be : read line [0,1,2] the first time then only line [2] the other times !).
    public int endLine;

    protected TextBoxManager theTextManager;
    public GameObject talkBubble;

    public bool requireButtonPress;
    public bool destroyWhenActivated;
    public bool destroyWhenFinished;
    public bool tagTriggersText = true; //I'm thinking of trying to get it to trigger with the collider's name instead of tag. Is this feasible?

    public string activatedByTag;
    public string activatedByName;

    public bool eventAtEndofText; //2018-10-13 - Keegan - Completely pointless as far as I can tell? should be replaced with 

    private bool waitForPress = false;
    public bool textWasManuallyActivated; // If the text box has been activated, player should scroll through it before they get to end.
                                           //man did i manage to confuse myself with the above variable's name!
    private bool destroyNextTimeTextboxCloses = false;
    private Player_Movement playerMover;

    public bool OnlyTriggerDialogueOnce; //Alex : To allow the player to trgger the dialogue only once, then has to click on NPC to trigger dialogue manually
    private bool DialoguehasbeenTriggered; //2nd bool that helps that;
    public bool HasDifferentDialogue; //Alex : This is... to start the dialogue on another line after talking it to once. If the NPC already introduced itself, we don't want it to introduce itself again.

    public GameObject InteractivityCue;

    [Header("Alex changes : new text after Yes/No")]

    public TextAsset YesText;
    public TextAsset NoText;

    public TextAsset TextBeginning;

    public string SpriteSheet;
    public string NameInSpriteSheet;

    protected UnityAction yesAnswer;
    protected UnityAction noAnswer;

    private void Awake()
    {
        saveManager = FindObjectOfType<SaveManager>(); 
    }
    // Use this for initialization
    protected void Start()
    {
        yesAnswer = this.YesButtonEvent;
        noAnswer = this.NoButtonEvent;

        /*
         * As of 2018/9/21, text box manager will be a singleton, its script being part of an object in the NeverUnload scene.
         */

        //theTextManager = FindObjectOfType<TextBoxManager>();
        theTextManager = TextBoxManager.Instance;

        theTextManager.eventAtEndofText = eventAtEndofText;

        textWasManuallyActivated = false;

        if (talkBubble != null)
        {
            talkBubble.SetActive(false);
        }

        TextBeginning = theText;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(gameObject.name + "'s useXml == " + useXml);

        /*
         * 2018/10/13 - Keegan: old code. Maybe get rid of this. 
         * 
         */
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
        if (!theTextManager.getIsActive() || (theTextManager.currentLine > theTextManager.endAtLine))
        {
            textWasManuallyActivated = false;
            //i.e., if the text isn't even activated any more, ofc it's not manually activated
        }

        if (talkBubble != null && useSpeechBubble && (gameObject.name == theTextManager.getLastTriggered()))
        {
            //Debug.Log("The text box is active, was last triggered by " + theTextManager.getLastTriggered() + ", so let's turn on the talk bubble");
            talkBubble.SetActive(theTextManager.getIsActive());
        }

        if (destroyNextTimeTextboxCloses && !theTextManager.getIsActive())
        {
            talkBubble.SetActive(false);
            Destroy(gameObject);
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (OnlyTriggerDialogueOnce && DialoguehasbeenTriggered) //Logic that allows : Trigger Dialogue once then manually click on NPC for dialogue, or not. (if, else if)
        {
            return;
        }

        else if ((OnlyTriggerDialogueOnce && !DialoguehasbeenTriggered) || !OnlyTriggerDialogueOnce)
        {
            //Debug.Log("ActivateTextAtLine's OnTriggerEnter triggered by: " + other.name);
            if (other.name == activatedByName || (tagTriggersText && other.CompareTag(activatedByTag)))
            {
                PlayerEnableText(true);

                if (HasDifferentDialogue == true)
                {
                    SaveDialogue();
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        //Debug.Log("ActivateTextAtLine's OnTriggerEnter triggered by: " + other.name);
        if (activatedByTag != "")
        {
            if (other.CompareTag(activatedByTag))
            {
                //DisableDialogPrompt is what we should be using, DisableCue is what Alex was using. 
                theTextManager.DisableCue();
                theTextManager.DisableDialogPrompt();
                waitForPress = false;
            }
        }
    }


    /*
     * 2018/10/13 - Exists for refactoring purposes
     */
    public void PlayerEnableText (bool start)
    {
        if (start)
        {
            ResetText();
        }
        this.PlayerEnableText();
    }

    public void PlayerEnableText() //To allow the player to CLICK on the NPC and start dialogue (or pass near it if not clicked, without trigger). The first dialogue, start will be true, if it's an answer to "Yes/No", start will be false.
    {
        /*
         * Worthless while ResetText is public, can't really make ResetText private because of C# reasons, neither do we really want to
         */
        /*
        if(start)
        {
            ResetText();
        }
        */


        /*
         * Again, keep in mind Alex got confused what the Interactivity Cue is supposed to be. Possibly get rid of this
         */
        if(InteractivityCue)
        {
            theTextManager.SetInteractivityCue(InteractivityCue);   
        }


        /*
         * 2018/10/13 - The below seems to be a result of Alex misunderstanding what EnableCue does
         */
        /*
        if (requireButtonPress)
        {
            theTextManager.EnableCue();
            waitForPress = true;
            return;
        }*/

        EventManager.StartListening("answersYes", yesAnswer);
        EventManager.StartListening("answersNo", noAnswer);

        this.Activate();

        if (destroyWhenFinished)
        {
            destroyNextTimeTextboxCloses = true;
        }

        if (destroyWhenActivated)
        {
        //Having the stuff that activates the text bubble in the update loop causes problems. uh oh. dunno how to fix yet

        /**
        * The below is a partial fix. The outstanding issue is that once that balloon activates, this script is effectively over.
        * Therefore, there's nothing available to make the balloon DISAPPEAR
        * 
        * What I need is someway to check if the text box is FINISHED.
        * 
        * I need to brainstorm some fixes: Maybe rather than "destroy when activated" I can do "destroy when finished"?
        * 

        if (talkBubble != null && useSpeechBubble && (gameObject.name == theTextManager.getLastTriggered()))
        {
            Debug.Log("The text box is active, was last triggered by " + theTextManager.getLastTriggered() + ", so let's turn on the talk bubble");
            talkBubble.SetActive(theTextManager.getIsActive());
        }

        */
        Destroy(gameObject);
        }
    }

    private void Activate()
    {
        if (HasDifferentDialogue) //Alex : I tried to do it so Every NPC has a different dialogue BUT we can find it with strings easily without mistakes ! It is supposed to work with EVERYONE ?
        {
            if (saveManager.dialogueSaver.Contains(name + startLineSecondTime.ToString()))
                startLine = startLineSecondTime;
        }
        //Debug.Log("In ActivateTextAtLine.cs's Activate function");
        theTextManager.setLastTriggered(gameObject.name);
        theTextManager.setNPCName(NPCName);
        theTextManager.setNPCGameObject(this.gameObject);
        theTextManager.SetSprite(SpriteSheet, NameInSpriteSheet);

        //Debug.Log("useXml == " + useXml);
        if (useXml)
        {
            theTextManager.ReloadScriptXML(theXml);
        }
        else
        {
            theTextManager.ReloadScript(theText);
        }
        
        theTextManager.currentLine = startLine;
        theTextManager.endAtLine = endLine;
        theTextManager.EnableTextBox();
    }

    private void SaveDialogue()
    {
        saveManager.dialogueSaver.Add(name + startLineSecondTime.ToString());
    }

    public void SetEventAtEndofText(bool eventbool)
    {
        theTextManager.eventAtEndofText = eventbool;
    }

    public void LoadOnYes()
    {
        theText = YesText;
        PlayerEnableText(false);
        theTextManager.DisableCue();
        theTextManager.DisableDialogPrompt();
        ChangeCue();
    }

    public void LoadOnNo()
    {
        theText = NoText;
        PlayerEnableText(false);
        SetEventAtEndofText(false);
        theTextManager.DisableCue();
        theTextManager.DisableDialogPrompt();
        ResetCue();
    }

    private void ResetText()
    {
        theTextManager = FindObjectOfType<TextBoxManager>();

        theTextManager.eventAtEndofText = eventAtEndofText;

        textWasManuallyActivated = false;

        if (talkBubble != null)
        {
            talkBubble.SetActive(false);
        }

        TextBeginning = theText;
    }

    //abstract public void ResetText(); //Alex : I made a mistake, resetText needs to have elements in it. I don't know how to force the "add" in this script so just copy past the textBertrand ResetText() to have the sample one. 

    abstract public void ChangeText(TextAsset NewTextYes, TextAsset NewTextNo);

    abstract public void ChangeCue();

    abstract public void ResetCue();

    virtual public void YesButtonEvent()
    {
        Debug.Log("This is ActivateTextAtLine.cs, confirming we're in the YesButtonEvent method");      
        EventManager.StopListening("answersYes", yesAnswer);
    }

    virtual public void NoButtonEvent()
    {
        Debug.Log("This is ActivateTextAtLine.cs, confirming we're in the NoButtonEvent method");
        EventManager.StopListening("answersNo", noAnswer);
    }
}
