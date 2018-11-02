﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BetterPlayer_Movement : MonoBehaviour {

    public PublicVariableHolderneverUnload _PublicVariableHolder;

    private UISpellSwap _UISpells;
    private Image _BoySelected;
    private Image _GirlSelected;
    private ParticleSystem _BoySelectedParticle;
    private ParticleSystem _GirlSelectedParticle;
    private GameObject Boy;
    private GameObject Girl;

    public bool NPCisinRange;

    private NavMeshAgent m_agent;
    public bool isCombat;
    public bool isTheBoy = false;
    public bool stopMoving = false;
    public bool boyActive = false;

    public GameObject MoveClick;
    [SerializeField] private GameObject curTarget;

    private bool ReviveStart;

    /* Alex : Drag Box Variables
     */
    [SerializeField]
    Image selectionBoxImage;
    Rect selectrionRect;

    private Vector2 oldMousePosition;
    private Vector2 newMousePosition;




    /* Keegan note 2018/6/6
     * 
     * The two functions below, Awake and OnDisable, imply the need for modifications and improvements to:
     * MovementManager.cs (possibly completely obselete?)
     * TextBoxManager.cs (needs improvement, general refactors)
     * [Possibly more?]
     * 
     * Remove this note once these changes have been made
     */

    private void Awake()
    {
        EventManager.StartListening("stopForTextbox", stopPlayerAgent);
        EventManager.StartListening("resumeAfterTextbox", startPlayerAgent);
    }

    private void OnDisable()
    {
        EventManager.StopListening("stopForTextbox", stopPlayerAgent);
        EventManager.StopListening("resumeAfterTextbox", startPlayerAgent);

    }

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
	
    /* Keegan note 2018/6/6
     * Codesmell:
     * Overly long Update function. Please review and refactor
     */
	// Update is called once per frame
	void Update () {

        if (PauseMenu.gameIsPaused)
        {
            return;
        }

        //For switching player characters
        if (isCombat)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                if (Boy.activeSelf == true && Boy.GetComponent<HealthController>().currentHealth > 0)
                {
                    SwapBoy();
                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                if (Girl.activeSelf == true && Boy.GetComponent<HealthController>().currentHealth > 0)
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

                    if (hit.collider.name == "Boy")
                    {

                        SwapBoy();

                        if (hit.collider.GetComponent<HealthController>().currentHealth <= 0)
                        {
                            Death();
                        }
                    }
                    if (hit.collider.name == "Girl")
                    {

                        SwapGirl();

                        if (hit.collider.GetComponent<HealthController>().currentHealth <= 0)
                        {
                            Death();
                        }
                    }
                }
            }
        }

        //For stopping the player character - whichever one that happens to be
        if (isTheBoy == boyActive && !stopMoving)
        {
            if(Input.GetMouseButtonDown(1))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                   if (hit.collider.tag == "Ground" || hit.collider.tag == "RangeIndicator")
                   {
                      this.GetComponent<PlayerAI>().AIavailable = false;
                      this.GetComponent<PlayerAI>().hasTarget = false;
                      Boy.GetComponent<SpellCommand>().CancelAOEAttack();
                      Boy.GetComponent<SpellCommand>().CancelHealAttack();
                      Boy.GetComponent<SpellCommand>().CancelBoyShield();
                      Boy.GetComponent<SpellCommand>().CancelBoyStun();

                      Girl.GetComponent<SpellCommand>().CancelAOEAttack();
                      Girl.GetComponent<SpellCommand>().CancelHealAttack();
                      Girl.GetComponent<SpellCommand>().CancelBoyShield();
                      Girl.GetComponent<SpellCommand>().CancelBoyStun();

                      Vector3 newpos = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                      m_agent.SetDestination(newpos);
                        if (curTarget && !curTarget.CompareTag("NPC") && !curTarget.CompareTag("Objects"))
                         curTarget.GetComponent<HealthController>().CancelEnemy(this.gameObject);
                      curTarget = null;
                      this.GetComponent<MeleeDamage>().TargetChanges(curTarget);

                      GameObject move = Instantiate(MoveClick, new Vector3(hit.point.x, transform.position.y, hit.point.z), Quaternion.identity);
                      Destroy(move, 1f);
                   }
                   else if (hit.collider.tag == "Enemy")
                   {
                       this.GetComponent<PlayerAI>().AIavailable = true;
                       this.GetComponent<PlayerAI>().hasTarget = true;
                       curTarget = hit.collider.gameObject;
                       curTarget.GetComponent<HealthController>().SetEnemy(this.gameObject);
                       this.GetComponent<MeleeDamage>().TargetChanges(curTarget);
                       //Debug.Log("Target is " + curTarget.name);
                       GameObject move = Instantiate(MoveClick, new Vector3(hit.point.x, transform.position.y, hit.point.z), Quaternion.identity);
                       ParticleSystem.MainModule movemain = move.GetComponent<ParticleSystem>().main;
                       movemain.startColor = Color.red;
                       movemain.startSize = 2;
                       Destroy(move, 1f);

                      //this should chase enemy if enemy is not currently in range
                      if (this.GetComponentInChildren<RangeChecker>().InRange(curTarget) == false)
                      {
                         //Debug.Log("in range: " + curTarget.name);
                         m_agent.SetDestination(hit.point);
                         //OnTriggerEnter should stop character once target is within range
                      }
                   }
                   else if (hit.collider.tag == "Objects")
                   {
                        curTarget = hit.collider.gameObject;
                     // hit.collider.GetComponent<InteractiveObjects>().DoAction();
                   }

                   else if (hit.collider.tag == "NPC")
                   {
                        curTarget = hit.collider.gameObject;                    
                   }
                   else if (hit.collider.tag == "Player")
                   {
                      curTarget = hit.collider.gameObject;
                   }
                }
                if (gameObject.GetComponent<HealthController>().m_reviveCoroutine == true)
                {
                   gameObject.GetComponent<HealthController>().StopReviveCoroutine();
                }
            }

        }

        /* Keegan note 2018/6/6:
         * The below may be COMPLETELY unnecessary right now. Check old scripts to see if there's any important logical difference
         */
        //m_agent.isStopped = stopMoving; //Alex changes
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

        NonCombat();

        if (curTarget)
        {
            if (curTarget.CompareTag("Objects"))
            {
                Debug.Log("Not Set Destination?");

                //Debug.Log("Target is " + curTarget.name);
                //this should chase enemy if enemy is not currently in range

                    //Debug.Log("in range: " + curTarget.name);
                  if (curTarget.GetComponent<InteractiveObjects>().isAttackable)
                    {
                        this.GetComponent<MeleeDamage>().TargetChanges(curTarget);
                    if (this.GetComponentInChildren<ObjectChecker>().InRange(curTarget) == false)
                         {
                        m_agent.SetDestination(curTarget.transform.position);
                         }
                    }
                    else
                    {
                        curTarget.GetComponent<InteractiveObjects>().DoAction();
                    }
                    //OnTriggerEnter should stop character once target is within range
            }

            if (curTarget.CompareTag("NPC"))
            {
                if (this.GetComponentInChildren<RangeChecker>().InRange(curTarget) == false)
                {
                    Debug.Log("Not in range: " + curTarget.name);
                    m_agent.SetDestination(curTarget.transform.position);
                    //OnTriggerEnter should stop character once target is within range
                }
            }
        }
    }

    public void NonCombat()
    {
        if(isCombat == false)
        {
            if (isTheBoy == true)
            {
                curTarget = Girl;
            }
        }
    }

    public void SwapBoy()
    {
        if (_PublicVariableHolder.Boy.GetComponent<HealthController>().currentHealth > 0 && !_PublicVariableHolder.StopAllActions)
        {
            boyActive = true;

            SelectedParticleBoy();

            Boy.GetComponent<SpellCommand>().CancelAOEAttack();
            Boy.GetComponent<SpellCommand>().CancelHealAttack();
            Boy.GetComponent<SpellCommand>().CancelBoyStun();
            Boy.GetComponent<SpellCommand>().CancelBoyShield();
            Girl.GetComponent<SpellCommand>().CancelAOEAttack();
            Girl.GetComponent<SpellCommand>().CancelHealAttack();
            Girl.GetComponent<SpellCommand>().CancelBoyStun();
            Girl.GetComponent<SpellCommand>().CancelBoyShield();


            Boy.GetComponent<SpellCommand>().isSmallUI =false;
            Girl.GetComponent<SpellCommand>().isSmallUI = false;
            _UISpells.BoySpellActive();
            _GirlSelected.enabled = false;
            _GirlSelectedParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        }
    }

    public void SelectedParticleBoy()
    {
        _BoySelected.enabled = true;
        _BoySelectedParticle.Play();
    }

    public void SwapGirl()
    {
        if (_PublicVariableHolder.Girl.GetComponent<HealthController>().currentHealth > 0 && !_PublicVariableHolder.StopAllActions)
        {
            boyActive = false;
            _BoySelected.enabled = false;
            SelectedParticleGirl();
            Boy.GetComponent<SpellCommand>().CancelAOEAttack();
            Boy.GetComponent<SpellCommand>().CancelHealAttack();
            Boy.GetComponent<SpellCommand>().CancelBoyStun();
            Boy.GetComponent<SpellCommand>().CancelBoyShield();
            Girl.GetComponent<SpellCommand>().CancelAOEAttack();
            Girl.GetComponent<SpellCommand>().CancelHealAttack();
            Girl.GetComponent<SpellCommand>().CancelBoyStun();

            Boy.GetComponent<SpellCommand>().isSmallUI = false;
            Girl.GetComponent<SpellCommand>().isSmallUI = false;

            _UISpells.GirlActive();
            _BoySelectedParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }

    public void SelectedParticleGirl()
    {
        _GirlSelected.enabled = true;
        _GirlSelectedParticle.Play();
    }

    public void CancelParticles()
    {
        _BoySelected.enabled = false;
        _BoySelectedParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        _GirlSelected.enabled = false;
        _GirlSelectedParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
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

                if (other.CompareTag("NPC") && !NPCisinRange)
                {
                    NPCisinRange = true;
                    ActivateTextAtLine activateText = curTarget.GetComponent<ActivateTextAtLine>();
                    if (activateText)
                        activateText.PlayerEnableText(true);
                }

                if (ReviveStart == true)
                {
                    gameObject.GetComponent<HealthController>().m_reviveCoroutine = true;
                    ReviveStart = false;
                }

                if(other.CompareTag("Objects"))
                {
                    Debug.Log("here for Lights");
                    other.GetComponent<InteractiveObjects>().DoAction();
                }
                //Pass attack function here
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
                if (other.CompareTag("NPC") && !NPCisinRange)
                {
                    NPCisinRange = true;
                    ActivateTextAtLine activateText = curTarget.GetComponent<ActivateTextAtLine>();
                    if (activateText)
                        activateText.PlayerEnableText(true);
                }

                if (ReviveStart == true)
                {
                    gameObject.GetComponent<HealthController>().m_reviveCoroutine = true;
                    ReviveStart = false;
                }

                if (other.CompareTag("Objects"))
                {
                    Debug.Log("here for Lights");
                    curTarget.GetComponent<InteractiveObjects>().DoAction();
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
        NPCisinRange = false;
    }

    private void Death()
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

    public void UndoCurTarget()
    {
        this.GetComponent<PlayerAI>().hasTarget = false;
        this.GetComponent<PlayerAI>().AIavailable = false;
        if(curTarget)
        curTarget.GetComponent<HealthController>().CancelEnemy(this.gameObject);
        curTarget = null;
    }

    public void SetCurTarget(GameObject target)
    {
        if (target != null) {
            curTarget = target;
            if (GetComponent<PlayerAI>() != null)
                this.GetComponent<PlayerAI>().hasTarget = true;
            curTarget.GetComponent<HealthController>().SetEnemy(this.gameObject);
            this.GetComponent<MeleeDamage>().TargetChanges(curTarget);
        }
    }


    public void stopPlayerAgent()
    {
        m_agent.isStopped = true;
    }

    public void startPlayerAgent()
    {
        m_agent.isStopped = false;
    }
}
