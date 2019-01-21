using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InteractiveObjectDungeonFloor1Exit : InteractiveObjectAbstract {

    public PublicVariableHolderArenaEntrance publicVariableHolderArenaEntrance;

    private GameObject boyPlayer;
    private GameObject girlPlayer;

    private GameObject ExitDoor;

    // Use this for initialization
    new void Start () {
        
        base.Start();

        girlPlayer = GameObject.FindGameObjectWithTag("Player/Girl");
        boyPlayer = GameObject.FindGameObjectWithTag("Player/Boy");

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

        boyPlayer.GetComponent<NavMeshAgent>().SetDestination(new Vector3(ExitDoor.transform.position.x, boyPlayer.transform.position.y, ExitDoor.transform.position.z));
        girlPlayer.GetComponent<NavMeshAgent>().SetDestination(new Vector3(ExitDoor.transform.position.x, girlPlayer.transform.position.y, ExitDoor.transform.position.z));

        publicVariableHolderNeverUnload.PlayerUI.SetActive(false);
    }

    public override void CancelAction(GameObject sender)
    {
        throw new System.NotImplementedException();
    }
}
