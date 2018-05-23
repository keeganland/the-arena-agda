using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCommand : MonoBehaviour {
    private int player_number;//variable used to hold value of which character is casting a spell
    private bool isWspell;
    private bool isQspell;
    private Vector3 AOEpos;
    private Vector3 Healpos;

    private bool isQGirlforced;
    private bool isWGirlforced;

    public int Healing;
    public GameObject Shield;
    public GameObject AOE;
    public GameObject RangeIndicatorAOE;
    public GameObject AttackIndicatorAOE;
    public GameObject AttackIndicatorHeal;
    public GameObject RangeIndicatorHeal;
    public GameObject Heal;
    public GameObject RangeIndicatorShield;

    public ParticleSystem _Qselected;
    public ParticleSystem _Wselected;

    /*Timer variables:*/
    public float _AOECooldown;
    private float _AOECooldownTimer = 0;
    public float _AOEUITimer = 8; //for Alex
    public float _HealCooldown;
    private float _HealCooldownTimer = 0;
    public float _HealUITimer; //for Alex
    public float _ShieldCooldown;
    private float _ShieldCooldownTimer = 0;
    public float _ShieldUITimer; //for Alex
    public float _StunCooldown;
    private float _StunCooldownTimer = 0;
    public float _StunUITimer; //for Alex

    private void Start() //This is purely error handling
    {
        if(_AOECooldown == 0)
        {
            _AOECooldown = 3;
        }
        if(_HealCooldown == 0)
        {
            _HealCooldown = 3;
        }
        if(_ShieldCooldown == 0)
        {
            _ShieldCooldown = 5;
        }
        if(_StunCooldown == 0)
        {
            _StunCooldown = 5;
        }

        _Qselected.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        _Wselected.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Q)) //could switch to GetButtonDown laster to allow player to customise controls
        {
            //Debug.Log("SpellCommand: Q pressed");
            if (this.name == "Girl" && _HealCooldownTimer ==0)
            {
             
                isQspell = true;
                if (_Qselected)
                    _Qselected.Play();
            }
            if(this.name == "Boy" && _ShieldCooldownTimer == 0)
            {
                isWspell = true;
                if (_Qselected)
                    _Qselected.Play();
            }
            CancelAOEAttack();

        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            //Debug.Log("SpellCommand: W pressed");
            if (this.name == "Girl" && _AOECooldownTimer == 0)
            {
                isWspell = true;
                if (_Wselected)
                    _Wselected.Play();
            }
            if(this.name == "Boy" && _StunCooldownTimer == 0)
            {
                isWspell = true;
                if (_Wselected)
                    _Wselected.Play();  
            }
            CancelHealAttack();

        }
        CastSpellW();
        CastSpellQ();

        //Cooldown timers to follow:
        //AOE Cooldown:
        if(_AOECooldownTimer < 0)
        {
            _AOECooldownTimer = 0;
        }
        if(_AOECooldownTimer > 0)
        {
            _AOECooldownTimer -= Time.deltaTime;
            _AOEUITimer = _AOECooldownTimer;
        }
        //Heal Cooldown
        if (_HealCooldownTimer < 0)
        {
            _HealCooldownTimer = 0;
        }
        if (_HealCooldownTimer > 0)
        {
            _HealCooldownTimer -= Time.deltaTime;
            _HealUITimer = _HealCooldownTimer;
        }
        //Stun Cooldown
        if (_StunCooldownTimer < 0)
        {
            _StunCooldownTimer = 0;
        }
        if (_StunCooldownTimer > 0)
        {
            _StunCooldownTimer -= Time.deltaTime;
            _StunUITimer = _StunCooldownTimer;
        }
        //Shield Cooldown
        if (_ShieldCooldownTimer < 0)
        {
            _ShieldCooldownTimer = 0;
        }
        if (_ShieldCooldownTimer > 0)
        {
            _ShieldCooldownTimer -= Time.deltaTime;
            _ShieldUITimer = _ShieldCooldownTimer;
        }

    }

    private void CastSpellQ() //used to call the spells connected to "Q"
    {
       
        if (isQspell)
        {
            player_number = CharacterSelect();
            //Debug.Log("SpellCommand: Q/player_number = " + player_number);
            //Boy spell called by Q (Shield)
            if (player_number == 0)
            {
                if (this.gameObject.name == "Boy") //checks if boy is casting and if this gamebobject is the boy
                {
                    //Spell goes here
                    //shield appears in front of boy in direction of mouse click (doesn't move)
                   // RangeIndicatorShield.SetActive(true);

                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.collider.tag == "RangeIndicator")
                        {
                            Vector3 directiondifference = hit.transform.position - this.transform.position;
                            //this.transform.LookAt(hit.transform, direction);
                            //Instantiate(Shield as GameObject);// This creates a shield in the place that I originally placed it in scene
                            //Shield.SetActive(true);
                            /* Notes:
                             * Want to create some sort of targetting arrow that follows mouse on first click
                             * Then spawn shield in the direction of the arrow on second click
                             * Leave shield where it is, don't need to move it
                             */
                        }
                    }
                }
                else
                {
                  
                }
                Debug.Log("SpellCommand: Boy cast Q");
            }
            //Girl spell called by Q (Heal)
            if ((player_number == 1 || isQGirlforced) && _HealCooldownTimer == 0 )
            {
                if (this.gameObject.name == "Girl" ) //checks if girl is casting and if this gamebobject is the girl
                {

                    //Spell goes here                  
                    RangeIndicatorHeal.SetActive(true);

                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.collider.tag == "RangeIndicator")
                        {
                            AttackIndicatorHeal.SetActive(true);
                            AttackIndicatorHeal.transform.position = new Vector3(hit.point.x, 1, hit.point.z);
                            if (Input.GetMouseButtonDown(0))
                            {
                                Healpos = new Vector3(hit.point.x, 1, hit.point.z);
                                Instantiate(Heal, Healpos, Quaternion.identity);
                                CancelHealAttack();
                                _HealCooldownTimer = _HealCooldown;
                            }
                        }
                        if (hit.collider.tag == "Ground")
                        {
                            AttackIndicatorHeal.SetActive(false);
                        }
                    }
                }
                else
                {
                    return;
                }
                Debug.Log("SpellCommand: Girl cast Q");
            }
        }
    }

    private void CastSpellW() ///used to call spells connected to "W"
    {
        if (isWspell)
        {
            player_number = CharacterSelect();
            //Debug.Log("SpellCommand: W/player_number = " + player_number);
            //Boy spell called by W (Stun)
            if (player_number == 0)
            {
                if (this.gameObject.name == "Boy") //checks if boy is casting and if this gamebobject is the boy
                {
                    //Spell goes here
                   
                }
                else
                {
                   
                }
                Debug.Log("SpellCommand: Boy cast W");
            }
            //Girl spell called by W (AOE)
            if ((player_number == 1 && _AOECooldownTimer ==0) || (isWGirlforced && _AOECooldownTimer == 0))
            {
                
                if (this.gameObject.name == "Girl") //checks if girl is casting and if this gamebobject is the girl
                {
                    RangeIndicatorAOE.SetActive(true);

                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.collider.tag == "RangeIndicator")
                        {
                            AttackIndicatorAOE.SetActive(true);
                            AttackIndicatorAOE.transform.position = new Vector3(hit.point.x, 1, hit.point.z);
                            if (Input.GetMouseButtonDown(0))
                            {
                                AOEpos = new Vector3(hit.point.x, 1, hit.point.z);
                                Instantiate(AOE, AOEpos, Quaternion.identity);
                                CancelAOEAttack();
                                _AOECooldownTimer = _AOECooldown;
                            }

                        }
                        if (hit.collider.tag == "Ground")
                        {
                            AttackIndicatorAOE.SetActive(false);
                        }
                    }
                }
                else
                {
                    return;
                }
            }
        }
    }

    private int CharacterSelect()//Determines which character is active
    {
        if (GameObject.FindWithTag("Player").GetComponent<BetterPlayer_Movement>().boyActive == true)
        {
            return 0;
        }
        else if (GameObject.FindWithTag("Player").GetComponent<BetterPlayer_Movement>().boyActive == false)
        {
            return 1;
        }
        else
        {
            return 2;
        }
    }

    public void CancelAOEAttack()
    {
        isWGirlforced = false;
        isWspell = false;
        if(_Wselected)
        _Wselected.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        if (RangeIndicatorAOE)
            RangeIndicatorAOE.SetActive(false);
        if(AttackIndicatorAOE)
            AttackIndicatorAOE.SetActive(false);
    }

    public void CancelHealAttack()
    {
        isQGirlforced = false;
        isQspell = false;
        if(_Qselected)
    _Qselected.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    if (RangeIndicatorHeal)
            RangeIndicatorHeal.SetActive(false);
        if (AttackIndicatorHeal)
            AttackIndicatorHeal.SetActive(false);
    }

    public void CastSpellQGirl()
    {
        isQGirlforced = true;
        isQspell = true;       
        CancelAOEAttack();
        _Qselected.Play();
     
    }

    public void CastSpellWGirl()
    {
        isWGirlforced = true;
        Debug.Log(isWGirlforced);
        isWspell = true;
        _Wselected.Play();
        CancelHealAttack();
    }

    public void CastSpellQBoy()
    {

    }

    public void CastSpellWBoy()
    {

    }
}