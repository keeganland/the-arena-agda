using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public int Damage;
    public GameObject _SpellFlare;

    private Vector3 m_angle;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
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
        if(other.gameObject.CompareTag("wall"))
        {
            DestroyObject();
        }
    }
    void OnCollisionEnter (Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("wall"))
        {
            DestroyObject();
            DoSpellFlare(collision);
          
        }
    }

    void DestroyObject()
    {     
          Destroy(gameObject, 2.5f);
            
    }    

    public void SpellFlare(float angle)
    {
       // Debug.Log(angle);
        if (_SpellFlare)
        {
            m_angle = new Vector3(0, 180 - angle, 0);
        }
    }

    private void DoSpellFlare(Collision col)
    {
        GameObject _Spell = Instantiate(_SpellFlare, transform.position, Quaternion.Euler(m_angle));
    }
}
