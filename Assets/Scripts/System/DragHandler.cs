using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler {

    [SerializeField]
    Image selectionBoxImage;
    Rect selectionRect;

    private Vector2 oldMousePosition;
    private Vector2 newMousePosition;

    public void OnEndDrag(PointerEventData eventData)
    {
        if (Input.GetMouseButtonUp(0))
        {
            selectionBoxImage.gameObject.SetActive(false);

            bool inRect = false;

            foreach (SelectableObjects selectable in SelectableObjects.allSelectableObjects)
            {
                if (selectionRect.Contains(Camera.main.WorldToScreenPoint(selectable.transform.position)))
                {
                    inRect = true;
                }
            }

            foreach (SelectableObjects selectable in SelectableObjects.allSelectableObjects)
            {
                if (selectionRect.Contains(Camera.main.WorldToScreenPoint(selectable.transform.position)))
                {
                    selectable.OnSelect(eventData);
                }
                else if(inRect)
                {
                    selectable.OnDeselect(eventData);
                }
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (Input.GetMouseButton(0))
        {
            if (eventData.position.x < oldMousePosition.x)
            {
                selectionRect.xMin = eventData.position.x;
                selectionRect.xMax = oldMousePosition.x;
            }
            else
            {
                selectionRect.xMin = oldMousePosition.x;
                selectionRect.xMax = eventData.position.x;
            }

            if (eventData.position.y < oldMousePosition.y)
            {
                selectionRect.yMin = eventData.position.y;
                selectionRect.yMax = oldMousePosition.y;
            }
            else
            {
                selectionRect.yMin = oldMousePosition.y;
                selectionRect.yMax = eventData.position.y;
            }

            selectionBoxImage.rectTransform.offsetMin = selectionRect.min;
            selectionBoxImage.rectTransform.offsetMax = selectionRect.max;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (Input.GetMouseButton(0))
        {
            selectionBoxImage.gameObject.SetActive(true);
            oldMousePosition = eventData.position;
        }
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
