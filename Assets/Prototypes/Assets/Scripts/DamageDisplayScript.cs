using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageDisplayScript : MonoBehaviour {

    [SerializeField]
    public Text m_text;
    private Animator m_anim;

    public Color _Color;
	// Use this for initialization

	void Start () {
        m_anim = GetComponentInChildren<Animator>();
	}
	
    public void GetDamageText(Color color, int text)
    {
        _Color = color;
        m_text.color = color;
        m_text.text = text.ToString();
        m_anim.SetBool("DisplayDamage", true);
    }
}
