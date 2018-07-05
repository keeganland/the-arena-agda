using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StopMoving : MonoBehaviour {

    private BetterPlayer_Movement playerMovement;
    public PublicVariableHolderneverUnload publicVariableHolder;

    private BetterPlayer_Movement Boy;
    private BetterPlayer_Movement Girl;

    private void Awake()
    {
        playerMovement = GetComponent<BetterPlayer_Movement>();
    }

    private void Start()
    {
        Boy = publicVariableHolder._BoyMovementScript;
        Girl = publicVariableHolder._GirlMovementScript;
    }

    private void OnEnable()
    {
        EventManager.StartListening("StopMoving", StopPlayerMovement);
        EventManager.StartListening("StartMoving", StartPlayerMovement);
        EventManager.StartListening("StopBoyMoving", StopPlayerMovementBoy);
        EventManager.StartListening("StartBoyMoving", StartPlayerMovementBoy);
        EventManager.StartListening("StopGirlMoving", StopPlayerMovementGirl);
        EventManager.StartListening("StartGirlMoving", StartPlayerMovementGirl);

        EventManager.StartListening("NotInCombat", NotInCombat);
        EventManager.StartListening("InCombat", InCombat);
    }

    private void OnDisable()
    {
        EventManager.StopListening("StopMoving", StopPlayerMovement);
        EventManager.StopListening("StartMoving", StartPlayerMovement);
        EventManager.StopListening("StopBoyMoving", StopPlayerMovementBoy);
        EventManager.StopListening("StartBoyMoving", StartPlayerMovementBoy);
        EventManager.StopListening("StopGirlMoving", StopPlayerMovementGirl);
        EventManager.StopListening("StartGirlMoving", StartPlayerMovementGirl);

        EventManager.StopListening("NotInCombat", NotInCombat);
        EventManager.StopListening("InCombat", InCombat);
    }

    void StopPlayerMovement()
    {
        playerMovement.stopMoving = true;
        publicVariableHolder.StopAllActions = true;
        Boy.UndoCurTarget();
        Girl.UndoCurTarget();
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
            Girl.UndoCurTarget();
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
            Boy.UndoCurTarget();
        }
    }

    void StartPlayerMovementBoy()
    {
        if (gameObject.name == "Boy")
        {
            playerMovement.stopMoving = false;
        }
    }

    void NotInCombat()
    {
        this.GetComponent<BetterPlayer_Movement>().isCombat = false;
    }
    void InCombat()
    {
        this.GetComponent<BetterPlayer_Movement>().isCombat = true;
    }
}
