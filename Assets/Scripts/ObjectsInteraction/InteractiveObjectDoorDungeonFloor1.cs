﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InteractiveObjectDoorDungeonFloor1 : InteractiveObjectAbstract {

    private GameObject DungeonDoorFloor1Boy;
    private GameObject DungeonDoorFloor1Girl;
    private GameObject Boy;
    private GameObject Girl;

    public PublicVariableHolderArenaEntrance publicVariableHolderArenaEntrance;

    public GameObject _DestroyEffect;
    public GameObject Door;

    // Use this for initialization
    new void Start()
    {
        base.Start();

        Boy = publicVariableHolderArenaEntrance.Boy;
        Girl = publicVariableHolderArenaEntrance.Girl;

        DungeonDoorFloor1Boy = publicVariableHolderArenaEntrance.DungeonDoorFloor1Boy;
        DungeonDoorFloor1Girl = publicVariableHolderArenaEntrance.DungeonDoorFloor1Girl;
    }

    public override void DoAction(GameObject sender)
    {
        StartCoroutine("Action", sender);
    }
       
    public override IEnumerator Action(GameObject sender)
    {
        EventManager.TriggerEvent("InCombat");
        EventManager.TriggerEvent("StopMoving");

        Vector3 BoyNewPos = new Vector3(DungeonDoorFloor1Boy.transform.position.x, Boy.transform.position.y, DungeonDoorFloor1Boy.transform.position.z);
        Vector3 GirlNewPos = new Vector3(DungeonDoorFloor1Girl.transform.position.x, Girl.transform.position.y, DungeonDoorFloor1Girl.transform.position.z);

        Boy.GetComponent<NavMeshAgent>().SetDestination(DungeonDoorFloor1Boy.transform.position);
        Girl.GetComponent<NavMeshAgent>().SetDestination(DungeonDoorFloor1Girl.transform.position);

        while (Boy.transform.position != BoyNewPos && Girl.transform.position != GirlNewPos)
        {
            yield return null;
        }

        yield return new WaitForSeconds(.5f);
        Boy.GetComponent<BetterPlayer_Movement>().SetCurTarget(this.gameObject);
        Debug.Log(currentHealth);
        yield return new WaitUntil(() => currentHealth <= 0);
        Debug.Log("It did work");
        currentHealth = 0;

        if (_DestroyEffect)
        {
            ScreenFader.PlayLaserDevastation();
            yield return new WaitForSeconds(.5f);
            GameObject sfx = Instantiate(_DestroyEffect, transform.position, Quaternion.identity);
            Destroy(sfx, 5f);
        }

        SpriteRenderer[] DoorSprite = Door.GetComponentsInChildren<SpriteRenderer>();
        for (int i = 0; i < DoorSprite.Length; i++)
        {
            DoorSprite[i].enabled = false;
        }

        Destroy(Door);

        EventManager.TriggerEvent("NotInCombat");
    }

    public override void ActionFunction(GameObject sender)
    {
        throw new System.NotImplementedException();
    }

    public override void CancelAction(GameObject sender)
    {
        throw new System.NotImplementedException();
    }
}