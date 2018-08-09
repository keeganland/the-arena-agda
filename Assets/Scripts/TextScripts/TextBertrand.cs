using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextBertrand: ActivateTextAtLine {

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

    }
}
