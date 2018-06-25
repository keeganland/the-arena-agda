using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightCave : MonoBehaviour {

    public PublicVariableHolderArenaEntrance publicVariableHolderArenaEntrance;

    private GameObject Girl;
    private GameObject Boy;

	void Start () {
        
        Girl = publicVariableHolderArenaEntrance.Girl;
        Boy = publicVariableHolderArenaEntrance.Boy;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if(gameObject.name == "PointLightGirl")
        {
            this.transform.position = new Vector3(Girl.transform.position.x, 16, Girl.transform.position.z);
        }
        if (gameObject.name == "PointLightBoy")
        {
            this.transform.position = new Vector3(Boy.transform.position.x, 16, Boy.transform.position.z);
        }
	}
}
