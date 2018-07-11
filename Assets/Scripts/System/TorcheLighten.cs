using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TorcheLighten : MonoBehaviour {

    public bool isLighten = true;

    Animator[] m_animChild;
    Collider m_collider;

	// Use this for initialization
	void Start () 
    {
        m_collider = GetComponent<Collider>();
        m_animChild = GetComponentsInChildren<Animator>();

        if(isLighten == true)
        {
            for (int i = 0; i < m_animChild.Length; i++)
            {
                    m_animChild[i].SetBool("Lighten", true);
            }

            if(m_collider)
            m_collider.enabled = false;
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (isLighten == false)
        {
            if (other.name == "Girl")
            {
                isLighten = true;

                for (int i = 0; i < m_animChild.Length; i++)
                {
                        m_animChild[i].SetBool("Lighten", true);
                }
                if(m_collider)
                m_collider.enabled = false;
            }
        }
    }

    public void OpenLight()
    {
        isLighten = true;

        for (int i = 0; i < m_animChild.Length; i++)
        {
                m_animChild[i].SetBool("Lighten", true);
        }
        if(m_collider)
            m_collider.enabled = false;
    }
}
