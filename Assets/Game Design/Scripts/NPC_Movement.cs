using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC_Movement : MonoBehaviour {

    private NavMeshAgent m_agent;
    public bool m_currentPlayer = false;

    public List<Vector3> _Targets;

    // Use this for initialization
    void Start()
    {
        m_agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
            if (Input.GetMouseButtonDown(1))
            {
            Debug.Log("here");
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.tag == "Ground")
                    {
                        m_agent.SetDestination(hit.point);
                    }
                }
            }
        }
 }
