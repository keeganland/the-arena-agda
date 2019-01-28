/* Responsible for making sure NPCs can be interacted with, such that they have standardized behaviour but can still output different text
 * Sends data to ActivateText, which it encapsulates, and which is responsible for interpreting the data it gets into something that plays nicely with TextBoxManager
 * 
 * ActivateText should always be the same. Because individual NPCs can differ (including in functionality if we extend this class), and because we might change how we set up how we format the raw input,
 * there are two stages of text processing. This file, and then ActivateText.cs
 * */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class InteractableNPC : MonoBehaviour
{

    public string npcName;
    public TextAsset theText;
    public bool IsEventAtEndOfText = false;
    public bool IsYesNoAtEndOfText = false;
    public bool IsMultiCharDialog = false;
    private ActivateText activateText;

    #region Properties
    public TextAsset TheText
    {
        get
        {
            return theText;
        }
        set
        {
            theText = value;
        }
    }
    #endregion

    protected UnityAction yesAnswer;
    protected UnityAction noAnswer;
    protected UnityAction resetNpcs;

    private void Awake()
    {
        activateText = new ActivateText(gameObject);
        resetNpcs = this.ResetNpcs;
        yesAnswer = this.YesButtonEvent;
        noAnswer = this.NoButtonEvent;
    }

    // Start is called before the first frame update
    protected void Start()
    {
        if (!TheText)
        {
            TheText = new TextAsset("This NPC does not have an associated text file!");
        }
        activateText.TheText = TheText;
        if (npcName == null)
        {
            npcName = "Unnamed";
        }
        activateText.NpcName = npcName;

    }

    /* Based on the "PlayerEnableText" method I'm getting rid of from ActivateTextAtLine
     * Alex wrote this method and it's really confusing and unplesant.
     * */
    public void StartInteraction()
    {
        /* Alex's original comment on PlayerEnableText read as follows:
         * //To allow the player to CLICK on the NPC and start dialogue (or pass near it if not clicked, without trigger). The first dialogue, start will be true, if it's an answer to "Yes/No", start will be false.
         * 
         * The "start" argument was really confusing but the intention seems to be that it's a flag that tells you whether you're loading a new convo
         * or branching using a yes/no choice.
         * */
        //if (newconvo)
        //{
        //    activateText.ResetText();
        //}


        EventManager.StartListening("resetNpcs", resetNpcs);
        EventManager.StartListening("answersYes", yesAnswer);
        EventManager.StartListening("answersNo", noAnswer);

        activateText.IsYesNoAtEndOfText = this.IsYesNoAtEndOfText; //noodly. please consider refactoring, but also - don't break things.
        activateText.IsEventAtEndOfText = this.IsEventAtEndOfText;
        activateText.IsMultiCharDialog = this.IsMultiCharDialog;
        activateText.Activate();        
    }

    abstract public void ChangeText(TextAsset NewTextYes, TextAsset NewTextNo);

    abstract public void ChangeCue();

    abstract public void ResetCue();

    virtual public void ResetNpcs()
    {
        EventManager.StopListening("answersYes", yesAnswer);
        EventManager.StopListening("answersNo", noAnswer);
    }

    virtual public void YesButtonEvent()
    {
        Debug.Log("This is InteractableNPC.cs, confirming we're in the YesButtonEvent method");
        EventManager.StopListening("answersYes", yesAnswer);
    }
    virtual public void NoButtonEvent()
    {
        Debug.Log("This is InteractableNPC.cs, confirming we're in the NoButtonEvent method");
        //EventManager.StopListening("answersNo", noAnswer); //redundant with below DisableTextUI call
        TextBoxManager.Instance.DisableTextUI();
        TextBoxManager.Instance.ReloadScript(theText);
    }
}
