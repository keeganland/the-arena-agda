using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryTester : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("v"))
        {
            EventManager.TriggerEvent("victory");
        }
	}
}
