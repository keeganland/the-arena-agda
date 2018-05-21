using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCommand : MonoBehaviour {
    private int player_number;//variable used to hold value of which character is casting a spell
    private bool isWspell;
    private bool isQspell;
    private Vector3 AOEpos;
    private Vector3 Healpos;

    public int Healing;
    public GameObject Shield;
    public GameObject AOE;
    public GameObject RangeIndicatorAOE;
    public GameObject AttackIndicatorAOE;
    public GameObject AttackIndicatorHeal;
    public GameObject RangeIndicatorHeal;
    public GameObject Heal;
    public GameObject RangeIndicatorShield;

    /*Timer variables:*/
    public int AOE_Cooldown;
    private float AOE_Cooldown_Timer = 0;
    private int AOE_UI_Timer; //for Alex
    public int Heal_Cooldown;
    private float Heal_Cooldown_Timer = 0;
    private int Heal_UI_Timer; //for Alex
    public int Shield_Cooldown;
    private float Shield_Cooldown_Timer = 0;
    private int Shield_UI_Timer; //for Alex
    public int Stun_Cooldown;
    private float Stun_Cooldown_Timer = 0;
    private int Stun_UI_Timer; //for Alex

	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Q)) //could switch to GetButtonDown laster to allow player to customise controls
        {
            //Debug.Log("SpellCommand: Q pressed");
            isQspell = true;
            CancelAOEAttack();

        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            //Debug.Log("SpellCommand: W pressed");
            isWspell = true;
            CancelHealAttack();

        }
        CastSpellW();
        CastSpellQ();

        //Cooldown timers to follow:
        //AOE Cooldown:
        if(AOE_Cooldown_Timer < 0)
        {
            AOE_Cooldown_Timer = 0;
        }
        if(AOE_Cooldown_Timer > 0)
        {
            AOE_Cooldown_Timer -= Time.deltaTime;
            AOE_UI_Timer = (int)AOE_Cooldown_Timer;
        }
        //Heal Cooldown
        if (Heal_Cooldown_Timer < 0)
        {
            Heal_Cooldown_Timer = 0;
        }
        if (Heal_Cooldown_Timer > 0)
        {
            Heal_Cooldown_Timer -= Time.deltaTime;
            Heal_UI_Timer = (int)Heal_Cooldown_Timer;
        }
        //Stun Cooldown
        if (Stun_Cooldown_Timer < 0)
        {
            Stun_Cooldown_Timer = 0;
        }
        if (Stun_Cooldown_Timer > 0)
        {
            Stun_Cooldown_Timer -= Time.deltaTime;
            Stun_UI_Timer = (int)Stun_Cooldown_Timer;
        }
        //Shield Cooldown
        if (Shield_Cooldown_Timer < 0)
        {
            Shield_Cooldown_Timer = 0;
        }
        if (Shield_Cooldown_Timer > 0)
        {
            Shield_Cooldown_Timer -= Time.deltaTime;
            Shield_UI_Timer = (int)Shield_Cooldown_Timer;
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
                    return;
                }
                Debug.Log("SpellCommand: Boy cast Q");
            }
            //Girl spell called by Q (Heal)
            if (player_number == 1 && Heal_Cooldown_Timer == 0)
            {
                if (this.gameObject.name == "Girl") //checks if girl is casting and if this gamebobject is the girl
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
                                Heal_Cooldown_Timer = Heal_Cooldown;
                            }
                            if (hit.collider.tag == "Ground")
                            {
                                AttackIndicatorHeal.SetActive(false);
                            }
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
                    return;
                }
                Debug.Log("SpellCommand: Boy cast W");
            }
            //Girl spell called by W (AOE)
            if (player_number == 1 && AOE_Cooldown_Timer ==0)
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
                                AOE_Cooldown_Timer = AOE_Cooldown;
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
                Debug.Log("SpellCommand: Girl cast W");
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
        
        isWspell = false;
        if(RangeIndicatorAOE)
            RangeIndicatorAOE.SetActive(false);
        if(AttackIndicatorAOE)
            AttackIndicatorAOE.SetActive(false);
    }

    public void CancelHealAttack()
    {
        isQspell = false;
        if (RangeIndicatorHeal)
            RangeIndicatorHeal.SetActive(false);
        if (AttackIndicatorHeal)
            AttackIndicatorHeal.SetActive(false);
    }
}