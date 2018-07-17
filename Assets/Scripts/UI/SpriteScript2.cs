using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpriteScript2 : MonoBehaviour
{
    public PublicVariableHolderneverUnload publicVariableHolderneverUnload;
    public float _DistanceFromSprite = 15;

    public Transform _Target;
    public NavMeshAgent _Agent;

    private Animator m_anim;
    private GameObject mainCamera;

    // Use this for initialization
    void Start()
    {
        if(!publicVariableHolderneverUnload)
        publicVariableHolderneverUnload = GameObject.Find("/PublicVariableHolderNeverUnload").GetComponent<PublicVariableHolderneverUnload>();
        
        m_anim = GetComponent<Animator>();

        mainCamera = publicVariableHolderneverUnload.MainCamera;

        Vector3 rotation = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, mainCamera.transform.localEulerAngles.z);
        this.gameObject.transform.localRotation = Quaternion.Euler(rotation);
    }

    private void Update()
    {
        if (publicVariableHolderneverUnload.MainCamera.transform.localRotation == Quaternion.Euler(0,0,0))
        {
            if (m_anim)
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
        }
        else if(publicVariableHolderneverUnload.MainCamera.transform.localRotation == Quaternion.Euler(0,0,-90))
        {
            if (m_anim)
            {
                if (System.Math.Abs(_Agent.velocity.z) >= System.Math.Abs(_Agent.velocity.x) && _Agent.velocity.z > 0)
                {                 
                    m_anim.SetInteger("Direction", 4);
                }
                else if (System.Math.Abs(_Agent.velocity.z) > System.Math.Abs(_Agent.velocity.x) && _Agent.velocity.z < 0)
                {
                    m_anim.SetInteger("Direction", 3);
                }
                else if (System.Math.Abs(_Agent.velocity.x) >= System.Math.Abs(_Agent.velocity.z) && _Agent.velocity.x > 0)
                {
                    m_anim.SetInteger("Direction", 1);
                }
                else if (System.Math.Abs(_Agent.velocity.x) > System.Math.Abs(_Agent.velocity.z) && _Agent.velocity.x < 0)
                {
                    m_anim.SetInteger("Direction", 2);
                }
                else
                {
                    m_anim.SetInteger("Direction", 0);
                }
            }
        }

    }
    // Update is called once per frame
    void LateUpdate()
    {
        float m_newx = _Target.position.x;
        float m_newy = _Target.position.z;

        transform.position = new Vector3(m_newx, _DistanceFromSprite, m_newy);
    }

    public void ForcePlayerRotation(int direction)
    {
      //  Debug.Log("here");
            if (direction == 1)
            {
                m_anim.Play("idle up");
            }
            else if (direction == 2)
            {
                m_anim.Play("idle down");
            }
            else if (direction == 3)
            {
                m_anim.Play("idle right");
            }
            else if (direction == 4)
            {
                m_anim.Play("idle left");
            }
        }
}
