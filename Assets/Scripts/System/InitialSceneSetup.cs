using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

abstract public class InitialSceneSetup : MonoBehaviour {

    public SaveManager saveManager;

	public PublicVariableHolderArenaEntrance publicArenaEntrance;
    public PublicVariableHolderneverUnload publicVariableHolderneverUnload;

	public float MainCameraFieldOfViewMin = 9;
	public float MainCameraFieldOfViewMax = 12;

    public NavMeshAgent BoyNav;
    public NavMeshAgent GirlNav;

    public GameObject boyPlayer;
    public GameObject girlPlayer;

	public GameObject MainCamera;

    protected void Awake()
    {
        saveManager = SaveManager.Instance;
        publicVariableHolderneverUnload = FindObjectOfType<PublicVariableHolderneverUnload>().GetComponent<PublicVariableHolderneverUnload>();
    }
    // Use this for initialization
    protected void Start()
    {
        publicArenaEntrance.publicVariableHolderNeverUnload.PlayerUI.SetActive(false);

        girlPlayer = GameObject.FindGameObjectWithTag("Player/Girl");
        boyPlayer = GameObject.FindGameObjectWithTag("Player/Boy");

        BoyNav = boyPlayer.GetComponent<NavMeshAgent>();
        GirlNav = girlPlayer.GetComponent<NavMeshAgent>();

        BoyNav.enabled = false;
        GirlNav.enabled = false;
      
    }
}
