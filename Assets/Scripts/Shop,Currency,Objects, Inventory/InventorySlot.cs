using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerUpHandler, IPointerDownHandler {
    
    public Image icon;
    public Image Foreground;
    public int ItemIsForBoy; // = 2 if it's the boy's slot, = 1 if it's the girl's slot, = 0 if it is inventory slot. It groups the slots as set such as : set inventory, set boy (head, hand, ...), set girl. 
    private Sprite initialIconSprite;
    private Color initialIconColor;

    public enum SlotType {INVENTORYSLOT ,WEAPON, HEAD, HAND, CHEST, PANTS, BOOTS}; //This gives the type of slot for the inventory slot. It will prevent the player to equip a ring as a head armor. 
    public SlotType slotType;

    public bool isEmpty = true; //if the slot if empty

    public GameObject StatsTextHolder; //This section contains the data for the mouse "hovering" part of the inventory. 

    [SerializeField] WeaponObject item; //What item does the slot contains? 

    Vector3 initialPos; //Pos of the slot before dragging the item to another. It will reset once the player release the mouse. 

    private void Start()
    {
        initialIconSprite = icon.sprite;
        initialIconColor = icon.color;
    }

    public void AddItem(WeaponObject newItem) //used to add an item, it gets the icen as WeaponObject and updates UI accordingly. 
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

    public void ClearSlot() //clear existing item.
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

    public void ShowStats() //When the mouse is hovered, we show the stats of the item such as the name, and the description.
    {
        if (item)
        {
            StatsTextHolder.transform.position = Input.mousePosition;
            StatsTextHolder.GetComponent<StatsItemHolder>().GetWeaponObject(item);
            StatsTextHolder.SetActive(true);
        }
    }

    public void HiddeStats() //When the mouse leaves the inventoryslot, the stats disapear. Alex : maybe we can make it disapear when we start dragging the inventorySlot. 
    {
        StatsTextHolder.SetActive(false);
    }

    public void OnDrag() //move the item. 
    {
        gameObject.transform.position = Input.mousePosition;
        Foreground.transform.position = Input.mousePosition;
    }

    //if the item is moved, where to and what happen on release ? If a WeaponObject.HEAD is moved toward a HEAD.inventory slot, it will be equiped is the slot is empty, 
    //and replace the existing object if the slot is not empty. 

    public void OnPointerUp(PointerEventData eventData) 
    {

        //I use a GraphicRaycaster to interact with the UI slots. 
        //Results store all UI's the mouse see when it's been released (after a drag).
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
                            else if((slotType != SlotType.INVENTORYSLOT && ItemIsForBoy == 2) && ((result.gameObject.GetComponent<InventorySlot>().slotType != SlotType.INVENTORYSLOT) && 
                                                                                                  result.gameObject.GetComponent<InventorySlot>().ItemIsForBoy == 1))
                            {
                                    Debug.Log("Transfer Item from Boy to Girl");
                                    InventoryManager.UnEquipItem(item.weaponName, 2);
                                    InventoryManager.EquipItem(item.weaponName, 1);                           
                            }

                            else if ((slotType != SlotType.INVENTORYSLOT && ItemIsForBoy == 1) && ((result.gameObject.GetComponent<InventorySlot>().slotType != SlotType.INVENTORYSLOT) &&
                                                                                         result.gameObject.GetComponent<InventorySlot>().ItemIsForBoy == 2))
                            {
                                Debug.Log("Transfer Item from Boy to Girl");
                                InventoryManager.UnEquipItem(item.weaponName, 1);
                                InventoryManager.EquipItem(item.weaponName, 2);
                            }

                            ClearSlot();
                        }
                        else
                        {
                            WeaponObject olditem = result.gameObject.GetComponent<InventorySlot>().item;
                            result.gameObject.GetComponent<InventorySlot>().AddItem(item);

                            if ((result.gameObject.GetComponent<InventorySlot>().ItemIsForBoy == 2 && slotType == SlotType.INVENTORYSLOT))
                            {
                                InventoryManager.UnEquipItem(olditem.weaponName, 2);
                                InventoryManager.EquipItem(item.weaponName, 2);          
                            }
                            else if(result.gameObject.GetComponent<InventorySlot>().ItemIsForBoy == 1 && slotType == SlotType.INVENTORYSLOT)
                            {
                                InventoryManager.UnEquipItem(olditem.weaponName, 1);
                                InventoryManager.EquipItem(item.weaponName, 1);
                            }
                            else if(result.gameObject.GetComponent<InventorySlot>().ItemIsForBoy == 1 && ItemIsForBoy == 2)
                            {
                                InventoryManager.UnEquipItem(item.weaponName, 2);
                                InventoryManager.UnEquipItem(olditem.weaponName, 1);

                                InventoryManager.EquipItem(olditem.weaponName, 2);
                                InventoryManager.EquipItem(item.weaponName, 1);
                            }
                            else if (result.gameObject.GetComponent<InventorySlot>().ItemIsForBoy == 2 && ItemIsForBoy == 1)
                            {
                                InventoryManager.UnEquipItem(item.weaponName, 1);
                                InventoryManager.UnEquipItem(olditem.weaponName, 2);

                                InventoryManager.EquipItem(olditem.weaponName, 1);
                                InventoryManager.EquipItem(item.weaponName, 2);
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
