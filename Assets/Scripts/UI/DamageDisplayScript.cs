using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageDisplayScript : MonoBehaviour {

    [SerializeField]
    public Text m_text;
    public Animation m_anim;

 
    public Color _Color;
	// Use this for initialization

	void Start ()
    {
        m_anim = GetComponentInChildren<Animation>();
	}
	
    public void GetDamageText(Color color, int text)
    {
        _Color = color;
        if (m_text)
        {
            m_text.color = color;
            m_text.text = text.ToString();
        }
        if(m_anim)
        m_anim.PlayQueued("DamageDisplay", QueueMode.PlayNow);
    }
}
