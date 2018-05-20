using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryCondition : MonoBehaviour {

    public GameObject boss;
	
	// Update is called once per frame
	void Update () {
		if (boss.activeSelf == false)
        {
            Debug.Log("boss was killed!");
            EventManager.TriggerEvent("victory");
        }
	}
}
