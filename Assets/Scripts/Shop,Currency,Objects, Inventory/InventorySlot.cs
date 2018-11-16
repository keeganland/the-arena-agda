using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerUpHandler, IPointerDownHandler {
    
    public Image icon;
    public Image Foreground;
    public int ItemIsForBoy;
    private Sprite initialIconSprite;
    private Color initialIconColor;

    public enum SlotType {INVENTORYSLOT ,WEAPON, HEAD, HAND, CHEST, PANTS, BOOTS};
    public SlotType slotType;

    public bool isEmpty = true;

    [SerializeField] WeaponObject item;

    Vector3 initialPos;

    private void Start()
    {
        initialIconSprite = icon.sprite;
        initialIconColor = icon.color;
    }

    public void AddItem(WeaponObject newItem)
    {
        item = newItem;
        icon.sprite = item.Icon;
        //Debug.Log(slotType);
        if(slotType != SlotType.INVENTORYSLOT)
        {
            icon.color = Color.white;
        }

        icon.enabled = true;
        isEmpty = false;

    }

    public void ClearSlot()
    {
        item = null;
        icon.sprite = initialIconSprite;
        icon.color = initialIconColor;

        if (slotType == SlotType.INVENTORYSLOT)
        {
            icon.enabled = false;
        }
        transform.localPosition = initialPos;
        isEmpty = true;

    }

    public void ShowStats()
    {
       
    }

    public void OnDrag()
    {
        gameObject.transform.position = Input.mousePosition;
        Foreground.transform.position = Input.mousePosition;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        GraphicRaycaster ray = GetComponentInParent<GraphicRaycaster>();
        List<RaycastResult> results = new List<RaycastResult>();

        ray.Raycast(eventData, results);
        //Debug.Log("Pointer Test: Point Up");

        foreach (RaycastResult result in results)
        {         
            if (result.gameObject.tag == "InventorySlot" && result.gameObject != this.gameObject)
            {
                //Debug.Log(result.gameObject.name + " Is Result");
                //Debug.Log("You released on another inventory Slot!!");
                if (item)
                {
                    if (item.slotType.ToString() == result.gameObject.GetComponent<InventorySlot>().slotType.ToString() || result.gameObject.GetComponent<InventorySlot>().slotType == SlotType.INVENTORYSLOT)
                    {
                        if (result.gameObject.GetComponent<InventorySlot>().isEmpty)
                        {
                            Debug.Log("You released on another Equipable Slot");
                            Debug.Log(slotType);
                            Debug.Log(result.gameObject.GetComponent<InventorySlot>().slotType);
                            result.gameObject.GetComponent<InventorySlot>().AddItem(item);

                            if (slotType != SlotType.INVENTORYSLOT && result.gameObject.GetComponent<InventorySlot>().slotType == SlotType.INVENTORYSLOT)
                            {                        
                                if (ItemIsForBoy == 2)
                                {
                                    Debug.Log("Here, trying to Unequip the Boy");
                                    InventoryManager.UnEquipItem(item.weaponName, 2);
                                }
                                else if(ItemIsForBoy == 1)
                                {
                                    InventoryManager.UnEquipItem(item.weaponName, 1);
                                }
                            }
                            else if (slotType == SlotType.INVENTORYSLOT && result.gameObject.GetComponent<InventorySlot>().slotType != SlotType.INVENTORYSLOT)
                            {
                                if (result.gameObject.GetComponent<InventorySlot>().ItemIsForBoy == 2)
                                {
                                    InventoryManager.EquipItem(item.weaponName, 2);
                                }
                                else if(result.gameObject.GetComponent<InventorySlot>().ItemIsForBoy == 1)
                                {
                                    InventoryManager.EquipItem(item.weaponName, 1);
                                }
                            }
                            ClearSlot();
                        }
                        else
                        {
                            WeaponObject olditem = result.gameObject.GetComponent<InventorySlot>().item;
                            result.gameObject.GetComponent<InventorySlot>().AddItem(item);

                            if (result.gameObject.GetComponent<InventorySlot>().ItemIsForBoy == 2)
                            {
                                InventoryManager.UnEquipItem(olditem.weaponName, 2);
                                InventoryManager.EquipItem(item.weaponName, 2);          
                            }
                            else if(result.gameObject.GetComponent<InventorySlot>().ItemIsForBoy == 1)
                            {
                                InventoryManager.UnEquipItem(olditem.weaponName, 1);
                                InventoryManager.EquipItem(item.weaponName, 1);
                            }
                            AddItem(olditem);
                        }
                    }
                }
            }
        }
        transform.localPosition = initialPos ;
        Foreground.sprite = null;
        Foreground.enabled = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        initialPos = transform.localPosition;
        Debug.Log(initialPos);
        //Debug.Log("Pointer Test: Point Down");
        if (!isEmpty)
        {
            Foreground.transform.position = icon.transform.position;
            Foreground.sprite = icon.sprite;
            Foreground.enabled = true;
        }
    }
}
