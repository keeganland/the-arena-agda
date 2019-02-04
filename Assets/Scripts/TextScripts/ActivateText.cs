/* Responsible for taking text data and turning it into something displayable for the TextBoxManager
 * */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActivateText
{

    #region Properties
    #region Read/Write Properties
    private GameObject npcGameObject;
    public string NpcName { get; set; }
    public TextAsset TheText { get; set; }
    public TextAsset YesText { get; set; }
    public TextAsset NoText { get; set; }
    public TextAsset TextBeginning { get; set; }
    public bool IsEventAtEndOfText { get; set; }
    public bool IsYesNoAtEndOfText { get; set; }
    public bool IsMultiCharDialog { get; set; }
    public bool IsRepeatingLastLine { get; set; }
    public string ActivatedByTag { get; set; }
    public string ActivatedByName { get; set; }
    #endregion
    #endregion

    public bool textWasManuallyActivated; // If the text box has been activated, player should scroll through it before they get to end.
                                          //man did i manage to confuse myself with the above variable's name!
    private bool destroyNextTimeTextboxCloses = false;
    private bool DialoguehasbeenTriggered = false; //2nd bool that helps that;

    public ActivateText(GameObject npcGameObject)
    {
        this.npcGameObject = npcGameObject;

        #region Default values for properties
        NpcName = "";
        TheText = new TextAsset("default TheText");
        YesText = new TextAsset("default YesText");
        NoText = new TextAsset("default NoText");
        TextBeginning = new TextAsset("default TextBeginning");
        IsEventAtEndOfText = false;
        IsYesNoAtEndOfText = false;
        IsMultiCharDialog = false;
        ActivatedByTag = "default ActivatedByTag";
        ActivatedByName = "default ActivatedByName";
        #endregion 
    }
    public void Activate()
    {
        TextBoxManager.Instance.IsEventAtEndOfText = this.IsEventAtEndOfText;
        TextBoxManager.Instance.IsYesNoAtEndOfText = this.IsYesNoAtEndOfText;
        TextBoxManager.Instance.IsMultiCharDialog = this.IsMultiCharDialog;
        //if (HasDifferentDialogue) //Alex : I tried to do it so Every NPC has a different dialogue BUT we can find it with strings easily without mistakes ! It is supposed to work with EVERYONE ?
        //{
        //    if (saveManager.dialogueSaver.Contains(name + startLineSecondTime.ToString()))
        //        startLine = startLineSecondTime;
        //}

        if (npcGameObject)
        {
            TextBoxManager.Instance.LastTriggered = npcGameObject.name;
            TextBoxManager.Instance.NPCGameObject = this.npcGameObject;
        }
        TextBoxManager.Instance.NpcName = NpcName;

        //TextBoxManager.Instance.SetSprite(SpriteSheet, NameInSpriteSheet);
        if (IsRepeatingLastLine)
        {
            TextBoxManager.Instance.ReloadScript(this.LastLineOfDialogue()); // don't like this, but it should work.
        }
        else
        {
            TextBoxManager.Instance.ReloadScript(TheText);
        }
        TextBoxManager.Instance.EnableTextUI();
    }
    private void SaveDialogue()
    {
        //SaveManager.Instance.dialogueSaver.Add(name + startLineSecondTime.ToString());
    }
    public void LoadOnYes()
    {
        TheText = YesText;
        //PlayerEnableText(false);
        Activate();
        TextBoxManager.Instance.YesNoPrompt.SetActive(false);
    }
    public void LoadOnNo()
    {
        TheText = NoText;
        Activate(); //PlayerEnableText(false);
        TextBoxManager.Instance.IsEventAtEndOfText = false; //SetEventAtEndofText(false);
        TextBoxManager.Instance.YesNoPrompt.SetActive(false);
    }

    public TextAsset LastLineOfDialogue()
    {
        //Redundant!!!! that's major codesmell, but i just want it to work....

        if (TheText != null)
        {
            string[] textLines = new string[1];
            textLines = (TheText.text.Split('\n'));

            return new TextAsset(textLines[textLines.Length - 1]);
        }

        else
        {
            return new TextAsset("Error: No text file associated with this NPC when LastLineOfDialogue was called!");
        }
    }
}
