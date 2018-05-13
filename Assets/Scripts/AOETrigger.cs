using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOETrigger : MonoBehaviour {

    private float AOETimer;
    public int AOEDamage;

	// Use this for initialization
	void Start () {
        AOETimer = 1;
	}
	
	// Update is called once per frame
	void Update () {
        AOETimer += Time.deltaTime;
        Debug.Log("AOE Trigger: Timer is " + AOETimer);

        if (AOETimer > 1) //Note: AOETimer counts up and disengages at 1
        {
            //should destroy AOE here
            this.GetComponentInParent<AOESelfDestruct>(AOEDestruct); 
        }
    }

    private void OnTriggerStay(Collider other) //If I am going to use Stay I need to ensure they aren't continually damaged. Otherwise try Enter
    {
        if (AOETimer > 1.0) 
        {
            return;
        }
        else //if (other.gameObject.tag == "Enemy")
        {
            DamageData dmgData = new DamageData();
            dmgData.damage = AOEDamage;

            MessageHandler msgHandler = other.GetComponent<MessageHandler>();

            msgHandler.GiveMessage(MessageTypes.DAMAGED, other.gameObject, dmgData); //Damages "other" which has to be tagged with enemy
        }
    }

    public void SetAOETimer(float Time)
    {
        AOETimer = Time;
    }
}
