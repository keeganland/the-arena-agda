using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorcheLighten : MonoBehaviour {

    public bool isLighten = true;
    Animator m_anim;
    Animator[] m_animChild;
    Collider m_collider;

	// Use this for initialization
	void Start () 
    {
        m_collider = GetComponent<Collider>();
        m_animChild = GetComponentsInChildren<Animator>();

        if(isLighten == true){
            foreach (Animator anim in  m_animChild)
            {
                if (anim.GetBool("Lighten") == false)
                    anim.SetBool("Lighten", true);
            }
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (isLighten == false)
        {
            if (other.CompareTag("Player"))
            {
                isLighten = true;
                foreach (Animator anim in m_animChild)
                {
                    if (anim.GetBool("Lighten") == false)
                        anim.SetBool("Lighten", true);
                }
            }
        }
    }
}
