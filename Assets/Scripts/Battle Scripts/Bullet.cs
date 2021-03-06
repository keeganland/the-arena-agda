﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bullet : MonoBehaviour
{

    public bool isHeal;
    public int Damage;
    public GameObject _SpellFlare;
    public Color _DamageColor;
    public Color _HealColor;
    public int Healing;



    private Vector3 m_angle;
    [SerializeField]
    private GameObject _SpellCaster;
    [SerializeField]
    private int AggroValue;


    /* Keegan:
     * This function feels kind of smelly to me. Flagging for refactor
     */
    private void OnTriggerEnter(Collider other)
    {
        string spellCasterTag = "";
        if (_SpellCaster)
        {
            spellCasterTag = _SpellCaster.tag;
        }

        if (other.gameObject.CompareTag("AttackRange"))
        {
            return;
        }

        //if ((other.gameObject.CompareTag("Player") && _SpellCaster.tag != "Player") || other.gameObject.CompareTag("Enemy"))
        if ((other.gameObject.tag == "Player/Boy" || other.gameObject.tag == "Player/Girl") || other.gameObject.tag == "Enemy")
        {
            MessageHandler msgHandler = other.GetComponent<MessageHandler>();

            if (isHeal)
            {
                // Debug.Log("Bullet: isHeal = true");
                RecoverData rcvrData = new RecoverData();
                rcvrData.HP_up = Healing;
                if (msgHandler)
                {
                    msgHandler.GiveMessage(MessageTypes.HEALED, this.gameObject, rcvrData);
                    DisplayHealing(other.gameObject, _HealColor, Healing);
                }
            }

            if ((spellCasterTag == "Player/Boy" || spellCasterTag == "Player/Girl") && (other.gameObject.tag == "Player/Boy" || other.gameObject.tag == "Player/Girl"))
            {
                return;
            }

            Debug.Log("I am here after the sheep attack against : " + other.tag + " with SpellCaster of : " + spellCasterTag);

            if (!isHeal)
            {
                DamageData dmgData = new DamageData();
                dmgData.damage = Damage;
                if (msgHandler)
                {
                    msgHandler.GiveMessage(MessageTypes.DAMAGED, this.gameObject, dmgData);
                    Debug.Log("Bullet check: Other's Tag = " + other.tag);
                    Debug.Log("Bullet Check: Spellcaster's Tag = " + _SpellCaster.tag);
                    DisplayDamage(other.gameObject, _DamageColor, Damage);
                }
            }
        }
        if (other.gameObject.CompareTag("wall"))
        {
            DestroyObject();
        }
        else return;
    }

    void OnCollisionEnter(Collision collision)
    {
        AggroData aggroData = new AggroData();
        aggroData.aggro = AggroValue;
        MessageHandler msgHandler = collision.gameObject.GetComponent<MessageHandler>();

        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("wall") || collision.gameObject.CompareTag("Objects"))
        {
            DestroyObject();
            DoSpellFlare();
            if (collision.gameObject.CompareTag("Enemy") && _SpellCaster != collision.gameObject)
            {
                DisplayDamage(collision.gameObject, _DamageColor, Damage);
            }
        }
        else if ((collision.gameObject.CompareTag("Player/Boy") || collision.gameObject.CompareTag("Player/Girl")) && _SpellCaster.gameObject.tag == "Enemy")
        {
            DestroyObject();
            DoSpellFlare();
            if ((collision.gameObject.CompareTag("Player/Boy") || collision.gameObject.CompareTag("Player/Girl")) && _SpellCaster != collision.gameObject)
            {
                DisplayDamage(collision.gameObject, _DamageColor, Damage);
            }
        }

        if (msgHandler || collision.gameObject.CompareTag("Enemy"))
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

    public void SetSpellCaster(GameObject caster)
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
        Canvas[] canvas = go.GetComponentsInChildren<Canvas>();
        if (canvas.Length != 0)
        {
            for (int i = 0; i < canvas.Length; i++)
            {
                if (canvas[i].GetComponentInChildren<DamageDisplayScript>())
                    canvas[i].GetComponentInChildren<DamageDisplayScript>().GetDamageText(damageColor, damageText);
            }
        }
    }

    private void DisplayHealing(GameObject targetdisplay, Color healingColor, int healingText)
    {
        GameObject go = targetdisplay.GetComponent<HealthController>().Sprite;
        Canvas[] canvas = go.GetComponentsInChildren<Canvas>();
        if (canvas.Length != 0)
        {
            for (int i = 0; i < canvas.Length; i++)
            {
                if (canvas[i].GetComponentInChildren<DamageDisplayScript>())
                    canvas[i].GetComponentInChildren<DamageDisplayScript>().GetDamageText(healingColor, healingText);
            }
        }
    }
}
