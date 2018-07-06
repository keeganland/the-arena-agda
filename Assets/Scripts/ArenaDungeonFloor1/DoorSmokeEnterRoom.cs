using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSmokeEnterRoom : MonoBehaviour {

    private ParticleSystem particleSystem;
    private Collider collider;
	// Use this for initialization
	void Start () {
        particleSystem = GetComponent<ParticleSystem>();
        collider = GetComponent<Collider>();
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            particleSystem.Play();
            Destroy(this, 5f);
        }
    }
}
