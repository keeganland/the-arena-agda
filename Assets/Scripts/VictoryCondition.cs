using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryCondition : MonoBehaviour {

    public GameObject boss;
    private bool youWon = false;
	
	// Update is called once per frame
	void Update () {
		if (boss.activeSelf == false && youWon == false)
        {
            youWon = true;
            Debug.Log("boss was killed!");
            EventManager.TriggerEvent("victory");
        }
	}
}
