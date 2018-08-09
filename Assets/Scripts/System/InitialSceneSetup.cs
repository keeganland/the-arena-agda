using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

abstract public class InitialSceneSetup : MonoBehaviour {

    public SaveManager saveManager;

	public PublicVariableHolderArenaEntrance publicArenaEntrance;

	public float MainCameraFieldOfViewMin = 9;
	public float MainCameraFieldOfViewMax = 12;

    public NavMeshAgent BoyNav;
    public NavMeshAgent GirlNav;

    public GameObject Boy;
    public GameObject Girl;

	public GameObject MainCamera;

    private void Awake()
    {
        saveManager = FindObjectOfType<SaveManager>();
    }
    // Use this for initialization
    protected void Start()
    {
        Boy = publicArenaEntrance.Boy;
        Girl = publicArenaEntrance.Girl;

        BoyNav = Boy.GetComponent<NavMeshAgent>();
        GirlNav = Girl.GetComponent<NavMeshAgent>();

        BoyNav.enabled = false;
        GirlNav.enabled = false;
    }
}
