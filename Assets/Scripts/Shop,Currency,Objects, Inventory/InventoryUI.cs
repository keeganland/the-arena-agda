using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour {


    public Transform itemsParent;

    InventorySlot[] slots;
	// Use this for initialization
	void Awake () 
    {
        InventoryManager.onItemChangedCallback += UpdateStoredUI;
        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
	}

    void UpdateStoredUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].ClearSlot();
        }

        //Debug.Log("UPDATING UI");
        for (int i = 0; i < SaveManager.Instance.StoredItems.Count; i++)
        {
            if (!SaveManager.Instance.EquipedItemsBoy.Contains(SaveManager.Instance.StoredItems[i]) && !SaveManager.Instance.EquipedItemsGirl.Contains(SaveManager.Instance.StoredItems[i]))
            {
                int b = 1;
                //Debug.Log(slots.Length + " how many slots?");
                for (int a = 0; a < slots.Length; a++)
                {
                    //Debug.Log("Slot number " + a + " is empty");
                    if (slots[a].isEmpty == true && b ==1)
                    {
                        slots[a].AddItem(SaveManager.Instance.StoredItems[i]);
                        b--;
                    }
                }
            }
        }
    }
}
