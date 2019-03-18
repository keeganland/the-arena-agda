using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextMerchant : InteractableNPC {

    public TextAsset brokeDialog;
    public TextAsset normalDialog;
    public TextAsset yesDialog;
    public TextAsset noDialog;

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

        EventManager.StopListening("answersYes", yesAnswer);
        this.yesAnswer = SpecialItemEvent;
        EventManager.StartListening("answersYes", yesAnswer);

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
        activateText.NpcName = this.npcName;
        this.yesAnswer = YesButtonEvent;
        this.noAnswer = NoButtonEvent;

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

    /*
     * Plays Alex's Waluigi item joke
     */
    public void SpecialItemEvent()
    {
        string specialItemName = "Cool Item";

        TextAsset specialText = new TextAsset("You bought a " + specialItemName);
        InventoryManager.AquireItem(specialItemName);

        activateText.NpcName = "";
        activateText.TheText = specialText;
        activateText.IsYesNoAtEndOfText = false;
        activateText.IsEventAtEndOfText = false;
        activateText.IsMultiCharDialog = false;

        TextBoxManager.Instance.ReloadScript(specialText);
        activateText.Activate();
    }
}
