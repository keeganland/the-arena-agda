using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StopMoving : MonoBehaviour {

    private BetterPlayer_Movement playerMovement;

    private void Awake()
    {
        playerMovement = GetComponent<BetterPlayer_Movement>();
    }

    private void OnEnable()
    {
        EventManager.StartListening("StopMoving", StopPlayerMovement);
        EventManager.StartListening("StartMoving", StartPlayerMovement);
    }

    private void OnDisable()
    {
        EventManager.StopListening("StopMoving", StopPlayerMovement);
        EventManager.StopListening("StartMoving", StartPlayerMovement);
    }

    void StopPlayerMovement()
    {
        playerMovement.stopMoving = true;
    }

    void StartPlayerMovement()
    {
        playerMovement.stopMoving = false;
    }


}
