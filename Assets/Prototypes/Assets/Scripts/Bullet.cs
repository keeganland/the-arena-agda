using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public int Damage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("AttackRange"))
        {
            return;
        }
        if (other.gameObject.CompareTag("Player"))
        {

            DamageData dmgData = new DamageData();
            dmgData.damage = Damage;

            MessageHandler msgHandler = other.GetComponent<MessageHandler>();

            if (msgHandler)
            {
                msgHandler.GiveMessage(MessageTypes.DAMAGED, this.gameObject, dmgData);
                //msgHandler.GiveMessage(MessageTypes.AGGROCHANGED, this.gameObject, aggroData);
                //Debug.Log("MeleeDamage: this = " + this.name);
            }
        }
    }
    void OnCollisionEnter (Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("wall"))
        {
            Destroy(gameObject, 2.5f);
        }
    }
}
