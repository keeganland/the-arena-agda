using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGorlAtStairs : MonoBehaviour {

    public PublicVariableHolderArenaEntrance publicVariableHolderArenaEntrance;

    private GameObject Bella;

    private void Start()
    {
        Bella = publicVariableHolderArenaEntrance.publicVariableHolderNeverUnload._GirlSpriteGameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Girl")
        {
            StartCoroutine("MonsterEvent");
        }
    }

    private IEnumerator MonsterEvent()
    {
        EventManager.TriggerEvent("StopMoving");
        CameraShake cameraShake = GameObject.FindWithTag("CameraHolder").GetComponent<CameraShake>();
        StartCoroutine(cameraShake.LaserShake(4f, .15f));
        yield return new WaitForSeconds(4f);
        Bella.GetComponent<SpriteScript2>().ForcePlayerRotation(4);
        yield return new WaitForSeconds(1f);
        Bella.GetComponent<SpriteScript2>().ForcePlayerRotation(3);
        Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        EventManager.TriggerEvent("StartMoving");
    }
}
