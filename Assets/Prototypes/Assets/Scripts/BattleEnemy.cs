using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleEnemy : MonoBehaviour {

    public GameObject[] _Targets;

    [Header("BossData")]
    private TargetRangeChecker m_range;
    private int m_currenthp;
    private int m_maxhp;

    [Header("Different Spells")]
    private float cooldownSpellA;
    private float cooldownSpellB;
    private float cooldownSpellC;

    private bool isSpellACasting;
    private bool isSpellBCasting;
    private bool isSpellCCasting;
    private bool isSpellDCasting;

    public bool isInvincible;


    [Header("Spell Data")]
    public int[] _Attacktimers;
    public GameObject spellTargetPosition;
    public GameObject _WarningbeforeSpell;
    public GameObject _ShowTotemPos;
    private GameObject m_shield;
    private GameObject m_totem = null;
    private GameObject m_showtotempos = null;

    [SerializeField]
    private GameObject[] spellPrefab;
    public GameObject[] storedPositions;
    public GameObject _Mouth;
    public GameObject _Body;
    public GameObject _Totem;
    public GameObject[] _ChevreSpawn;

    public void Start()
    {
        m_range = GetComponent<TargetRangeChecker>();
        MessageHandler msgHandler = GetComponent<MessageHandler>();

        if (msgHandler)
        {
            msgHandler.RegisterDelegate(RecieveMessage);
        }

        spellTargetPosition = Instantiate(spellTargetPosition as GameObject);

    }

    private void Update()
    {
        cooldownSpellA += Time.deltaTime;
        cooldownSpellB += Time.deltaTime;
        cooldownSpellC += Time.deltaTime;

        NormalAttackGirl();
        NormalAttackBoy();
        Shield();
        SpawnChevre();
        IsnotInvincible();
    }

    public void IsnotInvincible()
    {
        if (m_totem)
        {
            if (m_totem.GetComponent<TotemDestroyed>()._IsDestroyed == true)
            {
                isInvincible = false;
                isSpellCCasting = false;
                cooldownSpellC = 0;
                Destroy(m_shield, 1f);
                Destroy(m_totem, 1f);
                Destroy(m_showtotempos, 1f);
            }
        }
    }

    void RecieveMessage(MessageTypes msgType, GameObject go, MessageData msgData)
    {
        switch (msgType)
        {
            case MessageTypes.HEALTHCHANGED:
                HealthData hpData = msgData as HealthData;

                if (hpData != null)
                {                
                    m_currenthp = hpData.curHealth;
                    m_maxhp = hpData.maxHealth;
                    //Debug.Log((float) m_currenthp / (float)m_maxhp);
                }
                break;
        }   
    }

    void Shield()
    {
        if(cooldownSpellC >= _Attacktimers[2] && (float) m_currenthp / (float) m_maxhp <= 0.5f)
        {
            if (!isSpellCCasting)
            {
                StartCoroutine(CastSpellC());
            }
        }
    }
    
    void SpawnChevre()
    {
        if((float)m_currenthp / (float)m_maxhp <= 0.5f)
        {
            if (!isSpellDCasting)
            {
                StartCoroutine(CastSpellD());
            }
        }

    }

    void NormalAttackGirl()
    {
        if( cooldownSpellA >= _Attacktimers[0])
        {
            if (!isSpellACasting)
            {
                StartCoroutine(CastSpellA());
            }
        }
    }

    void NormalAttackBoy()
    {
         if( cooldownSpellB >= _Attacktimers[1] && !isInvincible)
        {
           // Debug.Log((int) cooldownSpellB);
            if (!isSpellBCasting)
            {
                StartCoroutine(CastSpellB());
            }
        }
    }

    public bool IsDivisble(int x, int n)
    {
        return (x % n) == 0;
    }

    private void StorePosition(int i)
    {
        if (storedPositions[i] == null)
        {
            spellTargetPosition.transform.position = _Targets[i].transform.position;

            Vector3 newTargetPosition = new Vector3(0,0,0);

            newTargetPosition.x = _Targets[i].transform.position.x - transform.position.x;
            newTargetPosition.z = _Targets[i].transform.position.z - transform.position.z;

            //Debug.Log(_Targets[i]);

            spellTargetPosition.transform.position += newTargetPosition * 6f;

            storedPositions[i] = spellTargetPosition;

        }
    }

    private IEnumerator CastSpellA()
    {
        isSpellACasting = true;

        this.GetComponent<Rigidbody>().isKinematic = true;

        int pos = Random.Range(0, 2);
        if(!_Targets[pos])
        {
            if (pos == 1)
                pos = 0;
            else pos = 1;
        }
        //Debug.Log(pos + "is pos");
        StorePosition(pos); // FIX : WHEN GIRL DIE, BOSS DOESNT ATTACK ANYMORE
                     
        transform.LookAt(storedPositions[pos].transform);
        Vector3 direction = storedPositions[pos].transform.position - transform.position;
        float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;

        GameObject warning = Instantiate(_WarningbeforeSpell, _Mouth.transform.position, transform.rotation);

        yield return new WaitForSeconds(2);

        GameObject go = Instantiate(spellPrefab[0], _Mouth.transform.position, transform.rotation);
        go.gameObject.GetComponent<Spell>().SetTarget(storedPositions[pos]);

        cooldownSpellA = 0;

        Destroy(warning);
        storedPositions[pos] = null;

        yield return new WaitForSeconds(0.5f);

        this.GetComponent<Rigidbody>().isKinematic = false;


        isSpellACasting = false;      
    }

    private IEnumerator CastSpellB()
    {
        isSpellBCasting = true;

        Instantiate(spellPrefab[1],_Body.transform.position, Quaternion.identity);

        yield return new WaitForSeconds(4);

        cooldownSpellB = 0;

        isSpellBCasting = false;
    }

    private IEnumerator CastSpellC()
    {
        isSpellCCasting = true;
        isInvincible = true;

        transform.LookAt(_Totem.transform);
        GameObject showtotempos = Instantiate(_ShowTotemPos, _Body.transform.position, transform.rotation);
        GameObject shield = Instantiate(spellPrefab[2], _Body.transform.position, Quaternion.identity);
        GameObject totem = Instantiate(spellPrefab[3], _Totem.transform.position, Quaternion.identity);
        m_shield = shield;
        m_totem = totem;
        m_showtotempos = showtotempos;

        yield return new WaitForSeconds(4);
    }

    private IEnumerator CastSpellD()
    {
        isSpellDCasting = true;

        int i = Random.Range(0, _ChevreSpawn.Length);

        Instantiate(spellPrefab[4], _ChevreSpawn[i].transform.position, Quaternion.identity);

        yield return new WaitForSeconds(_Attacktimers[3]);

        isSpellDCasting = false;
    }
}

