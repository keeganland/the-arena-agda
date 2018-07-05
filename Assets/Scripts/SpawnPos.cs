using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPos : MonoBehaviour {

    public PublicVariableHolderArenaEntrance publicArenaEntrance;

    private GameObject Boy;
    private GameObject Girl; 

    private GameObject SpawnPosBoy;
    private GameObject SpawnPosGirl;


	// Use this for initialization
	void Start () 
    {
        Boy = publicArenaEntrance.Boy;
        Girl = publicArenaEntrance.Girl;

        SpawnPosBoy = publicArenaEntrance.SpawnPosBoy;
        SpawnPosGirl = publicArenaEntrance.SpawnPosGirl;

        Boy.transform.position = SpawnPosBoy.transform.position;
        Girl.transform.position = SpawnPosGirl.transform.position;

        Boy.GetComponent<BetterPlayer_Movement>().isCombat = false;
        Girl.GetComponent<BetterPlayer_Movement>().isCombat = false;
    }

}
