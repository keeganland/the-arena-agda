using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextMerchant : InteractableNPC {

    public override void ChangeText(TextAsset NewTextYes, TextAsset NewTextNo)
    {
        //if (NewTextYes)
        //    YesText = NewTextYes;
        //if (NewTextNo)
        //    NoText = NewTextNo;
    }

    public override void ChangeCue()
    {
        //theTextManager.InteractivityCue = newCue;
    }

    public override void ResetCue()
    {
        //theTextManager.InteractivityCue = InteractivityCue;
        //newCue.SetActive(false);
    }
}
