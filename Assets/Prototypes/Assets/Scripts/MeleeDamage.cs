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
	}
	
	// Update is called once per frame
	void Update () {
        AttackTimer += Time.deltaTime; //including this although I'm not sure I have to. Should the animation Timer do this for me?
        if(AttackTimer >= AttackSpeed)
        {
            DamageData dmgData = new DamageData();
            dmgData.damage = Damage;

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
