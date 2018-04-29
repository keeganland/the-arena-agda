using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneBlink : MonoBehaviour {

    MeshRenderer m_rend;

    public bool isCoroutineStarted;

     void Start()
    {
        m_rend = GetComponent<MeshRenderer>();
    }

    public IEnumerator Blink()
    {
        isCoroutineStarted = true;

        m_rend.enabled = true;
        yield return new WaitForSeconds(0.5f);
        m_rend.enabled = false;
		yield return new WaitForSeconds(0.5f);
		m_rend.enabled = true;
		yield return new WaitForSeconds(0.5f);
		m_rend.enabled = false;


        isCoroutineStarted = false;
	}
}
