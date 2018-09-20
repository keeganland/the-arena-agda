using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : MonoBehaviour {


    private SphereCollider collider;
	// Use this for initialization
	void Start () 
    {
        collider = GetComponent<SphereCollider>();
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            ScreenFader.fadeOut();
        }
    }
}
