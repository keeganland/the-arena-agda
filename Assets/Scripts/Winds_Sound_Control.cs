using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Winds_Sound_Control : MonoBehaviour {

    public List<GameObject> _AudioSource;
    public List<float> _AudioTimers;

    private bool m_isCoroutine;


	void Update ()
    {
            if(!m_isCoroutine)
            StartCoroutine(BlowWind());
	}

    private IEnumerator BlowWind()
    {
        m_isCoroutine = true;
        for (int i = 0; i < _AudioSource.Count; i++)
        {
            _AudioSource[i].GetComponent<AudioSource>().enabled = true;

            yield return new WaitForSeconds(_AudioTimers[i]);

            _AudioSource[i].GetComponent<AudioSource>().enabled = false;
        }
        m_isCoroutine = false;
    }
}
