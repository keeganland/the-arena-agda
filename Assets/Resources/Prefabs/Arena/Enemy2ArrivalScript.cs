using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2ArrivalScript : MonoBehaviour {

    public ParticleSystem ParticleArrival;

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Arrival()
    {
        ParticleArrival.Play();
        Destroy(ParticleArrival.gameObject, 4f);
        anim.enabled = false;
    }

    public void ShakeCamera()
    {
        CameraShake cameraShake = GameObject.FindWithTag("CameraHolder").GetComponent<CameraShake>();
        StartCoroutine(cameraShake.LaserShake(1f, .15f));
    }
}
