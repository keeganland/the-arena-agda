using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {

    TargetManager m_targetM;
    private GameObject m_target = null;
    private UnityEngine.AI.NavMeshAgent m_agent;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        m_target = GetComponent<TargetManager>().curTarget;

        if (this.GetComponent<RangeChecker>().InRange(m_target) == false)
        {
            m_agent.SetDestination(m_target.transform.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == m_target.GetComponent<Collider>())
        {
            //Debug.Log("EnemyTarget in Range " + curTarget.name);
            //need to fix this
            //this.GetComponent<EnemyMovement>().CancelMovement();
            //Pass attack function here?
        }
        else return;
    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log("EnemyTriggerExit" + other.name);
        if (other == m_target.GetComponent<Collider>())
        {
            //will have player chase target once target leaves attack range trigger
            m_agent.SetDestination(m_target.transform.position);
            //Debug.Log("Target out of range " + curTarget.name);

        }
    }

    public void TargetSet(GameObject target)
    {
        m_target = target;
    }
}
