using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObjectsPickUp : InteractiveObjectAbstract {

    public WeaponObject item;

    new void Start()
    {
        base.Start();
    }

    public override void ActionFunction(GameObject sender)
    {
        //Debug.Log("You aquiring : " + item.weaponName);
        InventoryManager.AquireItem(item.weaponName);
        //Debug.Log(SaveManager.StoredItems.Count + "Has been stored?");
        Destroy(this.gameObject, 1f);
    }

    public override void DoAction(GameObject sender)
    {
        //Debug.Log("Pickup theo object");
        ActionFunction(sender);
    }

    public override IEnumerator Action(GameObject sender)
    {
        throw new System.NotImplementedException();
    }

    public override void CancelAction(GameObject sender)
    {
        throw new System.NotImplementedException();
    }


}
