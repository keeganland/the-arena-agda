using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour {

    InventoryManager inventory;

	// Use this for initialization
	void Start () 
    {
        inventory = InventoryManager.Instance;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void UpdateUI()
    {
        Debug.Log("UPDATING UI");
    }
}
