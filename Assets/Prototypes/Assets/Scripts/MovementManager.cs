using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour {

    public Player_Movement boyMover;
    public Player_Movement girlMover;


    // Use this for initialization
    void Start() {
    }
    
    // Update is called once per frame
    void Update() {
    }

    // Call to stop whichever player happens to be controlled right now.
    public void StopPlayerMovement()
    {

        if (boyMover.isTheBoy && boyMover.boyActive)
        {
            boyMover.stopMoving = true;
            boyMover.CancelMovement();
        }
        else if (!(girlMover.isTheBoy) && !(girlMover.boyActive))
        {
            girlMover.stopMoving = true;
            girlMover.CancelMovement();
        }


    }

    public void StartPlayerMovement()
    {
        Debug.Log("the game should now be unfreezing the player");
        boyMover.stopMoving = false;
        girlMover.stopMoving = false;
    }
}
