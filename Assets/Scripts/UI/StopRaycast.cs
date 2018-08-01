using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // <-- Important line.

public class StopRaycast : MonoBehaviour, IPointerDownHandler // <-- Interface.
{
    public void OnPointerDown(PointerEventData data) // <-- Automatically called.
    {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
    }
}
