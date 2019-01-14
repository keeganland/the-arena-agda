using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class FourthEnemy : BasicEnemyBehaviour{

    public float _AttackCD; //cooldown of the attack
    public float _WarningtoAttackCD; //total time you want to warn the player ahead of the attack

    private bool m_isAttacking; //For the coroutine

    public Slider _CastSpellSlider; //UI related objects
    public GameObject _CastSpellGameobject;
    public Text _SpellCasttimer;

    private float m_warningCastTime; //float that is the time that counts up until the warning time is there.
    public bool m_warningCastTimeBool; //bool that starts the Slider timers
    public bool isEnemyMoving; //a bool linked to an event to freeze everyone (menu, scripted events)

    public bool StopAttacking; //bool that makes the enemy stop attacking (but can still move)


    new void Start()
    {
        base.Start();
        _BoyOrGirl = Random.Range(0, 2);
        //isEnemyMoving = true;
    }

    new void Update()
    {
        base.Update();

        if (!isCollided && isEnemyMoving == true) //conditions for movement
        {
            if (_Target[_BoyOrGirl] && !m_isAttacking)
            {
                m_nav.SetDestination(_Target[_BoyOrGirl].transform.position);
            }
        }

        if (m_warningCastTimeBool == true) //UI slider part (warning time for spell)
        {
            if (m_warningCastTime < _WarningtoAttackCD)
            {
                m_warningCastTime += Time.deltaTime;
            }
            _CastSpellSlider.value = m_warningCastTime / _WarningtoAttackCD;
            _SpellCasttimer.text = System.Math.Round((float)(_WarningtoAttackCD - m_warningCastTime), 2).ToString();

        }
        m_timer += Time.deltaTime;
        CancelAttack();
    }

    private void CancelAttack() //opening in case the attack needs to stop halfway through (shield, stun, etc...)
    {
        if (gameObject.GetComponent<HealthController>().CurrentHealth == 0)
        {
        }
    }

    private void CancelMovement() //
    {
        isCollided = true;
        m_nav.SetDestination(m_nav.transform.position);
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (_Target[_BoyOrGirl] && !StopAttacking)
        {
            if (other == _Target[_BoyOrGirl].GetComponent<Collider>())
            {
                //Debug.Log("Target in Range " + curTarget.name)
                if (!m_isAttacking && m_timer >= _AttackCD)
                {
                    //Debug.Log("here");
                    CancelMovement();
                    StartCoroutine("FourthEnemyFirstAttack");
                }
            }
            else return;
        }
    }

    public override void OnTriggerStay(Collider other)
    {
        //Debug.Log(other.name);
        if (_Target[_BoyOrGirl] && !StopAttacking)
        {
            if (other == _Target[_BoyOrGirl].GetComponent<Collider>())
            {
                CancelMovement();
                //Debug.Log("Target in Range " + curTarget.name)
                if (!m_isAttacking && m_timer >= _AttackCD)
                {
                    transform.LookAt(_Target[_BoyOrGirl]);
                    StartCoroutine("FourthEnemyFirstAttack");
                }
            }
            else return;
        }
    }

    public override void OnTriggerExit(Collider other)
    {
        if (_Target[_BoyOrGirl])
        {
            //Debug.Log("TriggerExit" + other.name);
            if (other == _Target[_BoyOrGirl].GetComponent<Collider>())
            {
                isCollided = false;
                //will have player chase target once target leaves attack range trigger
                //Debug.Log("Target out of range " + curTarget.name);
            }
        }
    }

    private IEnumerator FourthEnemyFirstAttack()
    {
        yield return null;
    }

    public override void ResetToDefaults()
    {
        throw new System.NotImplementedException();
    }

    public override void Stunned(GameObject StunAnim, float StunDuration)
    {
        throw new System.NotImplementedException();
    }
}
