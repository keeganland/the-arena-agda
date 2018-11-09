using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InteractiveObjectDungeonFloor1Exit : InteractiveObjectAbstract {

    public PublicVariableHolderArenaEntrance publicVariableHolderArenaEntrance;

    private GameObject Boy;
    private GameObject Girl;

    private GameObject ExitDoor;

    // Use this for initialization
    new void Start () {
        
        base.Start();

        Boy = publicVariableHolderArenaEntrance.Boy;
        Girl = publicVariableHolderArenaEntrance.Girl;

        ExitDoor = publicVariableHolderArenaEntrance.ExitDoor;
	}
	
    public override void DoAction(GameObject sender)
    {
        ActionFunction(sender);
    }

    public override IEnumerator Action(GameObject sender)
    {
        throw new System.NotImplementedException();
    }

    public override void ActionFunction(GameObject sender)
    {
        EventManager.TriggerEvent("InCombat");
        EventManager.TriggerEvent("StopMoving");

        Boy.GetComponent<NavMeshAgent>().SetDestination(new Vector3(ExitDoor.transform.position.x, Boy.transform.position.y, ExitDoor.transform.position.z));
        Girl.GetComponent<NavMeshAgent>().SetDestination(new Vector3(ExitDoor.transform.position.x, Girl.transform.position.y, ExitDoor.transform.position.z));

        publicVariableHolderNeverUnload.PlayerUI.SetActive(false);
    }

    public override void CancelAction(GameObject sender)
    {
        throw new System.NotImplementedException();
    }
}
