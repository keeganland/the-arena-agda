using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bullet : MonoBehaviour {

    public int Damage;
    public GameObject _SpellFlare;
    public Color _DamageColor;
  

    private Vector3 m_angle;
    [SerializeField]
    private GameObject _SpellCaster;
    [SerializeField]
    private int AggroValue;
    

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.name);
        if (_SpellCaster)
        {
            //Debug.Log("BULLET: _SpellCaster = " + _SpellCaster.name);
        }
        if (other.gameObject.CompareTag("AttackRange"))
        {
            return;
        }
        if (other.gameObject.CompareTag("Player") && _SpellCaster != other.gameObject)
        {
            DamageData dmgData = new DamageData();
            dmgData.damage = Damage;
            /*AggroData aggroData = new AggroData();
            aggroData.aggro = AggroValue;*/
            DisplayDamage(other.gameObject, _DamageColor, Damage);
            MessageHandler msgHandler = other.GetComponent<MessageHandler>();

            if (msgHandler)
            {
                msgHandler.GiveMessage(MessageTypes.DAMAGED, this.gameObject, dmgData);
                //msgHandler.GiveMessage(MessageTypes.AGGROCHANGED, _SpellCaster, aggroData);
                //Debug.Log("Bullet: this = " + this.name);
                //Debug.Log("BULLET: _SpellCaster = " + _SpellCaster.name);
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
        AggroData aggroData = new AggroData();
        aggroData.aggro = AggroValue;
        MessageHandler msgHandler = collision.gameObject.GetComponent<MessageHandler>();

        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("wall"))
        {
            DestroyObject();
            DoSpellFlare();
            if (collision.gameObject.CompareTag("Enemy") && _SpellCaster != collision.gameObject)
            {
                DisplayDamage(collision.gameObject, _DamageColor, Damage);
            }
        }
        if(msgHandler || collision.gameObject.CompareTag("Enemy"))
        {
            msgHandler.GiveMessage(MessageTypes.AGGROCHANGED, _SpellCaster, aggroData);
          //  Debug.Log("Bullet: gave AGGRO message");
           // Debug.Log("BULLET: _Spellcaster is : " + _SpellCaster.name);
          //  Debug.Log("BULLET: aggroData is: " + aggroData.aggro);
          //  Debug.Log("BULLET: collision with: " + collision.gameObject.name);
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

    private void DisplayDamage(GameObject targetdisplay, Color damageColor, int damageText)
    {
            GameObject go = targetdisplay.GetComponent<HealthController>().Sprite;
            Canvas canvas = go.GetComponentInChildren<Canvas>();
            canvas.GetComponentInChildren<DamageDisplayScript>().GetDamageText(damageColor, damageText);
    }
}
