using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightCave : MonoBehaviour {

    public PublicVariableHolderArenaEntrance publicVariableHolderArenaEntrance;

    private GameObject boyPlayer;
    private GameObject girlPlayer;

    void Start () {

        girlPlayer = GameObject.FindGameObjectWithTag("Player/Girl");
        boyPlayer = GameObject.FindGameObjectWithTag("Player/Boy");
    }

    // Update is called once per frame
    void Update () 
    {
        if(gameObject.name == "PointLightGirl")
        {
            this.transform.position = new Vector3(girlPlayer.transform.position.x, 2, girlPlayer.transform.position.z);
        }
        if (gameObject.name == "PointLightBoy")
        {
            this.transform.position = new Vector3(boyPlayer.transform.position.x, 2, boyPlayer.transform.position.z);
        }
        if (gameObject.name == "PointLightGirlSprite")
        {
            this.transform.position = new Vector3(girlPlayer.transform.position.x, 16f, girlPlayer.transform.position.z);
        }
        if (gameObject.name == "PointLightBoySprite")
        {
            this.transform.position = new Vector3(boyPlayer.transform.position.x, 16f, boyPlayer.transform.position.z);
        }
	}
}
