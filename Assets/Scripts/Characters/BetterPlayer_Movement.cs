using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BetterPlayer_Movement : MonoBehaviour {

    public PublicVariableHolderneverUnload _PublicVariableHolder;

    //Config
    [SerializeField] private float volumeScaleFactor;
    private float footstepsSoundCd;

    //State
    [SerializeField] private bool isCombat;
    [SerializeField] private bool isTheBoy = false;
    [SerializeField] private bool stopMoving = false;
    [SerializeField] private bool boyActive = false;

    private bool ReviveStart;
    public bool NPCisinRange;

    //Cached component references
    [SerializeField] Image selectionBoxImage;
    Rect selectrionRect;

    private UISpellSwap _UISpells;
    private Image _BoySelected;
    private Image _GirlSelected;
    private ParticleSystem _BoySelectedParticle;
    private ParticleSystem _GirlSelectedParticle;
    private GameObject boyPlayer;
    private GameObject girlPlayer;
    private GameObject ObjectInteraction;

    private Vector2 oldMousePosition;
    private Vector2 newMousePosition;

    public AudioClip Footsteps;
    private AudioSource m_audioSource;
    private NavMeshAgent m_agent;
    public GameObject MoveClick;
    [SerializeField] private GameObject curTarget;

    /* Keegan note 2018/6/6
     * 
     * The two functions below, Awake and OnDisable, imply the need for modifications and improvements to:
     * MovementManager.cs (possibly completely obselete?)
     * TextBoxManager.cs (needs improvement, general refactors)
     * [Possibly more?]
     * 
     * Remove this note once these changes have been made
     * 
     * Alex note 2019/01/18 : Refactor (underway) with adding the Getter/Setter region + other small changes
     */

    #region Getters and Setters
    public bool BoyActive
    {
        get { return boyActive; }
        set { boyActive = value; }
    }
    public bool IsCombat
    {
        get { return isCombat; }
        set { isCombat = value; }
    }
    public bool StopMoving
    {
        get { return stopMoving; }
        set { stopMoving = value; }
    }
    public bool IsTheBoy
    {
        get { return isTheBoy; }
    }
    #endregion

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

        m_audioSource = GetComponent<AudioSource>();
        m_agent = GetComponent<NavMeshAgent>();

        SoundManager.onSoundChangedCallback += UpdateSound;

        _UISpells = _PublicVariableHolder._UISpells;
        _BoySelected = _PublicVariableHolder._BoySelected;
        _GirlSelected = _PublicVariableHolder._GirlSelected;
        _BoySelectedParticle = _PublicVariableHolder._BoySelectedParticle;
        _GirlSelectedParticle = _PublicVariableHolder._GirlSelectedParticle;
        girlPlayer = GameObject.FindGameObjectWithTag("Player/Girl");
        boyPlayer = GameObject.FindGameObjectWithTag("Player/Boy");
    }

    /* Keegan note 2018/6/6
     * Codesmell:
     * Overly long Update function. Please review and refactor
     */
    // Update is called once per frame
    void Update () {
        
        if (Footsteps)
        {
            footstepsSoundCd += Time.deltaTime;

            if (m_agent.velocity != new Vector3(0, 0, 0))
            {
                if (!m_audioSource.isPlaying && footstepsSoundCd >= 0.5)
                {
                    m_audioSource.PlayOneShot(Footsteps);
                    footstepsSoundCd = 0;
                }
            }
            else
            {
                m_audioSource.Stop();
            }
        }

        if (PauseMenu.gameIsPaused)
        {
            return;
        }

        //For switching player characters
        if (isCombat)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                if (boyPlayer.activeSelf == true && boyPlayer.GetComponent<HealthController>().CurrentHealth > 0)
                {
                    SwapBoy();
                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                if (girlPlayer.activeSelf == true && boyPlayer.GetComponent<HealthController>().CurrentHealth > 0)
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

                        if (hit.collider.GetComponent<HealthController>().CurrentHealth <= 0)
                        {
                            Death();
                        }
                    }
                    if (hit.collider.name == "Girl")
                    {

                        SwapGirl();

                        if (hit.collider.GetComponent<HealthController>().CurrentHealth <= 0)
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
                        boyPlayer.GetComponent<SpellCommand>().CancelAOEAttack();
                        boyPlayer.GetComponent<SpellCommand>().CancelHealAttack();
                        boyPlayer.GetComponent<SpellCommand>().CancelBoyShield();
                        boyPlayer.GetComponent<SpellCommand>().CancelBoyStun();

                        girlPlayer.GetComponent<SpellCommand>().CancelAOEAttack();
                        girlPlayer.GetComponent<SpellCommand>().CancelHealAttack();
                        girlPlayer.GetComponent<SpellCommand>().CancelBoyShield();
                        girlPlayer.GetComponent<SpellCommand>().CancelBoyStun();

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
                        //Debug.Log(hit.collider.gameObject.name + " Is an object");
                        curTarget = hit.collider.gameObject;
                        ObjectInteraction = hit.collider.gameObject;
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
                if (gameObject.GetComponent<HealthController>().ReviveCoroutine == true)
                {
                   gameObject.GetComponent<HealthController>().StopReviveCoroutine();
                }
                if (ObjectInteraction)
                {
                    if (ObjectInteraction.GetComponent<InteractiveObjectAbstract>().canBeCancelled && ObjectInteraction.GetComponent<InteractiveObjectAbstract>().isCoroutineStarted)
                    {
                        ObjectInteraction.GetComponent<InteractiveObjectAbstract>().CancelAction(this.gameObject);
                    }
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
                //Debug.Log("Not Set Destination?");

                //Debug.Log("Target is " + curTarget.name);
                //this should chase enemy if enemy is not currently in range

                    //Debug.Log("in range: " + curTarget.name);
                if (curTarget.GetComponent<InteractiveObjectAbstract>().isAttackable)
                    {
                        this.GetComponent<MeleeDamage>().TargetChanges(curTarget);
                    if (this.GetComponentInChildren<ObjectChecker>().InRange(curTarget) == false)
                         {                     
                        m_agent.SetDestination(curTarget.transform.position);
                         }
                    }
                    else
                    {
                    curTarget.GetComponent<InteractiveObjectAbstract>().DoAction(this.gameObject);
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
                curTarget = girlPlayer;
            }
        }
    }

    public void SwapBoy()
    {
        if (boyPlayer.GetComponent<HealthController>().CurrentHealth > 0 && !_PublicVariableHolder.StopAllActions)
        {
            boyActive = true;

            SelectedParticleBoy();

            boyPlayer.GetComponent<SpellCommand>().CancelAOEAttack();
            boyPlayer.GetComponent<SpellCommand>().CancelHealAttack();
            boyPlayer.GetComponent<SpellCommand>().CancelBoyStun();
            boyPlayer.GetComponent<SpellCommand>().CancelBoyShield();
            girlPlayer.GetComponent<SpellCommand>().CancelAOEAttack();
            girlPlayer.GetComponent<SpellCommand>().CancelHealAttack();
            girlPlayer.GetComponent<SpellCommand>().CancelBoyStun();
            girlPlayer.GetComponent<SpellCommand>().CancelBoyShield();


            boyPlayer.GetComponent<SpellCommand>().isSmallUI =false;
            girlPlayer.GetComponent<SpellCommand>().isSmallUI = false;
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
        if (girlPlayer.GetComponent<HealthController>().CurrentHealth > 0 && !_PublicVariableHolder.StopAllActions)
        {
            boyActive = false;
            _BoySelected.enabled = false;
            SelectedParticleGirl();
            boyPlayer.GetComponent<SpellCommand>().CancelAOEAttack();
            boyPlayer.GetComponent<SpellCommand>().CancelHealAttack();
            boyPlayer.GetComponent<SpellCommand>().CancelBoyStun();
            boyPlayer.GetComponent<SpellCommand>().CancelBoyShield();
            girlPlayer.GetComponent<SpellCommand>().CancelAOEAttack();
            girlPlayer.GetComponent<SpellCommand>().CancelHealAttack();
            girlPlayer.GetComponent<SpellCommand>().CancelBoyStun();

            boyPlayer.GetComponent<SpellCommand>().isSmallUI = false;
            girlPlayer.GetComponent<SpellCommand>().isSmallUI = false;

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


                if (other.CompareTag("NPC") && !NPCisinRange)
                {
                    NPCisinRange = true;
                    ActivateTextAtLine activateText = curTarget.GetComponent<ActivateTextAtLine>();
                    if (activateText)
                        activateText.PlayerEnableText(true);
                }

                if (ReviveStart == true)
                {
                    gameObject.GetComponent<HealthController>().ReviveCoroutine = true;
                    ReviveStart = false;
                }

                if(other.CompareTag("Objects") && GetComponentInChildren<ObjectChecker>().InRange(other.gameObject))
                {
                    //Debug.Log("here for Lights");
                    other.GetComponent<InteractiveObjectAbstract>().DoAction(this.gameObject);
                    UndoCurTarget();

                }
                else if(other.CompareTag("Objects") && !GetComponentInChildren<ObjectChecker>().InRange(other.gameObject))
                {
                    return;
                }
                CancelMovement();
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
           
                if (other.CompareTag("NPC") && !NPCisinRange)
                {
                    NPCisinRange = true;
                    ActivateTextAtLine activateText = curTarget.GetComponent<ActivateTextAtLine>();
                    if (activateText)
                        activateText.PlayerEnableText(true);
                }

                if (ReviveStart == true)
                {
                    gameObject.GetComponent<HealthController>().ReviveCoroutine = true;
                    ReviveStart = false;
                }

                if (other.CompareTag("Objects") && GetComponentInChildren<ObjectChecker>().InRange(other.gameObject))
                {
                    curTarget.GetComponent<InteractiveObjectAbstract>().DoAction(this.gameObject);
                    UndoCurTarget();
                }
                else if (other.CompareTag("Objects") && !GetComponentInChildren<ObjectChecker>().InRange(other.gameObject))
                {
                    return;
                }

                CancelMovement();
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
            curTarget = boyPlayer;
            ReviveStart = true;
        }
        if(gameObject.name == "Boy")
        {
            curTarget = girlPlayer;
            ReviveStart = true;
        }
    }  

    public void UndoCurTarget()
    {
        this.GetComponent<PlayerAI>().hasTarget = false;
        this.GetComponent<PlayerAI>().AIavailable = false;
        if(curTarget && curTarget.GetComponent<HealthController>())
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

    void UpdateSound()
    {
        m_audioSource.volume = (SoundManager.SFXVolume * volumeScaleFactor) / 100;
    }
}
