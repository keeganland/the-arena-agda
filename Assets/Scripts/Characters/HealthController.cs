using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{

    public int totalHealth = 100;
    public int currentHealth;
    public GameObject enemy;
    private int tempHealth;

    public AudioClip _DeathSound;

    private AudioSource m_audioSource;

    public GameObject Sprite;
    public GameObject _Slider;
    public GameObject _DeathAnim;

    public int AggroBoy = 0;
    public int AggroGirl = 0;

    NavMeshAgent agent; //Why is this here? (it doesn't seem to be used)
    public bool isTargeted;

    MessageHandler m_messageHandler;

    public bool isBoss; //KeeganNTS: temporary victory conditions. Deprecate with later design

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

    private void Update()
    {
        if (isBoss && (currentHealth <= 0))
        {
            //Keegan NTS: victory shenanigans
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
                    //Debug.Log("HealthController: case HEALED");
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
           // PlaySoundOnKill();
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

            /*
             * Keegan NTS:
             * 
             * The stuff below is a temporary bandaid for setting off the victory condition. We are to 
             */

            //VictoryScreen.youWon = true;
            //Debug.Log("This is HealthController.cs, youWon should have just flipped");
            //this.gameObject.SetActive(false);//This works
            if(this.gameObject.tag == "Player")
            DoDeath();

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

    private void DoDeath() {

        Sprite.GetComponent<SpriteRenderer>().enabled = false;
        if (_Slider)
            _Slider.SetActive(false);
        if (_DeathAnim)
            _DeathAnim.SetActive(true);

        if (this.gameObject.name == "Boy")
        {
            GameObject.Find("Girl").GetComponent<BetterPlayer_Movement>().SwapGirl();
            gameObject.GetComponent<BetterPlayer_Movement>().SwapGirl();
        }
        if (this.gameObject.name == "Girl")
        {
            GameObject.Find("Boy").GetComponent<BetterPlayer_Movement>().SwapBoy();
            gameObject.GetComponent<BetterPlayer_Movement>().SwapBoy();
        }
        this.gameObject.GetComponent<BetterPlayer_Movement>().enabled = false;//This works
    }

    public void UndoDeath()
    {
        Sprite.GetComponent<SpriteRenderer>().enabled = true;
        if (_Slider)
            _Slider.SetActive(true);
        if (_DeathAnim)
            _DeathAnim.SetActive(false);

        this.gameObject.GetComponent<BetterPlayer_Movement>().enabled = true;//This works

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
        if (tempHealth > totalHealth) //checks to make sure character doesn't go over max health
        {
            currentHealth = totalHealth;
        }
        else if(tempHealth <= totalHealth) //applies healing if health is still below max health
        {
            currentHealth = tempHealth;
        }
        if (currentHealth >= 0f && currentHealth <= totalHealth)
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

    private bool m_reviveCoroutine;
    private bool m_reviveCoroutineisStarted;
    private float m_castTime;
    private bool isReviving;

    public GameObject _CastReviveGameobject;
    public Slider _ReviveSlider;
    public Text _ReviveTextTimer;

    public int _ReviveCD;

    private IEnumerator ReviveByClicking()
    {
        m_reviveCoroutineisStarted = true;
        m_castTime = 0;
        _CastReviveGameobject.SetActive(true);
        _ReviveTextTimer.enabled = true;
        isReviving = true;
        yield return new WaitForSeconds(_ReviveCD);

        if(this.gameObject.name == "Boy")
        {
            GameObject.Find("Girl").GetComponent<HealthController>().currentHealth = GameObject.Find("Girl").GetComponent<HealthController>().totalHealth;
            GameObject.Find("Girl").GetComponent<HealthController>().UndoDeath();
        }
        if (this.gameObject.name == "Girl")
        {
            GameObject.Find("Boy").GetComponent<HealthController>().currentHealth = GameObject.Find("Boy").GetComponent<HealthController>().totalHealth;
            GameObject.Find("Boy").GetComponent<HealthController>().UndoDeath();
        }
        isReviving = false;
        _CastReviveGameobject.SetActive(false);
        _ReviveTextTimer.enabled = false;
        m_reviveCoroutineisStarted = false;
        m_reviveCoroutine = false;
    }

    private void FixedUpdate()
    {
        if(!m_reviveCoroutineisStarted && m_reviveCoroutine)
        {
            StartCoroutine("ReviveByClicking");
        }
        if (isReviving == true)
        {
            if (m_castTime < _ReviveCD)
            {
                m_castTime += Time.deltaTime;
            }
            _ReviveSlider.value = m_castTime / _ReviveCD;
            _ReviveTextTimer.text = System.Math.Round((float)(_ReviveCD - m_castTime), 2).ToString();

        }
    }
    public void StartReviveCoroutine()
    {
        Debug.Log("Here");
        m_reviveCoroutine = true;
    }
}