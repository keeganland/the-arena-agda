using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler {

    [SerializeField]
    Image selectionBoxImage;
    Rect selectrionRect;

    private Vector2 oldMousePosition;
    private Vector2 newMousePosition;

    public void OnEndDrag(PointerEventData eventData)
    {
        selectionBoxImage.gameObject.SetActive(false);
        foreach (SelectableObjects selectable in SelectableObjects.allSelectableObjects)
        {
            if (selectrionRect.Contains(Camera.main.WorldToScreenPoint(selectable.transform.position)))
            {
                selectable.OnSelect(eventData);
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.position.x < oldMousePosition.x)
        {
            selectrionRect.xMin = eventData.position.x;
            selectrionRect.xMax = oldMousePosition.x;
        }
        else
        {
            selectrionRect.xMin = oldMousePosition.x;
            selectrionRect.xMax = eventData.position.x;
        }

        if (eventData.position.y < oldMousePosition.y)
        {
            selectrionRect.yMin = eventData.position.y;
            selectrionRect.yMax = oldMousePosition.y;
        }
        else
        {
            selectrionRect.yMin = oldMousePosition.y;
            selectrionRect.yMax = eventData.position.y;
        }

        selectionBoxImage.rectTransform.offsetMin = selectrionRect.min;
        selectionBoxImage.rectTransform.offsetMax = selectrionRect.max;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightControl))
        {
            SelectableObjects.DeselectAll(new BaseEventData(EventSystem.current));
        }
        selectionBoxImage.gameObject.SetActive(true);
        oldMousePosition = eventData.position;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        float myDistance = 0;

        foreach (RaycastResult result in results)
        {
            if (result.gameObject == gameObject)
            {
                myDistance = result.distance;
                break;
            }
        }

        GameObject nextObject = null;
        float maxDistance = Mathf.Infinity;

        foreach (RaycastResult result in results)
        {
            if (result.distance > myDistance && result.distance < maxDistance)
            {
                nextObject = result.gameObject;
                maxDistance = result.distance;
            }
        }

        if (nextObject)
        {
            ExecuteEvents.Execute<IPointerClickHandler>(nextObject, eventData, (x, y) => { x.OnPointerClick((PointerEventData)y); });
        }
    }
}
