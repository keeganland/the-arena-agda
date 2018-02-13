using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player_Movement : MonoBehaviour {

    private NavMeshAgent m_agent;
    public bool isTheBoy = false;
    public bool stopMoving = false;
    public bool boyActive = false;
    private GameObject curTarget;
    
	// Use this for initialization
	void Start () {

        m_agent = GetComponent<NavMeshAgent>();

	}
	
	// Update is called once per frame
	void Update () {

        //For switching player characters
        if (Input.GetKeyDown(KeyCode.Space))
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
                        //need to pass this information somewhere to do something with it

                        //this should chase enemy if enemy is not currently in range
                        //need to fix InRange call for this to work
                        if (this.GetComponent<RangeChecker>().InRange(curTarget) == false)
                        {
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

    //commented out to try to avoid errors for now, will implement later *Patric
    /*private void OnTriggerEnter(Collider other)
    {
        if (other == curTarget)
        {
            CancelMovement();
        }
        else return;
    }
    
    private void OnTriggerExit(Collider other)
    {
        if(other == curTarget)
        {
            //will have player chase target once target leaves attack range trigger
            m_agent.SetDestination(curTarget.transform.position)
        }
    }*/
    
}
