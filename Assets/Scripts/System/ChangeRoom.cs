using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeRoom : MonoBehaviour {

    Collider m_collider;
    Animator m_anim;
    bool m_isFading;

	void Start () {

        m_collider = GetComponent<Collider>();
        m_anim = GetComponent<Animator>();
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            FadeRoomIn();

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
           FadeRoomOut();
        }
    }

    private void FadeRoomIn()
    {
        m_anim.SetBool("FadeOut", false);
        m_anim.SetBool("FadeIn", true);
    }

    private void FadeRoomOut()
    {
        m_anim.SetBool("FadeIn", false);
        m_anim.SetBool("FadeOut", true);
    }
}
