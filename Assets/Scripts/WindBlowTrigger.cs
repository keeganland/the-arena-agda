using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindBlowTrigger : MonoBehaviour {

    public Animator _Anim;

    private void OnTriggerEnter (Collider collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            _Anim.CrossFadeInFixedTime("Leaves", 0.1f);
        }
    }

    private void AnimationComplete()
    {
    }
}
