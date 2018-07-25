using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InitialSceneSetup : MonoBehaviour {

	public PublicVariableHolderArenaEntrance publicArenaEntrance;

	public float MainCameraFieldOfViewMin = 9;
	public float MainCameraFieldOfViewMax = 12;

    private NavMeshAgent BoyNav;
    private NavMeshAgent GirlNav;

    private GameObject Boy;
    private GameObject Girl;

    private GameObject SpawnPosBoy;
    private GameObject SpawnPosGirl;

	private GameObject MainCamera;


    // Use this for initialization
    void Awake()
    {
        Boy = publicArenaEntrance.Boy;
        Girl = publicArenaEntrance.Girl;

        BoyNav = Boy.GetComponent<NavMeshAgent>();
        GirlNav = Girl.GetComponent<NavMeshAgent>();

        BoyNav.enabled = false;
        GirlNav.enabled = false;

        SpawnPosBoy = publicArenaEntrance.SpawnPosBoy;
        SpawnPosGirl = publicArenaEntrance.SpawnPosGirl;

        Debug.Log(Boy.transform.position);
        Debug.Log(SpawnPosBoy.transform.position);

        Boy.transform.position = SpawnPosBoy.transform.position;
        Girl.transform.position = SpawnPosGirl.transform.position;

        BoyNav.enabled = true;
        GirlNav.enabled = true;

        Boy.GetComponent<BetterPlayer_Movement>().isCombat = false;
        Girl.GetComponent<BetterPlayer_Movement>().isCombat = false;

		MainCamera = publicArenaEntrance.MainCamera;    

		MainCamera.GetComponent<BetterCameraFollow>()._FieldOfViewMin = MainCameraFieldOfViewMin;
		MainCamera.GetComponent<BetterCameraFollow>()._FieldOfViewMax = MainCameraFieldOfViewMax;
    }

}
