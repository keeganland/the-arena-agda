using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObjectDungeonFloor1Torches : InteractiveObjectAbstract {

    [SerializeField] int LightingTime = 2;

    // Use this for initialization
    new void Start () 
    {
        base.Start();
	}

    public override void DoAction(GameObject sender)
    {
        StartCoroutine("Action", sender);
    }

    public override IEnumerator Action(GameObject sender)
    {
        isCoroutineStarted = true;

        gameObject.GetComponent<Collider>().enabled = false;

        sender.GetComponent<SpriteInteractionSlider>().SetUpInteractionSlider("Light", (float)LightingTime);

        yield return new WaitForSeconds(LightingTime);
        GetComponent<TorchesBoss>().LightUp();

        isCoroutineStarted = false;
    }

    public override void ActionFunction(GameObject sender)
    {
        throw new System.NotImplementedException();
    }

    public override void CancelAction(GameObject sender)
    {
        isCoroutineStarted = false;
        StopCoroutine("Action");
        gameObject.GetComponent<Collider>().enabled = true;
        sender.GetComponent<SpriteInteractionSlider>().CancelInteractionSlider();
    }
}
