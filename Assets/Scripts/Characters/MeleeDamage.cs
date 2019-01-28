using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//Alex : Refactor 01/13
public class MeleeDamage : MonoBehaviour {

    //Config
    [SerializeField] private int damage = 10;
    [SerializeField] private int aggro = 0;
    [SerializeField] private float attackSpeed = 3f;
    private float attackTimer;
    [SerializeField] Color color;

    //Cached component  references
    GameObject sprite;
    private List<GameObject> spellPrefab = new List<GameObject>();
    private Slider autoAttackslider;
    [SerializeField] GameObject target;

    #region Getters and Setters
    public int Damage
    {
        get { return damage; }
        set { damage = value; }
    }
    public float AttackSpeed
    {
        get { return attackSpeed; }
        set { attackSpeed = value; }
    }
    #endregion

    private void Awake()
    {
        //Might need more drastic revision
        if (gameObject.name == "Boy")
        {
            sprite = GameObject.Find("Evgeny");
            spellPrefab.Add(Resources.Load("Prefabs/Player/BoyMeleeDamage") as GameObject);

            Slider[] sliderChild = sprite.GetComponentsInChildren<Slider>();
            foreach (Slider sl in sliderChild)
                if (sl.name == "BoyAASlider")
                    autoAttackslider = sl;
        }
        if (gameObject.name == "Girl")
        {
            sprite = GameObject.Find("Eva");
            spellPrefab.Add(Resources.Load("Prefabs/Player/GirlMeleeDamage") as GameObject);
            Slider[] sliderChild = sprite.GetComponentsInChildren<Slider>();
            foreach (Slider sl in sliderChild)
                if (sl.name == "GirlAASlider")
                    autoAttackslider = sl;
        }


    }

    void Update()
    {
        //Debug.Log("MeleeDamage: Damage Target" + m_target.name);
        if (autoAttackslider)
        {
            autoAttackslider.value = attackTimer / attackSpeed;
        }
        attackTimer += Time.deltaTime;
        if (attackTimer >= attackSpeed && (this.GetComponentInChildren<RangeChecker>().InRange(target)) && target != null
            && gameObject.GetComponent<HealthController>().CurrentHealth > 0)
        {
            if (target.CompareTag("Objects") && target.GetComponent<HealthController>() == false)
            {
                return;
            }

            if (target.GetComponent<HealthController>().CurrentHealth <= 0)
            {
                return;
            }

            CastSpell();
            DisplayDamage();
            SendMessageToEnemyHealthController();

            attackTimer = 0.0f;
        }
    }

    private void SendMessageToEnemyHealthController()
    {
        DamageData dmgData = new DamageData();
        dmgData.damage = damage;

        AggroData aggroData = new AggroData();
        aggroData.aggro = aggro;

        //Really need to make sure MessageHandler is on all enemies and players
        MessageHandler msgHandler = target.GetComponent<MessageHandler>();

        if (msgHandler)
        {
            if (target.GetComponent<BattleEnemy>())
            {
                if (target.GetComponent<BattleEnemy>().isInvincible == true)
                {
                    //m_audioSource.PlayOneShot(_InvinsibleSFX);
                    return;
                }
            }

            msgHandler.GiveMessage(MessageTypes.DAMAGED, this.gameObject, dmgData);
            msgHandler.GiveMessage(MessageTypes.AGGROCHANGED, this.gameObject, aggroData);
            //Debug.Log("MeleeDamage: this = " + this.name);
        }
    }

    private void DisplayDamage()
    {
        if (color != new Color(0, 0, 0, 0))
        {
            if (gameObject.name == "Boy")
            {
                GameObject sprite = target.GetComponent<HealthController>().Sprite;
                Canvas[] canvas = sprite.GetComponentsInChildren<Canvas>();
                if (canvas.Length != 0)
                {
                    for (int i = 0; i < canvas.Length; i++)
                    {
                        if (canvas[i].GetComponentInChildren<DamageDisplayScript>())
                        canvas[i].GetComponentInChildren<DamageDisplayScript>().GetDamageText(color, damage);
                    }
                }
            }
        }
    }

    public void TargetChanges(GameObject target)
    {
        this.target = target;
    }

    public void CastSpell()
    {
        Vector3 direction = target.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
        transform.LookAt(target.transform);

        int rotation = 0;

        if (45 <= transform.eulerAngles.y && transform.eulerAngles.y < 135)
        {
            rotation = 3;
        }
        else if (135 <= transform.eulerAngles.y && transform.eulerAngles.y < 225)
        {
            rotation = 2;
        }
        else if (225 <= transform.eulerAngles.y && transform.eulerAngles.y < 315)
        {
            rotation = 4;
        }
        else
        {
            rotation = 1;
        }

        if (rotation != 0)
        {
            sprite.GetComponent<SpriteScript2>().ForcePlayerRotation(rotation);
        }

        GameObject go = Instantiate(spellPrefab[0], transform.position, Quaternion.Euler(0, -angle, 0));

        go.gameObject.GetComponent<Spell>().SetTarget(target);
        //go.gameObject.GetComponent<Bullet>().GetAggro(Aggro);
        go.gameObject.GetComponent<Bullet>().SpellFlare(angle);
        go.gameObject.GetComponent<Bullet>().SetSpellCaster(this.gameObject);
    }
}

