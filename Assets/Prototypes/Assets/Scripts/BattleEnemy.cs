using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleEnemy : MonoBehaviour {

    public GameObject[] _Targets;

    private TargetRangeChecker m_range;

    [Header("Different Spells")]
    private float cooldownSpellA;
    private float cooldownSpellB;
    private float cooldownSpellC;
    private bool isSpellACasting;
    private bool isSpellBCasting;
    private bool isSpellCCasting;


    private int cooldownMax = 1;

    [Header("Spell Data")]
    public int[] _Attacktimers;
    public GameObject spellTargetPosition;
    public GameObject _WarningbeforeSpell;

    [SerializeField]
    private GameObject[] spellPrefab;
    public GameObject[] storedPositions;

    public void Start()
    {
        m_range = GetComponent<TargetRangeChecker>();
        MessageHandler msgHandler = GetComponent<MessageHandler>();

        if (msgHandler)
        {
            msgHandler.RegisterDelegate(RecieveMessage);
        }

       for(int i = 0; i < _Attacktimers.Length; i++)
        {
            cooldownMax *= _Attacktimers[i];
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
        ChangeColor();
    }

    void RecieveMessage(MessageTypes msgType, GameObject go, MessageData msgData)
    {
        switch (msgType)
        {
            case MessageTypes.HEALTHCHANGED:
                HealthData hpData = msgData as HealthData;

                if (hpData != null)
                {
                    UltimateAttack(hpData.curHealth, hpData.maxHealth);
                    ChangeColor();
                }
                break;
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
         if( cooldownSpellB >= _Attacktimers[1])
        {
           // Debug.Log((int) cooldownSpellB);
            if (!isSpellBCasting)
            {
                StartCoroutine(CastSpellB());
            }
        }
    }

    void UltimateAttack(int currhealth, int maxhealth)
    {
        if ((float)currhealth/(float)maxhealth > 0.4 && (float) currhealth / (float)maxhealth < 0.55
            ||  (float) currhealth / (float)maxhealth > 0.10 && (float)currhealth / (float)maxhealth < 0.15)
        {
            
        }
                                                                                                
    }

    void ChangeColor()
    {
        
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

            Debug.Log(_Targets[i]);

            spellTargetPosition.transform.position += newTargetPosition * 10;

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
        Debug.Log(pos + "is pos");
        StorePosition(pos); // FIX : WHEN GIRL DIE, BOSS DOESNT ATTACK ANYMORE
                     
        transform.LookAt(storedPositions[pos].transform);
        Vector3 direction = storedPositions[pos].transform.position - transform.position;
        float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;

        GameObject warning = Instantiate(_WarningbeforeSpell, transform.position, transform.rotation);

        yield return new WaitForSeconds(2);

        GameObject go = Instantiate(spellPrefab[0], transform.position, transform.rotation);
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

        Instantiate(spellPrefab[1],transform.position, Quaternion.identity);

        yield return new WaitForSeconds(4);

        cooldownSpellB = 0;

        isSpellBCasting = false;
    }

    private IEnumerator CastSpellC()
    {
        isSpellCCasting = true;

        yield return new WaitForSeconds(4);

        cooldownSpellC = 0;

        isSpellCCasting = false;
    }
}

