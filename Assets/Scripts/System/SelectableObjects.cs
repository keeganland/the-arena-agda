using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


// Tutorial https://www.youtube.com/watch?v=ln03y7sJ_OE

public class SelectableObjects : MonoBehaviour, ISelectHandler, IPointerClickHandler, IDeselectHandler {

    public static HashSet<SelectableObjects> allSelectableObjects = new HashSet<SelectableObjects>();
    public static HashSet<SelectableObjects> currentlySelected = new HashSet<SelectableObjects>();

    Renderer myRenderer;


    void Awake()
    {
        allSelectableObjects.Add(this);
        myRenderer = GetComponent<Renderer>();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (this.name == "Boy")
        {
            BetterPlayer_Movement movBoy = this.GetComponent<BetterPlayer_Movement>();
            BetterPlayer_Movement movGirl = this.GetComponent<BetterPlayer_Movement>();

            movBoy.SwapGirl();
            movGirl.SwapGirl();
        }

        if (this.name == "Girl")
        {
            BetterPlayer_Movement movBoy = this.GetComponent<BetterPlayer_Movement>();
            BetterPlayer_Movement movGirl = this.GetComponent<BetterPlayer_Movement>();

            movBoy.SwapBoy();
            movGirl.SwapBoy();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightControl))
        {
            DeselectAll(eventData);
        }
        OnSelect(eventData);
    }

    public void OnSelect(BaseEventData eventData)
    {
       currentlySelected.Add(this);

        if (this.name == "Boy")
        {
            BetterPlayer_Movement movBoy = this.GetComponent<BetterPlayer_Movement>();

            if (movBoy.isCombat == true)
            {
                movBoy.boyActive = true;
                movBoy.SelectedParticleBoy();
            }
        }

        if(this.name == "Girl")
        {
            BetterPlayer_Movement movGirl = this.GetComponent<BetterPlayer_Movement>();

            if (movGirl.isCombat == true)
            {
                movGirl.boyActive = false;
                movGirl.SelectedParticleGirl();
            }
        }
    }

    public static void DeselectAll(BaseEventData eventData)
    {
        foreach(SelectableObjects selectable in currentlySelected)
        {
            selectable.OnDeselect(eventData);
        }
        currentlySelected.Clear();
    }
}


