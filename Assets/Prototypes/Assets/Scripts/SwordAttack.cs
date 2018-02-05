using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour {
    GameObject m_target;
    GameObject m_attacker;
    public int damage;

	// Use this for initialization
	void Start () {
        //this section is trying to assign something to target. THIS WILL NOT WORK
       /* RaycastHit hit;
        if(hit.collider.tag == "Enemy")
        {
            hit.collider = m_target;
        }*/

        //if (collision.gameObject == m_target)//need to generate target according to how we attack
        if (m_target.CompareTag("Enemy"))
        {
            DamageData dmgData = new DamageData();
            dmgData.damage = damage;

            MessageHandler msgHandler = m_target.GetComponent<MessageHandler>();

            if (msgHandler)
            {
                msgHandler.GiveMessage(MessageTypes.DAMAGED, m_attacker, dmgData);
            }

        }
    }
}
