using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PublicVariableHolderArenaEntrance : MonoBehaviour {

    public PublicVariableHolderneverUnload publicVariableHolderNeverUnload;


    public GameObject Girl; 
    public GameObject Boy;

    [Header("GeneralCharacteristics")]
    public GameObject MainCamera;

    [Header("SpawnPosition")]
    public GameObject SpawnPosBoy;
    public GameObject SpawnPosGirl;

    [Header("InteractiveObjectsEvents")]
    public GameObject DungeonDoorFloor1Boy;
    public GameObject DungeonDoorFloor1Girl;

    private void Awake()
    {
        publicVariableHolderNeverUnload = GameObject.Find("/PublicVariableHolderNeverUnload").GetComponent<PublicVariableHolderneverUnload>();
        Girl = publicVariableHolderNeverUnload.Girl;
        Boy = publicVariableHolderNeverUnload.Boy;
        MainCamera = publicVariableHolderNeverUnload.MainCamera;
    }
}
