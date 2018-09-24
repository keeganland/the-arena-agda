using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenFader : MonoBehaviour {

    Animator m_anim;
    bool m_isFading;

    public static ScreenFader screenFader;

    public static ScreenFader Instance
    {
        get
        {
            if (!screenFader)
            {
                screenFader = FindObjectOfType(typeof(ScreenFader)) as ScreenFader;

                if (!screenFader)
                {
                    Debug.LogError("There needs to be one active EventManger script on a GameObject in your scene.");
                }
                else
                {
                    screenFader.Init();
                }
            }
            return screenFader;
        }
    }

    void Init()
    {
        m_anim = GetComponent<Animator>();  
    }

    private void Awake()
    {
        m_anim = GetComponent<Animator>();
        screenFader = this;
    }

    static public void fadeIn()
    {
        screenFader.StartCoroutine("FadeIn");
    }

    static public void fadeOut()
    {
        screenFader.StartCoroutine("FadeOut");
    }

    IEnumerator FadeIn()
    {
        m_isFading = true;
        m_anim.SetBool("FadeIn", true);
        m_anim.SetBool("FadeOut", false);
        while (m_isFading) yield return null;
    }

    IEnumerator FadeOut()
    {
        m_isFading = true;
        m_anim.SetBool("FadeOut", true);
        m_anim.SetBool("FadeIn", false);
        while (m_isFading) yield return null;
    }

    void AnimationComplete()
    {
        m_isFading = false;
        m_anim.SetBool("FadeIn", false);
        m_anim.SetBool("FadeOut", false);
    }

    public static void PlayLaserDevastation()
    {
        screenFader.Laser();
    }

    private void Laser()
    {
        m_anim.Play("LaserAttackDevastation");
    }

}
