using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextFloor1Arrival : ActivateTextAtLine 
{
    public string SecondSheet;
    public string SecondSpriteName;

    public string SecondCharacterName;

    private WeaponObject ThirdPerson; 

    new void Start()
    {
        base.Start();

        //if (SaveManager.EquipedItemsBoy.Count != 0)
        //{
        //    ThirdPerson = SaveManager.EquipedItemsBoy[0];
        //}
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

    public override void ChangeCue()
    {
        throw new System.NotImplementedException();
    }

    public override void ChangeText(TextAsset NewTextYes, TextAsset NewTextNo)
    {
        throw new System.NotImplementedException();
    }
}
