using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCMovement : MonoBehaviour {

    public float speed = 1;
    public List<GameObject> _Waypoints;
    public List<float> _Timers;

    private NavMeshAgent m_agent;
    private int m_i = 0;
    private bool m_coroutinestarted;
    private Vector3 m_newpos;

	// Use this for initialization
	void Start () {

        m_agent = GetComponent<NavMeshAgent>();

	}
	
	// Update is called once per frame
	void Update () {

        if (m_coroutinestarted != true)
        {
            Debug.Log("here");

            m_newpos = new Vector3(_Waypoints[m_i].transform.position.x, this.transform.position.y, _Waypoints[m_i].transform.position.z);

            m_agent.SetDestination(m_newpos);

            if (transform.position == m_newpos)
            {
                StartCoroutine(Move(m_i));
            }
   
        }
	}

    private IEnumerator Move(int i)
    {
        m_coroutinestarted = true;

        Debug.Log("here2");
        yield return new WaitForSeconds(_Timers[i]);

        m_i += 1;

        m_coroutinestarted = false;
    }
}
