using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnAnimationComplete : MonoBehaviour {

    private Animator m_anim;
    private Text m_text;

    void Start()
    {
        m_anim = GetComponent<Animator>();
        m_text = GetComponent<Text>();
    }

    public void OnanimationComplete()
    {
        m_text.text = null;
    }
}
