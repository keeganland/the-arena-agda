using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeRoom : MonoBehaviour {

    Collider m_collider;
    Animator[] m_anim;
    bool m_isFading;

	void Start () {

        m_collider = GetComponent<Collider>();
        m_anim = GetComponentsInChildren<Animator>();
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
        for (int i = 0; i < m_anim.Length; i++)
        {
            m_anim[i].SetBool("FadeOut", false);
            m_anim[i].SetBool("FadeIn", true);
        }
    }

    private void FadeRoomOut()
    {
        for (int i = 0; i < m_anim.Length; i++)
        {
            m_anim[i].SetBool("FadeOut", true);
            m_anim[i].SetBool("FadeIn", false);
        }
    }
}
