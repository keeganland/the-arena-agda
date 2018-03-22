using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpriteScript2 : MonoBehaviour
{

    public Transform _Target;
    public NavMeshAgent _Agent;

    private Animator m_anim;

    // Use this for initialization
    void Start()
    {

        m_anim = GetComponent<Animator>();

    }

    private void Update()
    {

        if (System.Math.Abs(_Agent.velocity.z) >= System.Math.Abs(_Agent.velocity.x) && _Agent.velocity.z > 0)
        {
            m_anim.SetInteger("Direction", 1);
        }
        else if (System.Math.Abs(_Agent.velocity.z) > System.Math.Abs(_Agent.velocity.x) && _Agent.velocity.z < 0)
        {
            m_anim.SetInteger("Direction", 2);
        }
        else if (System.Math.Abs(_Agent.velocity.x) >= System.Math.Abs(_Agent.velocity.z) && _Agent.velocity.x > 0)
        {
            m_anim.SetInteger("Direction", 3);
        }
        else if (System.Math.Abs(_Agent.velocity.x) > System.Math.Abs(_Agent.velocity.z) && _Agent.velocity.x < 0)
        {
            m_anim.SetInteger("Direction", 4);
        }
        else
        {
            m_anim.SetInteger("Direction", 0);
        }

    }
    // Update is called once per frame
    void LateUpdate()
    {

        float m_newx = _Target.position.x;
        float m_newy = _Target.position.z;

        transform.position = new Vector3(m_newx, 15, m_newy);

    }

    public void ForcePlayerRotation(int direction)
    {
        Debug.Log(direction);

        if(direction == 1)
        {
            m_anim.Play("6 idle up");
        }
        else if (direction == 2)
        {
            m_anim.Play("6 idle down");
        }
        else if (direction == 3)
        {
            m_anim.Play("6 idle right");
        }
        else if(direction == 4)
        {
            m_anim.Play("6 idle left");
        }
    }
}
