using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextBertrand: InteractableNPC
{

    new void Start()
    {
        base.Start();
        this.yesAnswer = YesButtonEvent;
    }


    [Header("Unique to Bertrand - for now")]
    public GameObject fightCanvas;

    public override void ChangeText(TextAsset TextYes, TextAsset TextNo)
    {
        throw new System.NotImplementedException();
    }

    public override void ChangeCue()
    {
        throw new System.NotImplementedException();
    }

    public override void ResetCue()
    {
        throw new System.NotImplementedException();
    }
    /*
    public override void ResetText()
    {
        theTextManager = FindObjectOfType<TextBoxManager>();

        theTextManager.eventAtEndofText = eventAtEndofText;

        textWasManuallyActivated = false;

        if (talkBubble != null)
        {
            talkBubble.SetActive(false);
        }

        TextBeginning = theText;
    }*/

    new void YesButtonEvent()
    {
        base.YesButtonEvent();
        Debug.Log("This is TextBertrand.cs, confirming we're in the YesButtonEvent method");
        TextBoxManager.Instance.DisableTextUI();
        fightCanvas.SetActive(true);
    }

}
