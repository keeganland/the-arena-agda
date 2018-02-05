using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind_Control : MonoBehaviour {

    public List<WindZone> _Winds;

    public List<float> _BurstDuration;
    public List<float> _WindForce;
    public List<float> _BurstCooldown;

    private List<bool> m_isCoroutine;

    private void Start()
    {
        m_isCoroutine = new List<bool>(new bool[_Winds.Count]);
        Debug.Log(m_isCoroutine.Count);
    }
    private void Update()
    {
        for (int i = 0; i <= _Winds.Count; i++)
        {
            if (m_isCoroutine[i] != true)
            {
                StartCoroutine(WindBursts(i));
            }
        }
           
    }

    private IEnumerator WindBursts(int i)
    {

        m_isCoroutine[i] = true;

        _Winds[i].windMain = _WindForce[i];

        yield return new WaitForSeconds(_BurstDuration[i]);

        _Winds[i].windMain = 0;

        yield return new WaitForSeconds(_BurstCooldown[i]);

        m_isCoroutine[i] = false;


    }
}
