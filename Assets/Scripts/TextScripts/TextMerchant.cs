﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextMerchant : InteractableNPC {

    public TextAsset brokeDialog;
    public TextAsset normalDialog;
    public TextAsset yesDialog;
    public TextAsset noDialog;

    new void Start()
    {
        base.Start();
        this.noAnswer = NoButtonEvent;
    }

    override public void StartInteraction()
    {
        ReloadMerchantText();
        base.StartInteraction();
    }


    public override void ChangeText(TextAsset NewTextYes, TextAsset NewTextNo)
    {
        //if (NewTextYes)
        //    YesText = NewTextYes;
        //if (NewTextNo)
        //    NoText = NewTextNo;
    }

    override public void ChangeCue()
    {
        //theTextManager.InteractivityCue = newCue;
    }

    override public void ResetCue()
    {
        //theTextManager.InteractivityCue = InteractivityCue;
        //newCue.SetActive(false);
    }

    override public void YesButtonEvent()
    {
        activateText.TheText = yesDialog;

        activateText.IsYesNoAtEndOfText = true;
        activateText.IsEventAtEndOfText = false;
        activateText.IsMultiCharDialog = false;
        activateText.IsRepeatingLastLine = false;

        activateText.Activate();
    }

    override public void NoButtonEvent()
    {
        activateText.TheText = noDialog;
        activateText.IsYesNoAtEndOfText = false;
        activateText.IsEventAtEndOfText = false;
        activateText.IsMultiCharDialog = false;

        activateText.Activate();
    }

    private void ReloadMerchantText()
    {
        if (SaveManager.Instance.CurrentMoney > 0)
        {
            this.TheText = normalDialog;
            activateText.TheText = normalDialog;
            IsYesNoAtEndOfText = true;
        }
        else
        {
            this.TheText = brokeDialog;
            activateText.TheText = brokeDialog;
            IsYesNoAtEndOfText = false;
        }
    }
}
