using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour {

    TargetManager m_targetM;
    private GameObject m_target = null; 
    private NavMeshAgent m_agent;


    // Use this for initialization
    void Start () {
        m_targetM = GetComponent<TargetManager>();
        m_agent = GetComponentInChildren<NavMeshAgent>();
    }
	
	// Update is called once per frame
	void Update () {

        m_target = m_targetM.curTarget;
        //Debug.Log("[EnemyMovement] set target as " + m_target.name);

        if (this.GetComponentInChildren<RangeChecker>().InRange(m_target) == false && m_target != null)
        {
            Debug.Log("[EnemyMovement] set destination1 for " + m_target.name);
            m_agent.SetDestination(m_target.transform.position);
        Debug.Log("position is" + m_target.transform.position);//WTF is going on, only worked after adding this debug????
            Debug.Log("[EnemyMovement] set destination2 for " + m_target.name);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == m_target.GetComponent<Collider>())
        {
            //Debug.Log("EnemyTarget in Range " + curTarget.name);
            //need to fix this
            CancelMovement();
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
            Debug.Log("Target out of range " + m_target.name);

        }
    }

    public void CancelMovement()
    {
        m_agent.SetDestination(m_agent.transform.position);
    }

    public void TargetSet(GameObject target)
    {
        m_target = target;
    }
}
