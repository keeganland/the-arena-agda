using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBoy : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("BoyShield " + other.name);
        if(other.tag == "Projectile")
        {
            other.enabled=false;
        }
    }
}
