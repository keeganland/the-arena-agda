using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameObject.FindObjectOfType< ColliderDarknessDamage>().InLight(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject.FindObjectOfType<ColliderDarknessDamage>().ExitLight(other.gameObject);
    }
}
