using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeRoom : MonoBehaviour {

    Collider m_collider;
    Animator[] m_anim;
    ParticleSystem[] m_particle;
    bool m_isFading;

	void Start () {

        m_collider = GetComponent<Collider>();
        m_anim = GetComponentsInChildren<Animator>();
        m_particle = GetComponentsInChildren<ParticleSystem>();
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
               FadeRoomIn();
            for (int i = 0; i < m_particle.Length; i++)
            {
                m_particle[i].Play();  
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
                FadeRoomOut();

            for (int i = 0; i < m_particle.Length; i++)
            {
                m_particle[i].Stop();
            }

        }
    }

    private void FadeRoomIn()
    {
        for (int i = 0; i < m_anim.Length; i++)
        {
            if (m_anim[i])
            {
                m_anim[i].SetBool("FadeOut", false);
                m_anim[i].SetBool("FadeIn", true);
            }
        }
    }

    private void FadeRoomOut()
    {
        for (int i = 0; i < m_anim.Length; i++)
        {
            if (m_anim[i])
            {
                m_anim[i].SetBool("FadeOut", true);
                m_anim[i].SetBool("FadeIn", false);
            }
        }
    }
}
