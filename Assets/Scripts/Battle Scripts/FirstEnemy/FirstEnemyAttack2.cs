﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class FirstEnemyAttack2 : BasicEnemyBehaviour {

    public float _AttackCD;
    public float _WarningtoAttackCD;

	private bool m_isDashAttack;

	public GameObject _WarningDash;
    public GameObject _DashSpell;
    public GameObject _TeleportPosition;

    public Slider _CastSpellSlider;
    public GameObject _CastSpellGameobject;
    public Text _SpellCasttimer;

    private float m_warningCastTime;
    public bool m_warningCastTimeBool;
    public bool isEnemyMoving;
    private Vector3 m_targetPos;
    private bool m_dashingAnim;

    public float _DashSpeed = 4;
    public int Aggro;

    public GameObject _DashFX;

	private void Start()
	{
        isEnemyMoving = true;
        //_Target[0] = GameObject.Find("Boy").GetComponent<Transform>();
        //_Target[1] = GameObject.Find("Girl").GetComponent<Transform>();
	}

	// Update is called once per frame
	void Update ()
    {
        if (!isCollided && isEnemyMoving == true)
        {
          if (_Target[_BoyOrGirl] && !m_isDashAttack)
          {
              m_nav.SetDestination(_Target[_BoyOrGirl].transform.position);
          }
        }

        if (m_warningCastTimeBool == true)
        {
            if (m_warningCastTime < _WarningtoAttackCD) {
                m_warningCastTime += Time.deltaTime;
            }
            _CastSpellSlider.value = m_warningCastTime / _WarningtoAttackCD;
            _SpellCasttimer.text = System.Math.Round((float)(_WarningtoAttackCD - m_warningCastTime), 2).ToString();
   
        }
        m_timer += Time.deltaTime;
	}

    private void FixedUpdate()
    {
        if (m_dashingAnim)
        {
            transform.position = Vector3.Lerp(transform.position, m_targetPos, _DashSpeed * Time.fixedDeltaTime);
        }
    }

    private void CancelDashMovement()
	{
		isCollided = true;
		m_nav.SetDestination(m_nav.transform.position);
	}

	public override void OnTriggerEnter(Collider other)
	{
		if (_Target[_BoyOrGirl])
		{
			if (other == _Target[_BoyOrGirl].GetComponent<Collider>())
			{
				//Debug.Log("Target in Range " + curTarget.name)
                if (!m_isDashAttack && m_timer >= _AttackCD)
                {
                    //Debug.Log("here");
                    CancelDashMovement();
                    StartCoroutine("DashAttack");
                }
            }
			else return;
		}
	}

    public override void OnTriggerStay(Collider other)
    {
        //Debug.Log(other.name);
        if (_Target[_BoyOrGirl])
        {
            if (other == _Target[_BoyOrGirl].GetComponent<Collider>())
            {
                CancelDashMovement();
                //Debug.Log("Target in Range " + curTarget.name)
                if (!m_isDashAttack && m_timer >= _AttackCD)
                {                 
                    transform.LookAt(_Target[_BoyOrGirl]);                  
                    StartCoroutine("DashAttack");
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

    private IEnumerator DashAttack()
    {
		m_isDashAttack = true;

		

        //Look toward target and draw "warning" line
        _WarningDash.SetActive(true);

        //Slider
        m_warningCastTime = 0;
        _CastSpellGameobject.SetActive(true);
        _SpellCasttimer.enabled = true;
        //   _SpellCasttimer.text = string.Format("0.00", );
        //_SpellCasttimer.text = (_WarningtoAttackCD - m_warningCastTime).ToString("F2");
        _SpellCasttimer.text =  System.Math.Round((float)(_WarningtoAttackCD), 2).ToString();
        m_warningCastTimeBool = true;

        yield return new WaitForSeconds(_WarningtoAttackCD);

        m_warningCastTimeBool = false;
        _SpellCasttimer.enabled = false;
        _CastSpellGameobject.SetActive(false);

        _WarningDash.SetActive(false);
        //Start the Attack
        Quaternion m_dashRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y - 180, transform.rotation.eulerAngles.z);
        GameObject go = Instantiate(_DashSpell, this.transform.position,Quaternion.identity);
        GameObject fx = Instantiate(_DashFX, this.transform.position, m_dashRotation);
        m_targetPos = _TeleportPosition.transform.position;
        go.GetComponent<DashCollider>().SetTarget(m_targetPos);
        go.GetComponent<Bullet>().GetSpellCaster(this.gameObject);

       
        m_dashingAnim = true;
   
        yield return new WaitForSeconds(_WarningtoAttackCD);
       
        //Reset Enemy
        Destroy(go);
        m_dashingAnim = false;

        m_nav.SetDestination(transform.position);

        yield return new WaitForSeconds(_WarningtoAttackCD);

        Destroy(fx);        

        _BoyOrGirl = Random.Range(0, 2);

        m_timer = 0;
        isCollided = false;
        m_isDashAttack = false;
    }
}

