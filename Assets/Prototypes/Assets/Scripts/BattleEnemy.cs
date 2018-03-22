using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleEnemy : MonoBehaviour {

    public int[] _Attacktimers;
    public GameObject[] _Targets;

    private TargetRangeChecker m_range;
    private float cooldownSpellA;
    private int cooldownMax = 1;
    private bool isSpellACasting;

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

        NormalAttackGirl();
        NormalAttackBoy();
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
                    ChangeColor(hpData.curHealth);
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
                StartCoroutine(CastSpell());
            }
        }
    }

    void NormalAttackBoy()
    {
        if(IsDivisble((int) 2, 15))
        {
            
        }
    }

    void UltimateAttack(int currhealth, int maxhealth)
    {
        if ((float)currhealth/(float)maxhealth > 0.4 && (float) currhealth / (float)maxhealth < 0.55
            ||  (float) currhealth / (float)maxhealth > 0.10 && (float)currhealth / (float)maxhealth < 0.15)
        {
            
        }
                                                                                                
    }

    void ChangeColor(int currhealth)
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


            spellTargetPosition.transform.position += newTargetPosition * 10;

            storedPositions[i] = spellTargetPosition;

        }
    }

    private IEnumerator CastSpell()
    {
        isSpellACasting = true;

        this.GetComponent<Rigidbody>().isKinematic = true;
        StorePosition(0);

        transform.LookAt(storedPositions[0].transform);
        Vector3 direction = storedPositions[0].transform.position - transform.position;
        float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;

        GameObject warning = Instantiate(_WarningbeforeSpell, transform.position, transform.rotation);

        yield return new WaitForSeconds(2);

        GameObject go = Instantiate(spellPrefab[0], transform.position, transform.rotation);
        go.gameObject.GetComponent<Spell>().SetTarget(storedPositions[0]);

        cooldownSpellA = 0;

        Destroy(warning);
        storedPositions[0] = null;

        yield return new WaitForSeconds(0.5f);

        this.GetComponent<Rigidbody>().isKinematic = false;


        isSpellACasting = false;      
    }
}
