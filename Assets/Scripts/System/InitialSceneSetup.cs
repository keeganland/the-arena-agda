using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InitialSceneSetup : MonoBehaviour {

    SaveManager saveManager;

	public PublicVariableHolderArenaEntrance publicArenaEntrance;

	public float MainCameraFieldOfViewMin = 9;
	public float MainCameraFieldOfViewMax = 12;

    private NavMeshAgent BoyNav;
    private NavMeshAgent GirlNav;

    private GameObject Boy;
    private GameObject Girl;

    private GameObject SpawnPosBoy;
    private GameObject SpawnPosGirl;

    private GameObject ReturnFromArenaGirlPos;
    private GameObject ReturnFromArenaBoyPos;

	private GameObject MainCamera;

    private void Awake()
    {
        saveManager = FindObjectOfType<SaveManager>();
    }
    // Use this for initialization
    void Start()
    {
        Boy = publicArenaEntrance.Boy;
        Girl = publicArenaEntrance.Girl;

        BoyNav = Boy.GetComponent<NavMeshAgent>();
        GirlNav = Girl.GetComponent<NavMeshAgent>();

        BoyNav.enabled = false;
        GirlNav.enabled = false;

        ReturnFromArenaBoyPos = publicArenaEntrance.ReturnFromArenaBoyPos;
        ReturnFromArenaGirlPos = publicArenaEntrance.ReturnFromArenaGirlPos;

        if (ReturnFromArenaGirlPos != null && ReturnFromArenaBoyPos != null && saveManager.returnFromArena)
        {
             SpawnPosBoy = publicArenaEntrance.ReturnFromArenaBoyPos;
             SpawnPosGirl = publicArenaEntrance.ReturnFromArenaGirlPos;
        }
        else
        {
            SpawnPosBoy = publicArenaEntrance.SpawnPosBoy;
            SpawnPosGirl = publicArenaEntrance.SpawnPosGirl;
        }

        Boy.transform.position = new Vector3(SpawnPosBoy.transform.position.x, transform.position.y, SpawnPosBoy.transform.position.z);
        Girl.transform.position = new Vector3(SpawnPosGirl.transform.position.x, transform.position.y, SpawnPosGirl.transform.position.z);

        BoyNav.enabled = true;
        GirlNav.enabled = true;

        if (publicArenaEntrance.ReturnFromArena && saveManager.returnFromArena)
            GirlNav.SetDestination(new Vector3(publicArenaEntrance.ReturnFromArena.transform.position.x, Girl.transform.position.y, publicArenaEntrance.ReturnFromArena.transform.position.z));
        
        Boy.GetComponent<BetterPlayer_Movement>().isCombat = false;
        Girl.GetComponent<BetterPlayer_Movement>().isCombat = false;

		MainCamera = publicArenaEntrance.MainCamera;    

		MainCamera.GetComponent<BetterCameraFollow>()._FieldOfViewMin = MainCameraFieldOfViewMin;
		MainCamera.GetComponent<BetterCameraFollow>()._FieldOfViewMax = MainCameraFieldOfViewMax;

        EventManager.TriggerEvent("setup");

        publicArenaEntrance.publicVariableHolderNeverUnload.PlayerUI.SetActive(false);

        publicArenaEntrance.publicVariableHolderNeverUnload.fader.StartCoroutine("FadeIn");
    }

}
