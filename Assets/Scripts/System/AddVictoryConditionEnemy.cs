using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddVictoryConditionEnemy : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        VictoryReferee.AddEnemiesCount(this.gameObject);
	}
	
	// Update is called once per frame
	void OnDestroy ()
    {
        VictoryReferee.RemoveEnemy(this.gameObject);
	}
}
