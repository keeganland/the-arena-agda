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
		
	}

    public void StartNPCMovement()
    {
        NPCMovementAllowed = true;
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
