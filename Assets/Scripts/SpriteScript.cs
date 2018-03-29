using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpriteScript : MonoBehaviour {

    public Transform _Target;
    public NavMeshAgent _Agent;

    private Animator m_anim;
    private AudioSource m_audio;
    private bool m_audioPlaying;

	// Use this for initialization
	void Start () {

        m_anim = GetComponent<Animator>();
        m_audio = GetComponent<AudioSource>();
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

        FootSteps();
    }
    // Update is called once per frame
    void LateUpdate () {

        float m_newx = _Target.position.x;
        float m_newy = _Target.position.z;

        transform.position = new Vector3(m_newx, 0, m_newy);

	}

    private void FootSteps()
    {
        if (m_anim.GetInteger("Direction")>0 && !m_audioPlaying)
        {
            m_audio.Play();
            m_audioPlaying = true;
        }
        else if(m_anim.GetInteger("Direction") == 0 || PauseMenu.gameIsPaused)
        {
            m_audio.Stop();
            m_audioPlaying = false;
        }
    }
}
