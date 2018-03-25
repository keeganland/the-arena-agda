﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HealthController : MonoBehaviour
{

    public int totalHealth = 100;
    public int currentHealth;
    public GameObject enemy;

    public int AggroBoy;
    public int AggroGirl;

    NavMeshAgent agent; //Why is this here?
    public bool isTargeted;

    MessageHandler m_messageHandler;

    private void Start()
    {
        AggroBoy = 0;
        AggroGirl = 0;
        currentHealth = totalHealth;
        agent = GetComponent<NavMeshAgent>(); //?????
        m_messageHandler = GetComponent<MessageHandler>();

        if (m_messageHandler)
        {
            m_messageHandler.RegisterDelegate(RecieveMessage);
        }
    }

    void RecieveMessage(MessageTypes msgType, GameObject go, MessageData msgData)
    {
        switch (msgType)
        {
            case MessageTypes.DAMAGED:
                DamageData dmgData = msgData as DamageData;

                if (dmgData != null)
                {
                    ApplyDamage(dmgData.damage, go);
                }
                break;
             case MessageTypes.AGGROCHANGED:
                Debug.Log("HealthController: changing Aggro");
                AggroData aggroData = msgData as AggroData;
                Debug.Log("HealthController: Aggro"); //This isn't working. - Works when I remove the "+ aggroData.attacker.name" part
                //issue is within how I'm calling from aggroData
                if (aggroData.attacker.tag == "Player") //should be .name and "Boy", but this isn't activating
                {
                    AggroBoy += aggroData.aggro;
                    Debug.Log(AggroBoy);
                }
                else if(aggroData.attacker.name == "Girl")
                {
                    AggroGirl += aggroData.aggro;
                }
                AggroBoy += aggroData.aggro;//This doesn't work in itself. Need to change within if statements
                break;
        }
    }

    public void ApplyDamage(int damage, GameObject go)
    {
        currentHealth -= damage;

        //Will need to change this if statement, pretty sure I need to remove the spawn part
        if (currentHealth <= 0f /*&& GameObject.Find("Cube-Spawn").GetComponent<HealthController>().currentHealth > 0*/)
        {
            currentHealth = 0;

            if (m_messageHandler)
            {
                DeathData deathData = new DeathData();
                deathData.attacker = go;
                deathData.attacked = gameObject;

                m_messageHandler.GiveMessage(MessageTypes.DIED, gameObject, deathData);
            }

            Destroy(this.gameObject);//This works

            //agent.enabled = false; //this is from the original script. Don't think it's remotely related
            // transform.position = enemy.GetComponent<enermy_movement>().spawnPoint.position;

            //Think this is related to respawn?
            /*currentHealth = totalHealth;
            agent.enabled = true;*/

        }
        else if (currentHealth <= 0f && enemy.tag != ("Enemy Boss"))
        {
            Destroy(gameObject);
        }
        else if(currentHealth >= 0f && currentHealth <= totalHealth)
        {
            this.GetComponent<HealthUI>().UpdateUi(totalHealth, currentHealth);
        }

        if (m_messageHandler)
        {
            HealthData hpData = new HealthData();

            hpData.maxHealth = totalHealth;
            hpData.curHealth = currentHealth;

            m_messageHandler.GiveMessage(MessageTypes.HEALTHCHANGED, gameObject, hpData);
        }
    }
}