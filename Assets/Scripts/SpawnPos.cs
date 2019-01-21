using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPos : MonoBehaviour {

    public PublicVariableHolderArenaEntrance publicArenaEntrance;

    private GameObject boyPlayer;
    private GameObject girlPlayer; 

    private GameObject SpawnPosBoy;
    private GameObject SpawnPosGirl;


	// Use this for initialization
	void Start () 
    {
        girlPlayer = GameObject.FindGameObjectWithTag("Player/Girl");
        boyPlayer = GameObject.FindGameObjectWithTag("Player/Boy");

        SpawnPosBoy = publicArenaEntrance.SpawnPosBoy;
        SpawnPosGirl = publicArenaEntrance.SpawnPosGirl;

        boyPlayer.transform.position = SpawnPosBoy.transform.position;
        girlPlayer.transform.position = SpawnPosGirl.transform.position;

        boyPlayer.GetComponent<BetterPlayer_Movement>().IsCombat = false;
        girlPlayer.GetComponent<BetterPlayer_Movement>().IsCombat = false;
    }

}
