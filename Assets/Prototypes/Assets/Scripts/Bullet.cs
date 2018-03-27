using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public int Damage;
    public GameObject _SpellFlare;
  

    private Vector3 m_angle;
    [SerializeField]
    private GameObject _SpellCaster;
    private int AggroValue;
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("AttackRange"))
        {
            return;
        }
        if (other.gameObject.CompareTag("Player") && !_SpellCaster)
        {                              
            DamageData dmgData = new DamageData();
            dmgData.damage = Damage;
            AggroData aggroData = new AggroData();
            aggroData.aggro = AggroValue;

            MessageHandler msgHandler = other.GetComponent<MessageHandler>();

            if (msgHandler)
            {
                msgHandler.GiveMessage(MessageTypes.DAMAGED, this.gameObject, dmgData);
                msgHandler.GiveMessage(MessageTypes.AGGROCHANGED, _SpellCaster, aggroData);
                //Debug.Log("MeleeDamage: this = " + this.name);
            }
        }
        if (other.gameObject.CompareTag("wall"))
        {

            DestroyObject();
        }
        else return;
    }
    void OnCollisionEnter (Collision collision)
    {

        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("wall"))
        {
            DestroyObject();
            DoSpellFlare();
          
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

    public void GetSpellCaster (GameObject caster)
    {
        _SpellCaster = caster;
    }

    private void DoSpellFlare()
    {
        if (_SpellFlare)
        {
            GameObject _Spell = Instantiate(_SpellFlare, transform.position, Quaternion.Euler(m_angle));
        }
    }

    public void GetAggro(int Aggro)
    {
        AggroValue = Aggro;
        //Debug.Log("Bullet: changed Aggro");
    }
}
