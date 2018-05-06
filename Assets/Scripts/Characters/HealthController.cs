using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HealthController : MonoBehaviour
{

    public int totalHealth = 100;
    public int currentHealth;
    public GameObject enemy;
    private int tempHealth;

    public AudioClip _DeathSound;

    private AudioSource m_audioSource;

    public GameObject Sprite;

    public int AggroBoy = 0;
    public int AggroGirl = 0;

    NavMeshAgent agent; //Why is this here? (it doesn't seem to be used)
    public bool isTargeted;

    MessageHandler m_messageHandler;

    private void Start()
    {
        currentHealth = totalHealth;
        m_messageHandler = GetComponent<MessageHandler>();

        if (m_messageHandler)
        {
            m_messageHandler.RegisterDelegate(RecieveMessage);
        }

        m_audioSource = GetComponent<AudioSource>();
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
                //Debug.Log("HealthController: changing Aggro");
                AggroData aggroData = msgData as AggroData;
                //Debug.Log("Healthcontroller: AGGROCHANGED: go = " + go.name);
                //Debug.Log("Healthcontroller: AGGROCHANGED on: " + this.name);
                //Debug.Log(aggroData.aggro);
                //Debug.Log("Healthcontroller: go name is" + go.name);
                if (go.name == "Boy")
                {
                    AggroBoy += aggroData.aggro; //Need to establish Aggro for each character on all enemies
                }
                else if(go.name == "Girl")
                {
                    //Debug.Log("here");
                    AggroGirl += aggroData.aggro;
                }
                 break;
            case MessageTypes.HEALED: //for healing character
                RecoverData recoverData = msgData as RecoverData;
                if(recoverData != null)
                {
                    RecoverHealth(recoverData.HP_up, go);
                }
                break;
        }
    }

    public void ApplyDamage(int damage, GameObject go)
    {
        currentHealth -= damage;
        

        //Will need to change this if statement, pretty sure I need to remove the spawn part
        if (currentHealth <= 0f /*&& GameObject.Find("Cube-Spawn").GetComponent<HealthController>().currentHealth > 0*/)
        {
            PlaySoundOnKill();
            currentHealth = 0;

            if (m_messageHandler)
            {
                DeathData deathData = new DeathData();
                deathData.attacker = go;
                deathData.attacked = gameObject;

                m_messageHandler.GiveMessage(MessageTypes.DIED, gameObject, deathData);
            }

            if(transform.parent != null)
            {
                if (gameObject.GetComponentInParent<TotemDestroyed>())
                {
                    gameObject.GetComponentInParent<TotemDestroyed>()._IsDestroyed = true;
                }
            }
            VictoryScreen.youWon = true;
            Debug.Log("This is HealthController.cs, youWon should have just flipped");
            this.gameObject.SetActive(false);//This works
            Sprite.SetActive(false);

            //agent.enabled = false; //this is from the original script. Don't think it's remotely related
            // transform.position = enemy.GetComponent<enermy_movement>().spawnPoint.position;

            //Think this is related to respawn?
            /*currentHealth = totalHealth;
            agent.enabled = true;*/

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

    private void PlaySoundOnKill()
    {
        if (_DeathSound)
        {
            m_audioSource.PlayOneShot(_DeathSound);
        }
    }

    public void RecoverHealth(int HP_up, GameObject go) //Applies healing to character
    {
        tempHealth = HP_up + currentHealth;
        if(tempHealth > totalHealth) //checks to make sure character doesn't go over max health
        {
            currentHealth = totalHealth;
        }
        else if(tempHealth <= totalHealth) //applies healing if health is still below max health
        {
            currentHealth = tempHealth;
        }
    }
}