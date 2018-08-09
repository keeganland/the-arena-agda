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
        theTextManager.interactivityCue = newCue;
    }

    public override void ResetCue()
    {
        theTextManager.interactivityCue = InteractivityCue;
        newCue.SetActive(false);
    }

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
}
