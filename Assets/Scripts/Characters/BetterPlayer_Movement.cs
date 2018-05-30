using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BetterPlayer_Movement : MonoBehaviour {

    public PublicVariableHolder _PublicVariableHolder;

    private UISpellSwap _UISpells;
    private Image _BoySelected;
    private Image _GirlSelected;
    private ParticleSystem _BoySelectedParticle;
    private ParticleSystem _GirlSelectedParticle;
    private GameObject Boy;
    private GameObject Girl;

    private NavMeshAgent m_agent;
    public bool isTheBoy = false;
    public bool stopMoving = false;
    public bool boyActive = false;
    private GameObject curTarget;

    private bool ReviveStart;
    
	// Use this for initialization
	void Start () {

        m_agent = GetComponent<NavMeshAgent>();

        _UISpells = _PublicVariableHolder._UISpells;
        _BoySelected = _PublicVariableHolder._BoySelected;
        _GirlSelected = _PublicVariableHolder._GirlSelected;
        _BoySelectedParticle = _PublicVariableHolder._BoySelectedParticle;
        _GirlSelectedParticle = _PublicVariableHolder._GirlSelectedParticle;
        Boy = _PublicVariableHolder.Boy;
        Girl = _PublicVariableHolder.Girl;

    }
	
	// Update is called once per frame
	void Update () {

        Debug.Log("ReviveStart is" + ReviveStart);

        if (PauseMenu.gameIsPaused)
        {
            return;
        }

        //For switching player characters
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (Boy.activeSelf == true && Boy.GetComponent<HealthController>().currentHealth>0)
            {
                SwapBoy();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (Girl.activeSelf == true && Boy.GetComponent<HealthController>().currentHealth> 0)
            {
                SwapGirl();
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if(hit.collider.name == "Boy")
                {
                    Debug.Log("Click");
                    if(hit.collider.GetComponent<HealthController>().currentHealth > 0)
                        SwapBoy();
                }
                if (hit.collider.name == "Girl")
                {
                    if (hit.collider.GetComponent<HealthController>().currentHealth > 0)
                        SwapGirl();
                }
            }
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
                    if (hit.collider.tag == "Ground" || hit.collider.tag == "RangeIndicator")
                    {
                        this.gameObject.GetComponent<SpellCommand>().CancelAOEAttack();
                        this.gameObject.GetComponent<SpellCommand>().CancelHealAttack();


                        Vector3 newpos = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                        m_agent.SetDestination(newpos);
                        curTarget = null;
                        this.GetComponent<MeleeDamage>().TargetChanges(curTarget);
                    }
                    if(hit.collider.tag == "Enemy")
                    {
                        curTarget = hit.collider.gameObject;
                        this.GetComponent<MeleeDamage>().TargetChanges(curTarget);
                        //Debug.Log("Target is " + curTarget.name);

                        //this should chase enemy if enemy is not currently in range
                        if (this.GetComponentInChildren<RangeChecker>().InRange(curTarget) == false)
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

        //this should chase enemy if enemy is not currently in range
        if (this.GetComponentInChildren<RangeChecker>().InRange(curTarget) == false)
        {
            if (curTarget)
            {
                //Debug.Log("in range: " + curTarget.name);
                m_agent.SetDestination(curTarget.transform.position);
            }
        }

    }

    public void SwapBoy()
    {
        boyActive = true;
        _BoySelected.enabled = true;
        this.gameObject.GetComponent<SpellCommand>().CancelAOEAttack();
        this.gameObject.GetComponent<SpellCommand>().CancelHealAttack();
        _UISpells.BoySpellActive();
        _GirlSelected.enabled = false;
        _BoySelectedParticle.Play();
        _GirlSelectedParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    public void SwapGirl()
    {
        boyActive = false;
        _BoySelected.enabled = false;
        _GirlSelected.enabled = true;
        _UISpells.GirlActive();
        _BoySelectedParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        _GirlSelectedParticle.Play();
    }

    public void CancelMovement()
    {
        m_agent.SetDestination(m_agent.transform.position);
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (curTarget)
        {
            if (other == curTarget.GetComponent<Collider>())
            {
                //Debug.Log("Target in Range " + curTarget.name);
                CancelMovement();

                if (ReviveStart == true)
                {
                    gameObject.GetComponent<HealthController>().m_reviveCoroutine = true;
                    ReviveStart = false;
                }
                //Pass attack function here?
            }
            else return;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (curTarget)
        {
            if (other == curTarget.GetComponent<Collider>())
            {
                //Debug.Log("Target in Range " + curTarget.name);
                CancelMovement();

                if (ReviveStart == true)
                {
                    gameObject.GetComponent<HealthController>().m_reviveCoroutine = true;
                    ReviveStart = false;
                }
                //Pass attack function here?
            }
            else return;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (curTarget)
        {
            //Debug.Log("TriggerExit" + other.name);
            if (other == curTarget.GetComponent<Collider>())
            {          
                //will have player chase target once target leaves attack range trigger
                m_agent.SetDestination(curTarget.transform.position);              
                //Debug.Log("Target out of range " + curTarget.name);
            }
        }
    }

    public void Death()
    {
        Debug.Log("Revive");
        if(gameObject.name == "Girl")
        {
            curTarget = Boy;
            ReviveStart = true;
        }
        if(gameObject.name == "Boy")
        {
            curTarget = Girl;
            ReviveStart = true;
        }
    }
    
}
