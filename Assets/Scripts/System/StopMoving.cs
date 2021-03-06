﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
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
        EventManager.StartListening("GirlInCombat", GirlInCombat);

        EventManager.StartListening("ResetTargets", ResetTargets);
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
        EventManager.StopListening("GirlInCombat", GirlInCombat);

        EventManager.StopListening("ResetTargets", ResetTargets);
    }

    void StopPlayerMovement()
    {
        playerMovement.StopMoving = true;
        publicVariableHolder.StopAllActions = true;
        Boy.UndoCurTarget();
        Girl.UndoCurTarget();
        Boy.GetComponent<NavMeshAgent>().SetDestination(Boy.transform.position);
        Girl.GetComponent<NavMeshAgent>().SetDestination(Girl.transform.position);
    }

    void StartPlayerMovement()
    {
        playerMovement.StopMoving = false;
        publicVariableHolder.StopAllActions = false;

    }
    void StopPlayerMovementGirl()
    {
        if (gameObject.name == "Girl")
        {
            playerMovement.StopMoving = true;
            Girl.UndoCurTarget(); 
            Girl.GetComponent<NavMeshAgent>().SetDestination(Girl.transform.position);
        }
    }

    void StartPlayerMovementGirl()
    {
        if (gameObject.name == "Girl")
        {
            playerMovement.StopMoving = false;
        }
    }
    void StopPlayerMovementBoy()
    {
        if (gameObject.name == "Boy")
        {
            playerMovement.StopMoving = true;
            Boy.UndoCurTarget();
            Boy.GetComponent<NavMeshAgent>().SetDestination(Boy.transform.position);
        }
    }

    void StartPlayerMovementBoy()
    {
        if (gameObject.name == "Boy")
        {
            playerMovement.StopMoving = false;
        }
    }

    void NotInCombat()
    {
        this.GetComponent<BetterPlayer_Movement>().IsCombat = false;
        this.GetComponentInChildren<RangeChecker>().ResetList();
        this.GetComponent<BetterPlayer_Movement>().CancelParticles();
        InventoryManager.ShowInventoryIcon();
    }
    void InCombat()
    {
        this.GetComponent<BetterPlayer_Movement>().IsCombat = true;
        InventoryManager.HiddeInventoryIcon();
    }

    void GirlInCombat()
    {
        if(gameObject.name == "Girl")
        {
            GetComponent<BetterPlayer_Movement>().IsCombat = true;
        }
    }

    void ResetTargets()
    {
         Girl.UndoCurTarget();
         Boy.UndoCurTarget();
    }
}
