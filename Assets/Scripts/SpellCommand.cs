using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCommand : MonoBehaviour {

    public PublicVariableHolderneverUnload _PublicVariableHolder;

    private int player_number;//variable used to hold value of which character is casting a spell
    private bool isWspell = false;
    private bool isQspell = false;
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
    public GameObject ShieldRangeIndicator;
    public GameObject ShieldDirectionIndicator;

    public ParticleSystem _Qselected;
    public ParticleSystem _Wselected;
    public ParticleSystem _SmallQselected;
    public ParticleSystem _SmallWselected;

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
            _AOECooldown = 6;
        }
        if(_HealCooldown == 0)
        {
            _HealCooldown = 7;
        }
        if(_ShieldCooldown == 0)
        {
            _ShieldCooldown = 12;
        }
        if(_StunCooldown == 0)
        {
            _StunCooldown = 15;
        }

        _Qselected.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        _Wselected.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        _SmallQselected.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        _SmallWselected.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Q) && !_PublicVariableHolder.StopAllActions) //could switch to GetButtonDown later to allow player to customise controls
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
                isQspell = true;
                if (_Qselected)
                    _Qselected.Play();
            }
            CancelAOEAttack();

        }
        if (Input.GetKeyDown(KeyCode.W) && !_PublicVariableHolder.StopAllActions)
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

                    ShieldRangeIndicator.SetActive(true);

                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.collider.tag == "RangeIndicator")
                        {
                            ShieldDirectionIndicator.SetActive(true);

                            float zpos = this.transform.position.z;//in case the click is directly in line
                            float xpos = this.transform.position.x;
                            //Create new system for this. Read how far away the x value is, make it that far off boy unless it exceeds a certain max
                            //Do the same for z
                            //This may cause issues regarding Needing to spawn the shield a certain minimum distance away so it isn't on top of boy
                            //This distance will be much smaller when using the -20x rotation
                            if (this.transform.position.x > hit.point.x)//adapting to try to move shield off character depending on where the click is
                            {
                                if (this.transform.position.x - hit.point.x >= 1.5)
                                {
                                    xpos = this.transform.position.x - (float)1.5;//Need to decrease this when the click is closer
                                }
                                else
                                {
                                    xpos = hit.point.x;
                                }
                            }//Adapting for whether the click is less than or more than 1.5
                            if (this.transform.position.x < hit.point.x)
                            {
                                if (hit.point.x - this.transform.position.x >= 1.5)
                                {
                                    xpos = this.transform.position.x + (float)1.5;
                                }
                                else
                                {
                                    xpos = hit.point.x;
                                }
                            }
                            if (this.transform.position.z > hit.point.z)//adapting to try to move shield off character depending on where the click is
                            {
                                if (this.transform.position.z - hit.point.z >= 1.5)
                                {
                                    zpos = this.transform.position.z - (float)1.5;
                                }
                                else
                                {
                                    zpos = hit.point.z;
                                }
                            }
                            if (this.transform.position.z < hit.point.z)
                            {
                                if (hit.point.z - this.transform.position.z >= 1.5)
                                {
                                    zpos = this.transform.position.z + (float)1.5;
                                }
                                else
                                {
                                    zpos = hit.point.z;
                                }
                            }

                            Vector3 difference = new Vector3(hit.point.x, this.transform.position.y, hit.point.z) - this.transform.position;//difference btw hit point and boy
                            Vector3 RotationRectangle = new Vector3(this.transform.position.x, 1, this.transform.position.z);//difference btw hit point and boy
                            Vector3 PositionRectangle = new Vector3(hit.point.x, this.transform.position.y, hit.point.z) - this.transform.position;
                            Debug.Log(PositionRectangle);
                            PositionRectangle.y = 1;
                            PositionRectangle.Normalize();
                            ShieldDirectionIndicator.transform.position = PositionRectangle * 2f + new Vector3(transform.position.x, 0 , transform.position.z);

                            //float angle = Mathf.Atan2(difference.x, difference.z) * Mathf.Rad2Deg;
                            //// apply that rotation to the Z axis.
                            //ShieldDirectionIndicator.transform.localRotation = Quaternion.Euler(0f, angle, 0);
                            ShieldDirectionIndicator.transform.LookAt(RotationRectangle);
                            //ShieldDirectionIndicator.transform.localRotation = Quaternion.Euler(90, 0, ShieldDirectionIndicator.transform.localRotation.z);
                            if (Input.GetMouseButtonDown(0))
                            {                     
                                Vector3 sheildpos = new Vector3(xpos, 0, zpos);

                                GameObject shields = Instantiate(Shield, sheildpos, Quaternion.LookRotation(difference));//Need to assign a rotaion of -20x
                                shields.transform.SetParent(null);
                                Destroy(shields, 10.0f);
                                CancelBoyShield();
                                _ShieldCooldownTimer = _ShieldCooldown;
                            }
                        }
                    }
                }
           
                Debug.Log("SpellCommand: Boy cast Q");
            }
            //Girl spell called by Q (Heal)
            if ((player_number == 1 && _HealCooldownTimer == 0) || (isQGirlforced && _HealCooldownTimer == 0)) //What is "isQGirlForeced" and is the _HealCooldownTimer == 0 part necessary? This is checked in update
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
                    isWspell = false;
                    //Stop Coroutines
                    //Make stop attacking (FirstEnemyAttack2) true
                    //Trigger the event
                   
                }
            
                Debug.Log("SpellCommand: Boy cast W");
            }
            //Girl spell called by W (AOE)
            if ((player_number == 1 && _AOECooldownTimer ==0) || (isWGirlforced && _AOECooldownTimer == 0)) //What is "isWGirlforced"? And again, the timer is checked in update
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
        _SmallWselected.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
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
        _SmallQselected.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        if (RangeIndicatorHeal)
            RangeIndicatorHeal.SetActive(false);
        if (AttackIndicatorHeal)
            AttackIndicatorHeal.SetActive(false);
    }

    public void CancelBoyShield()
    {
        isQspell = false;
        if(_Qselected)
        _Qselected.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        _SmallQselected.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        if (RangeIndicatorShield)
            RangeIndicatorShield.SetActive(false);
        if (ShieldDirectionIndicator)
            ShieldDirectionIndicator.SetActive(false);
        if (ShieldRangeIndicator)
            ShieldRangeIndicator.SetActive(false);
    }

    public void CastSpellQGirl(bool isBig) //When is this used? What does this do?
    {
                                           //Alex answer : It's the function called when you click on the big spell (UI)        
            Debug.Log(isBig);
            if (isBig)
            {
                _Qselected.Play();
            }
            else
            {
                Debug.Log("I am here");
                _SmallQselected.Play();
            }
            isQGirlforced = true;
            isQspell = true;   //This is done in update?     //Alex answer : it also need to be done when you click on the UI, otherwise it won't work
            CancelAOEAttack();
    
    }

    public void CastSpellWGirl(bool isBig)
    {  
            if (isBig)
            {
                _Wselected.Play();
            }
            else
            {
                _SmallWselected.Play();
            }
            isWGirlforced = true;
            Debug.Log(isWGirlforced);
            isWspell = true;
            CancelHealAttack();      
    }

    public void CastSpellQBoy(bool isBig)
    {
            if (isBig)
            {
                _Qselected.Play();
            }
            else
            {
                _SmallQselected.Play();
            }
            
            isQspell = true;       
    }

    public void CastSpellWBoy(bool isBig)
    {      
            if (isBig)
            {
                _Wselected.Play();
            }
            else
            {
                _SmallWselected.Play();
            }

            isWspell = true;       
    }
}