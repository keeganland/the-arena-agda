using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextFloor1Arrival : InteractableNPC
{
    public string SecondSheet;
    public string SecondSpriteName;

    public string SecondCharacterName;

    private WeaponObject ThirdPerson; 

    new void Start()
    {
        base.Start();
    }

    public override void ResetCue()
    {
        throw new System.NotImplementedException();
    }


    public override void ChangeCue()
    {
        throw new System.NotImplementedException();
    }

    public override void ChangeText(TextAsset NewTextYes, TextAsset NewTextNo)
    {
        throw new System.NotImplementedException();
    }
}
