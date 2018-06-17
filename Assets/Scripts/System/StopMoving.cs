using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StopMoving : MonoBehaviour {

    private BetterPlayer_Movement playerMovement;
    public PublicVariableHolderneverUnload publicVariableHolder;

    private void Awake()
    {
        playerMovement = GetComponent<BetterPlayer_Movement>();
    }

    private void OnEnable()
    {
        EventManager.StartListening("StopMoving", StopPlayerMovement);
        EventManager.StartListening("StartMoving", StartPlayerMovement);
        EventManager.StartListening("StopBoyMoving", StopPlayerMovementBoy);
        EventManager.StartListening("StartBoyMoving", StartPlayerMovementBoy);
        EventManager.StartListening("StopGirlMoving", StopPlayerMovementGirl);
        EventManager.StartListening("StartGirlMoving", StartPlayerMovementGirl);
    }

    private void OnDisable()
    {
        EventManager.StopListening("StopMoving", StopPlayerMovement);
        EventManager.StopListening("StartMoving", StartPlayerMovement);
        EventManager.StopListening("StopBoyMoving", StopPlayerMovementBoy);
        EventManager.StopListening("StartBoyMoving", StartPlayerMovementBoy);
        EventManager.StopListening("StopGirlMoving", StopPlayerMovementGirl);
        EventManager.StopListening("StartGirlMoving", StartPlayerMovementGirl);
    }

    void StopPlayerMovement()
    {
        playerMovement.stopMoving = true;
        publicVariableHolder.StopAllActions = true;
    }

    void StartPlayerMovement()
    {
        playerMovement.stopMoving = false;
        publicVariableHolder.StopAllActions = false;
    }
    void StopPlayerMovementGirl()
    {
        if (gameObject.name == "Girl")
        {
            playerMovement.stopMoving = true;
        }
    }

    void StartPlayerMovementGirl()
    {
        if (gameObject.name == "Girl")
        {
            playerMovement.stopMoving = false;
        }
    }
    void StopPlayerMovementBoy()
    {
        if (gameObject.name == "Boy")
        {
            playerMovement.stopMoving = true;
        }
    }

    void StartPlayerMovementBoy()
    {
        if (gameObject.name == "Boy")
        {
            playerMovement.stopMoving = false;
        }
    }
}
