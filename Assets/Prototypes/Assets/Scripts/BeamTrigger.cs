using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamTrigger : MonoBehaviour {

    public GameObject _Launcher;
    public GameObject _Wildfire;
    public float _FireDuration = 4.0f;
    public GameObject _Plane;
    public ParticleSystem _Smoke;

    private GameObject m_fire;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("BeamTarget"))
        {
            _Launcher.GetComponent<Collider>().enabled = true;
            m_fire = Instantiate(_Wildfire, other.transform) as GameObject;
            if (_Launcher.GetComponentInChildren<LineRenderer>())
            {
                _Launcher.GetComponentInChildren<LineRenderer>().enabled = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("BeamTarget"))
            
        {
            _Launcher.GetComponent<Collider>().enabled = false;
            _Launcher.GetComponentInChildren<LineRenderer>().enabled = false;
			_Plane.GetComponent<MeshCollider>().enabled = true;
        }
    }

    private void Update()
    {
        if (m_fire && _Plane.GetComponent<PlaneBlink>().isCoroutineStarted == false)
        {
            _Plane.GetComponent<MeshRenderer>().enabled = true;
        }

        else if (!m_fire && _Plane.GetComponent<PlaneBlink>().isCoroutineStarted == false)
        {
            _Plane.GetComponent<MeshRenderer>().enabled = false;
			_Plane.GetComponent<MeshCollider>().enabled = false;
        }
    }

    public void DestroyFire() 
    {
        StartCoroutine("PlaySmoke");
        Destroy(m_fire, 4.0f);
    }

    private IEnumerator PlaySmoke()
    {
        yield return new WaitForSeconds(4.0f);
        _Smoke.Play();
    }

}
