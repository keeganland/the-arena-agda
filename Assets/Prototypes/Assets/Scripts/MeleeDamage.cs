using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeDamage : MonoBehaviour {

    public int Damage;
    public float AttackSpeed;
    private float AttackTimer;
    public GameObject target;

	// Use this for initialization
	void Start () {
        //Gets target on click from Player_Movement
        target = GetComponent<Player_Movement>().curTarget;
        //This doesn't seem to be going through
	}
	
	// Update is called once per frame
	void Update () {
        //This script doesn't seem to be working... Why not.................?
        Debug.Log("MeleeDamage: Damage Target" + target.name);
        AttackTimer += Time.deltaTime; //including this although I'm not sure I have to. Should the animation Timer do this for me?
        if(AttackTimer >= AttackSpeed && (this.GetComponent<RangeChecker>().InRange(target)))
        {
            DamageData dmgData = new DamageData();
            dmgData.damage = Damage;

            Debug.Log("MeleeDamage: deal damage to " + target.name);

            //Really need to make sure MessageHandler is on all enemies and players
            MessageHandler msgHandler = target.GetComponent<MessageHandler>();

            if (msgHandler)
            {
                msgHandler.GiveMessage(MessageTypes.DAMAGED, this.gameObject/*will this work?*/, dmgData);
            }
            AttackTimer = 0.0f;
        }
    }
}
