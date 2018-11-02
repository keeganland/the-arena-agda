using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightAnimationComplete : MonoBehaviour {

    Animator anim;

    public float intensity = 1;

	void Start ()
    {
        anim = GetComponent<Animator>();
	}

    public void OnAnimationCompleteLights()
    {
        anim.SetBool("TurnOn", false);
        anim.SetBool("TurnOff", false);
    }
}
