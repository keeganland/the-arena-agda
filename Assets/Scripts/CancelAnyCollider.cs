using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CancelAnyCollider : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Girl")
        {
            GetComponent<Collider>().enabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Girl")
        {
            GetComponent<Collider>().enabled = true;
        }
    }
}
