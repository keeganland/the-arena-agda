using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PublicVariableHolderArenaEntrance : MonoBehaviour {

    public PublicVariableHolderneverUnload publicVariableHolderNeverUnload;

    public GameObject Girl; 
    public GameObject Boy;


    private void Start()
    {
        publicVariableHolderNeverUnload = GameObject.Find("/PublicVariableHolderNeverUnload").GetComponent<PublicVariableHolderneverUnload>();
        Girl = publicVariableHolderNeverUnload.Girl;
        Boy = publicVariableHolderNeverUnload.Boy;
    }
}
