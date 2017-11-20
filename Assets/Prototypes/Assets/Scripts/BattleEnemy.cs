using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEnemy : MonoBehaviour {

    private float m_timer = 0;

    public List<GameObject> Attacks;
    public List<float> _Attacktimers;
    public float _CooldownTimer;
	
	// Update is called once per frame
	void Update ()
    {
        int n = 0; 
        while (m_timer <= _CooldownTimer)
        {
            for (int i = 0; i < Attacks.Count; i++)
            {
                if (m_timer >= _Attacktimers[i] && Attacks[i].GetComponent<PlaneBlink>().isCoroutineStarted == false && n<i)
                {
                    Attacks[i].GetComponent<PlaneBlink>().StartCoroutine("Blink");
                    n = i;
                }
                m_timer += Time.deltaTime;
            }         
        }
        m_timer = 0;
    }
}
