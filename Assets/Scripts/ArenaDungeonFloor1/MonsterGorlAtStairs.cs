using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGorlAtStairs : MonoBehaviour {

    private GameObject girlSpriteGameobject;

    private void Start()
    {
        girlSpriteGameobject = GameObject.FindGameObjectWithTag("Sprite/Girl");
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
        girlSpriteGameobject.GetComponent<SpriteScript2>().ForcePlayerRotation(4);
        yield return new WaitForSeconds(1f);
        girlSpriteGameobject.GetComponent<SpriteScript2>().ForcePlayerRotation(3);
        Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        EventManager.TriggerEvent("StartMoving");
    }
}
