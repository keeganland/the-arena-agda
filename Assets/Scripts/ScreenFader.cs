using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenFader : MonoBehaviour {


    Animator m_anim;
    bool m_isFading;


	void Start () 
    {
        m_anim = GetComponent<Animator>();	
	}

    public IEnumerator FadeIn()
    {
        m_isFading = true;
        m_anim.SetBool("FadeIn", true);
        while (m_isFading) yield return null;
    }

    public IEnumerator FadeOut()
    {
        m_isFading = true;
        m_anim.SetBool("FadeOut", true);
        while (m_isFading) yield return null;
    }

    void AnimationComplete()
    {
        m_isFading = false;
    }

}
