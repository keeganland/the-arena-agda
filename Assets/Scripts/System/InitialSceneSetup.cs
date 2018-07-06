using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialSceneSetup : MonoBehaviour {

	public PublicVariableHolderArenaEntrance publicArenaEntrance;

	public float MainCameraFieldOfViewMin = 9;
	public float MainCameraFieldOfViewMax = 12;

    private GameObject Boy;
    private GameObject Girl;

    private GameObject SpawnPosBoy;
    private GameObject SpawnPosGirl;

	private GameObject MainCamera;


    // Use this for initialization
    void Start()
    {
        Boy = publicArenaEntrance.Boy;
        Girl = publicArenaEntrance.Girl;

        SpawnPosBoy = publicArenaEntrance.SpawnPosBoy;
        SpawnPosGirl = publicArenaEntrance.SpawnPosGirl;

        Boy.transform.position = SpawnPosBoy.transform.position;
        Girl.transform.position = SpawnPosGirl.transform.position;

        Boy.GetComponent<BetterPlayer_Movement>().isCombat = false;
        Girl.GetComponent<BetterPlayer_Movement>().isCombat = false;

		MainCamera = publicArenaEntrance.MainCamera;    

		MainCamera.GetComponent<BetterCameraFollow>()._FieldOfViewMin = MainCameraFieldOfViewMin;
		MainCamera.GetComponent<BetterCameraFollow>()._FieldOfViewMax = MainCameraFieldOfViewMax;
    }

}
