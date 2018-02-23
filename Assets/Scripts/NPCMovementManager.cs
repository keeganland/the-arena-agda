using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovementManager : MonoBehaviour {

    private bool NPCMovementAllowed = true;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        //Debug.Log(isNPCMovementAllowed());
		
	}

    public void StartNPCMovement()
    {
        NPCMovementAllowed = true;
        Debug.Log("StartNPCMovement was called");
        Debug.Log("NPCMovementAllowed == " + NPCMovementAllowed);
    }

    public void StopNPCMovement()
    {
        NPCMovementAllowed = false;
    }

    public bool isNPCMovementAllowed()
    {
        return NPCMovementAllowed;
    }
}
