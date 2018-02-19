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
        //Debug.Log("boyMover.stopMoving = " + boyMover.stopMoving);
        //Debug.Log("girlMover.stopMoving = " + girlMover.stopMoving);
    }

    // Call to stop whichever player happens to be controlled right now.
    public void StopPlayerMovement()
    {

        if (boyMover.m_currentPlayer)
        {
            boyMover.stopMoving = true;
            Debug.Log("boyMover.stopMoving set to " + boyMover.stopMoving);

        }

        if (girlMover.m_currentPlayer)
        {
            girlMover.stopMoving = true;
            Debug.Log("girlMover.stopMoving set to " + girlMover.stopMoving);
        }

        Debug.Log("boyMover.m_CurrentPlayer = " + boyMover.m_currentPlayer);
        Debug.Log("girlMover.m_CurrentPlayer = " + girlMover.m_currentPlayer);

    }

    public void StartPlayerMovement()
    {
        Debug.Log("the game should now be unfreezing the player");
        boyMover.stopMoving = false;
        girlMover.stopMoving = false;
    }
}
