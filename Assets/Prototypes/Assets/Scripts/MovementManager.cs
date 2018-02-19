using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour {

    public Player_Movement boyMover;
    public Player_Movement girlMover;

    private bool playerIsBoy = false;
    private bool playerIsGirl = false;


    // Use this for initialization
    void Start() {
    }
    
    // Update is called once per frame
    void Update() {
        playerIsBoy = (boyMover.isTheBoy && boyMover.boyActive);
        playerIsGirl = (!(girlMover.isTheBoy) && !(girlMover.boyActive)); //redundant? could just use !playerIsBoy in normal gameplay. Is there any disadvantage to this? does this prevent any bugs?
    }

    // Call to stop whichever player happens to be controlled right now.
    public void StopPlayerMovement()
    {

        if (playerIsBoy)
        {
            boyMover.stopMoving = true;
            boyMover.CancelMovement();
        }
        else if (playerIsGirl)
        {
            girlMover.stopMoving = true;
            girlMover.CancelMovement();
        }


    }

    public void StartPlayerMovement()
    {
        boyMover.stopMoving = false;
        girlMover.stopMoving = false;
    }
}
