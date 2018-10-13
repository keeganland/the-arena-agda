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


