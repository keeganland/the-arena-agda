using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteInteractionSlider : MonoBehaviour {

    public Slider InteractionSlider;
    public Text Casting;
    public Text CastingTime;

    private float m_castTime;
    private float m_totalCD;
    private bool m_interact;


	// Update is called once per frame
	void FixedUpdate () {
        
        if (m_interact)
        {
            if (m_castTime < m_totalCD)
            {
                m_castTime += Time.fixedDeltaTime;
                InteractionSlider.value = m_castTime / m_totalCD;
                CastingTime.text = System.Math.Round((float)(m_totalCD - m_castTime), 1).ToString();
            }
            else
            {
                CancelInteractionSlider();
            }
        }
	}

    public void SetUpInteractionSlider(string casting, float totalTime)
    {
        Casting.text = casting;
        m_totalCD = totalTime;
        m_castTime = 0;
        InteractionSlider.gameObject.SetActive(true);
        m_interact = true;
    }

    public void CancelInteractionSlider()
    {
        InteractionSlider.value = 0;
        InteractionSlider.gameObject.SetActive(false);
        m_interact = false;
    }
}
