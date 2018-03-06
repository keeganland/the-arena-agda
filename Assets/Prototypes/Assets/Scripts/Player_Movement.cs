using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player_Movement : MonoBehaviour {

    private NavMeshAgent m_agent;
    public bool isTheBoy = false;
    public bool stopMoving = false;
    public bool boyActive = false;
    public GameObject curTarget; //Need this to be public but don't want to have to manually assign it. How can I manage this?
    
	// Use this for initialization
	void Start () {

        m_agent = GetComponent<NavMeshAgent>();

	}
	
	// Update is called once per frame
	void Update () {

        //For switching player characters
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            boyActive = !boyActive;
        }

        //For stopping the player character - whichever one that happens to be
        if (isTheBoy == boyActive && !stopMoving)
        {
            if (Input.GetMouseButtonDown(1))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.tag == "Ground")
                    {
                        m_agent.SetDestination(hit.point);
                    }
                    if(hit.collider.tag == "Enemy")
                    {
                        curTarget = hit.collider.gameObject;
                        //Debug.Log("Target is " + curTarget.name);
                        //need to pass this information somewhere to do something with it

                        //this should chase enemy if enemy is not currently in range
                        if (this.GetComponent<RangeChecker>().InRange(curTarget) == false)
                        {
                            //Debug.Log("in range: " + curTarget.name);
                            m_agent.SetDestination(hit.point);
                            //OnTriggerEnter should stop character once target is within range
                        }
                    }
                }
            }
        }

        m_agent.isStopped = stopMoving;

        /*
        //The above is a more concise way of putting this more readable code:

        if (stopMoving)
        {
            m_agent.isStopped = true;
        }

        if (!stopMoving)
        {
            m_agent.isStopped = false;
        }
        */
	}

    public void CancelMovement()
    {
        m_agent.SetDestination(m_agent.transform.position);
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other == curTarget.GetComponent<Collider>())
        {
            //Debug.Log("Target in Range " + curTarget.name);
            CancelMovement();
            //Pass attack function here?
        }
        else return;
    }
    
    private void OnTriggerExit(Collider other)
    {
        //Debug.Log("TriggerExit" + other.name);
        if(other == curTarget.GetComponent<Collider>())
        {
            //will have player chase target once target leaves attack range trigger
            m_agent.SetDestination(curTarget.transform.position);
            //Debug.Log("Target out of range " + curTarget.name);

        }
    }
    
}
