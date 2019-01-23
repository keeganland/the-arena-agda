using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextMerchant : ActivateTextAtLine {

    public GameObject newCue;

    public TextAsset StartText;

    public override void ChangeText(TextAsset NewTextYes, TextAsset NewTextNo)
    {
        if (NewTextYes)
            YesText = NewTextYes;
        if (NewTextNo)
            NoText = NewTextNo;
    }

    public override void ChangeCue()
    {
        theTextManager.InteractivityCue = newCue;
    }

    public override void ResetCue()
    {
        theTextManager.InteractivityCue = InteractivityCue;
        newCue.SetActive(false);
    }
    /*
    public override void ResetText()
    {
        if (FindObjectOfType<SaveManager>().CurrentMoney == 0 && FindObjectOfType<SaveManager>().fightNumberAvailable == 0)
        {
            theText = StartText;
            SetEventAtEndofText(false);
        }
        else
        {
            theText = TextBeginning;
            SetEventAtEndofText(eventAtEndofText);
        }
    }
    */
}
