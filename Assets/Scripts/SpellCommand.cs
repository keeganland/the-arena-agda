using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCommand : MonoBehaviour {
    /*NEED TO INCORPORATE TIMER*/
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

	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Q)) //could switch to GetButtonDown laster to allow player to customise controls
        {
            //Debug.Log("SpellCommand: Q pressed");
            isQspell = true;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            //Debug.Log("SpellCommand: W pressed");
            isWspell = true;
            
        }
        CastSpellW();
        CastSpellQ();
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
                    RangeIndicatorShield.SetActive(true);

                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.collider.tag == "RangeIndicator")
                        {
                            Vector3 direction = hit.transform.position - this.transform.position;
                            //this.transform.LookAt(hit.transform, direction);
                            //Instantiate(Shield as GameObject);// This creates a shield in the place that I originally placed it in scene
                            //Shield.SetActive(true);
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
            if (player_number == 1)
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
            if (player_number == 1)
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