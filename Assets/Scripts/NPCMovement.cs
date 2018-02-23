using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCMovement : MonoBehaviour {

    public float speed = 1;
    public List<GameObject> _Waypoints;
    public List<float> _Timers;
    public NPCMovementManager movementMananger; //not sure if I should ultimately use the MovementManager as NPCMovementManager

    public bool stop;

    private NavMeshAgent m_agent;
    private int m_i = 0;
    private bool m_coroutinestarted;
    private Vector3 m_newpos;

	void Start () {

        m_agent = GetComponent<NavMeshAgent>();

	}
	
	// Update is called once per frame
	void Update () {

        stop = !movementMananger.isNPCMovementAllowed();

        if (stop)
        {
            //Debug.Log("Stopping!");
            return;
        }

        if (m_coroutinestarted != true)
        {

            m_newpos = new Vector3(_Waypoints[m_i].transform.position.x, this.transform.position.y, _Waypoints[m_i].transform.position.z);

            m_agent.SetDestination(m_newpos);

            if (transform.position == m_newpos)
            {
                StartCoroutine(Move());
            }
   
        }
	}

    private IEnumerator Move()
    {
        m_coroutinestarted = true;

        yield return new WaitForSeconds(_Timers[m_i]);

        if(this.CompareTag("NPC")){
            m_i = Random.Range(0,_Waypoints.Count);

        }
        else
        {
            m_i += 1;
        }

        m_coroutinestarted = false;
    }
}
