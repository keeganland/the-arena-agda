using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    public PublicVariableHolderneverUnload _PublicVariableHolder;

    //Config
    [SerializeField] bool isBoss; //Needed here and not in the public var holder

    [SerializeField] private int totalHealth = 100;
    [SerializeField] private int currentHealth;
    [SerializeField] private int aggroBoy = 0;
    [SerializeField] private int aggroGirl = 0;
    [SerializeField] private int reviveCooldown;

    private float invincibilityTimer;
    private float invincibilityHealTimer;
    private int tempHealth;
    public string gameObjectName;
    private float m_castTime;

    //State
    private bool reviveCoroutineisStarted;
    private bool isReviving;
    public bool reviveCoroutine;
    public bool isTargeted;
    private bool invincibility;
    private bool invincibilityHeal;

    //Cached component  references
    [SerializeField] private GameObject sprite;
    [SerializeField] private List<GameObject> enemy;
    [SerializeField] private AudioClip deathSound;

    private AudioSource audioSource;
    private GameObject castReviveGameObject;
    private Slider reviveSlider;
    private Text reviveTextTimer;
    private GameObject slider; 
    private GameObject deathAnim; 
    private MessageHandler messageHandler;

    public GameObject deathTutorial;
    //Messages then methods

    #region Getter and Setters
    public int TotalHealth
    {
        get { return totalHealth; }
        set { totalHealth = value; }
    }
    public GameObject Sprite
    {
        get { return sprite; }
    }
    public int CurrentHealth
    {
        get { return currentHealth; }
        set { currentHealth = value; }
    }
    public bool IsBoss
    {
        set { isBoss = value; }
    }
    #endregion

    private void Awake()
    {
        currentHealth = TotalHealth;
    }

    private void OnDisable()
    {
        EventManager.TriggerEvent("camTargetRefresh");
        EventManager.StopListening("resetPlayer", resetPlayers);
        EventManager.StopListening("refreshUI", refreshUI);
        for (int i = 0; i < enemy.Count; i ++)
        {
            if (enemy[i])
            {
                Debug.Log("here " + gameObject.name);
                Debug.Log(enemy[i].name);
                enemy[i].GetComponent<PlayerAI>().hasTarget = false;
            }
        }
    }

    private void OnEnable()
    {
        EventManager.StartListening("resetPlayer", resetPlayers);
        EventManager.StartListening("refreshUI", refreshUI);
    }

    private void resetPlayers()
    {
        StartCoroutine(StartResetPlayer());
    }

    private IEnumerator StartResetPlayer()
    {
        CurrentHealth = TotalHealth;
        UndoDeath();
        yield return new WaitForSeconds(1f);

        if (this.gameObject.name == "Girl")
        {
            _PublicVariableHolder.DeathCanvas.SetActive(false);
            _PublicVariableHolder.fader.StartCoroutine("FadeIn");
        }
    }

    private void Start()
    {
        if(gameObject.name == "Boy")
        {
            castReviveGameObject = _PublicVariableHolder._BoyCastReviveGameobject;
            reviveSlider = _PublicVariableHolder._BoyReviveSlider;
            reviveTextTimer = _PublicVariableHolder._BoyReviveTextTimer;
            sprite = _PublicVariableHolder._BoySpriteGameObject;
            slider = _PublicVariableHolder._BoySlider;
            deathAnim = _PublicVariableHolder._BoyDeathAnim;
        }
        if (gameObject.name == "Girl")
        {
            castReviveGameObject = _PublicVariableHolder._GirlCastReviveGameobject;
            reviveSlider = _PublicVariableHolder._GirlReviveSlider;
            reviveTextTimer = _PublicVariableHolder._GirlReviveTextTimer;
            sprite = _PublicVariableHolder._GirlSpriteGameObject;
            slider = _PublicVariableHolder._GirlSlider;
            deathAnim = _PublicVariableHolder._GirlDeathAnim;
        }

        messageHandler = GetComponent<MessageHandler>();

        if (messageHandler)
        {
            messageHandler.RegisterDelegate(RecieveMessage);
        }

        audioSource = GetComponent<AudioSource>();

    }

    private void Update()
    {
        if (gameObject.tag == "Player")
        {
            if (invincibilityTimer >= 1f)
            {
                invincibility = false;
            }
            if (invincibilityTimer <= 10f)
            {
                invincibilityTimer += Time.deltaTime;
            }
            if (invincibilityHealTimer <= 10f)
            {
                invincibilityHealTimer += Time.deltaTime; 
            }
            if(invincibilityHealTimer >= 1f)
            {
                invincibilityHeal = false;
            }
        }
    }

    void RecieveMessage(MessageTypes msgType, GameObject go, MessageData msgData)
    {
        switch (msgType)
        {
            case MessageTypes.DAMAGED:
                DamageData dmgData = msgData as DamageData;

                if (gameObject.tag == "Player")
                {
                    if (dmgData != null && !invincibility)
                    {
                        invincibilityTimer = 0;
                        invincibility = true;
                        ApplyDamage(dmgData.damage, go);
                    }
                }
                if (gameObject.tag == "Enemy" || gameObject.tag == "Objects")
                {
                    if (dmgData != null && !invincibility)
                    {                       
                        ApplyDamage(dmgData.damage, go);
                    }
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
                    aggroBoy += aggroData.aggro; //Need to establish Aggro for each character on all enemies
                }
                else if(go.name == "Girl")
                {
                    //Debug.Log("here");
                    aggroGirl += aggroData.aggro;
                }
                 break;
            case MessageTypes.HEALED: //for healing character
                RecoverData recoverData = msgData as RecoverData;
                if(recoverData != null && !invincibilityHeal)
                {
                    invincibilityHealTimer = 0;
                    invincibilityHeal = true;
                    //Debug.Log("HealthController: case HEALED");
                    RecoverHealth(recoverData.HP_up, go);
                }
                break;
        }
    }

    public void ApplyDamage(int damage, GameObject go)
    {
        CurrentHealth -= damage;
        

        //Will need to change this if statement, pretty sure I need to remove the spawn part
        if (CurrentHealth <= 0f /*&& GameObject.Find("Cube-Spawn").GetComponent<HealthController>().currentHealth > 0*/)
        {
           // PlaySoundOnKill();
            CurrentHealth = 0;

            if (messageHandler)
            {
                DeathData deathData = new DeathData();
                deathData.attacker = go;
                deathData.attacked = gameObject;

                messageHandler.GiveMessage(MessageTypes.DIED, gameObject, deathData);
            }

            if(transform.parent != null)
            {
                if (gameObject.GetComponentInParent<TotemDestroyed>())
                {
                    gameObject.GetComponentInParent<TotemDestroyed>()._IsDestroyed = true;
                }
            }

            if (this.gameObject.tag == "Player")
            {
                DoDeath();
            }

            if (this.gameObject.tag == "Enemy")
            {
                GameObject.Find("/PlayerUI").GetComponent<UISpellSwap>().currentEnemy = null;
                Destroy(transform.parent.gameObject, 0.15f);

                if (isBoss)
                {
                    EventManager.TriggerEvent("bossDied");
                }
                EventManager.TriggerEvent("checkVictory");
            }
        }

        else if(CurrentHealth >= 0f && CurrentHealth <= totalHealth && this.GetComponent<HealthUI>())
        {
            this.GetComponent<HealthUI>().UpdateUi(totalHealth, currentHealth);
        }

        if (messageHandler)
        {
            HealthData hpData = new HealthData();
            hpData.maxHealth = totalHealth;
            hpData.curHealth = currentHealth;

            messageHandler.GiveMessage(MessageTypes.HEALTHCHANGED, gameObject, hpData);
        }
    }

    private void DoDeath() {

        if (deathTutorial)
        {
            deathTutorial.SetActive(true);
        }

        gameObject.GetComponent<CapsuleCollider>().enabled = true;
        if (slider)
            slider.SetActive(false);
        if (deathAnim)
            deathAnim.SetActive(true);

        if (sprite)
        {
            sprite.GetComponent<Animator>().Play("Death");
            sprite.GetComponent<Animator>().SetBool("Death", true);
        }

        if (this.gameObject.name == "Boy")
        {
            EventManager.TriggerEvent("StopBoyMoving");
            GameObject.Find("Girl").GetComponent<BetterPlayer_Movement>().SwapGirl();
            gameObject.GetComponent<BetterPlayer_Movement>().SwapGirl();
            _PublicVariableHolder._DeathBoyParticle.Play();
            gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
            gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
          //  _PublicVariableHolder._DeathBoyParticle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
        if (this.gameObject.name == "Girl")
        {
            EventManager.TriggerEvent("StopGirlMoving");
            GameObject.Find("Boy").GetComponent<BetterPlayer_Movement>().SwapBoy();
            gameObject.GetComponent<BetterPlayer_Movement>().SwapBoy();
            _PublicVariableHolder._DeathGirlParticle.Play();
            gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
            gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
            //  _PublicVariableHolder._DeathGirlParticle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
        this.gameObject.GetComponent<BetterPlayer_Movement>().enabled = false;//This works
    }

    public void UndoDeath()
    {
        sprite.GetComponent<SpriteRenderer>().enabled = true;
        sprite.GetComponent<Animator>().SetBool("Death", false);

        if (gameObject.GetComponent<CapsuleCollider>())
            gameObject.GetComponent<CapsuleCollider>().enabled = false;
        if (slider)
            slider.SetActive(true);
        if (deathAnim)
            deathAnim.SetActive(false);

        if(this.gameObject.GetComponent<BetterPlayer_Movement>())
            this.gameObject.GetComponent<BetterPlayer_Movement>().enabled = true;//This works

        if(this.gameObject.name == "Boy")
        {
            EventManager.TriggerEvent("StartBoyMoving");
            _PublicVariableHolder._ReviveBoyParticle.Play();
           // _PublicVariableHolder._ReviveBoyParticle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
        if (this.gameObject.name == "Girl")
        {
            EventManager.TriggerEvent("StartGirlMoving");
            _PublicVariableHolder._ReviveGirlParticle.Play();
          //  _PublicVariableHolder._ReviveGirlParticle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }

        if (messageHandler)
        {
            HealthData hpData = new HealthData();

            hpData.maxHealth = totalHealth;
            hpData.curHealth = currentHealth;

            messageHandler.GiveMessage(MessageTypes.HEALTHCHANGED, gameObject, hpData);
        }
    }

    private void PlaySoundOnKill()
    {
        if (deathSound)
        {
            audioSource.PlayOneShot(deathSound);
        }
    }

    public void RecoverHealth(int HP_up, GameObject go) //Applies healing to character
    {
        tempHealth = HP_up + currentHealth;
            if (tempHealth > totalHealth) //checks to make sure character doesn't go over max health
            {
                currentHealth = totalHealth;
            }
            else if (tempHealth <= totalHealth) //applies healing if health is still below max health
            {
                currentHealth = tempHealth;
            }
            if (currentHealth >= 0f && currentHealth <= totalHealth)
            {
                this.GetComponent<HealthUI>().UpdateUi(totalHealth, currentHealth);
            }
            if (messageHandler)
            {
                HealthData hpData = new HealthData();
                hpData.maxHealth = totalHealth;
                hpData.curHealth = currentHealth;

                messageHandler.GiveMessage(MessageTypes.HEALTHCHANGED, gameObject, hpData);
            }
    }

    public IEnumerator ReviveByClicking()
    {
        reviveCoroutineisStarted = true;
        m_castTime = 0;
        castReviveGameObject.SetActive(true);
        reviveTextTimer.enabled = true;
        isReviving = true;
        yield return new WaitForSeconds(reviveCooldown);

        if(this.gameObject.name == "Boy")
        {
            GameObject.Find("Girl").GetComponent<HealthController>().CurrentHealth = GameObject.Find("Girl").GetComponent<HealthController>().TotalHealth;
            GameObject.Find("Girl").GetComponent<HealthController>().UndoDeath();
            GameObject.Find("Girl").GetComponent<BetterPlayer_Movement>().UndoCurTarget();
        }
        if (this.gameObject.name == "Girl")
        {
            GameObject.Find("Boy").GetComponent<HealthController>().CurrentHealth = GameObject.Find("Boy").GetComponent<HealthController>().TotalHealth;
            GameObject.Find("Boy").GetComponent<HealthController>().UndoDeath();
            GameObject.Find("Boy").GetComponent<BetterPlayer_Movement>().UndoCurTarget();

        }
        isReviving = false;
        castReviveGameObject.SetActive(false);
        reviveTextTimer.enabled = false;
        reviveCoroutineisStarted = false;
        reviveCoroutine = false;
    }

    private void FixedUpdate()
    {
        if(!reviveCoroutineisStarted && reviveCoroutine)
        {
            StartCoroutine("ReviveByClicking");
        }

        if (isReviving == true)
        {
            if (m_castTime < reviveCooldown)
            {
                m_castTime += Time.fixedDeltaTime;
            }
            reviveSlider.value = m_castTime / reviveCooldown;
            reviveTextTimer.text = System.Math.Round((float)(reviveCooldown - m_castTime), 2).ToString();

        }
    }

    public void StopReviveCoroutine()
    {
        StopCoroutine("ReviveByClicking");

        isReviving = false;
        castReviveGameObject.SetActive(false);
        reviveTextTimer.enabled = false;
        reviveCoroutineisStarted = false;
        reviveCoroutine = false;
    }

    public void SetEnemy(GameObject Attackeur)
    {
        if(!enemy.Contains(Attackeur))
        {
            enemy.Add(Attackeur);
        }
    }

    public void CancelEnemy(GameObject Attackeur)
    {
        if (enemy.Contains(Attackeur))
        {
            enemy.Remove(Attackeur);
        }
    }

    private void refreshUI()
    {
        GetComponent<HealthUI>().UpdateUi(totalHealth, currentHealth);
    }
}
