using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FirstEnemyAttack2 : MonoBehaviour {

	private Rigidbody m_rbEnemy;

    [Header("Targeting and Attacks Data")]
    private Transform[] _Target = new Transform[2];
    public int _BoyOrGirl;

    public float _AttackCD;
    public float _WarningtoAttackCD;

    private float m_timer; 

	NavMeshAgent m_nav;
	bool isCollided;
	private bool m_isDashAttack;

	public GameObject _WarningDash;
    public GameObject _DashSpell;
    public GameObject _TeleportPosition;

    private void Start()
    {
        _Target[0] = GameObject.Find("Boy").GetComponent<Transform>();
        _Target[1] = GameObject.Find("Girl").GetComponent<Transform>();

        m_rbEnemy = GetComponent<Rigidbody>();
        m_nav = GetComponent<NavMeshAgent>();
        _BoyOrGirl = Random.Range(0, 2);
        m_timer = 15;
    }

    // Update is called once per frame
    void Update ()
    {
		if (!isCollided)
		{
			if (_BoyOrGirl == 1 && _Target[_BoyOrGirl].GetComponent<HealthController>().currentHealth == 0 )
			{
                _BoyOrGirl = 0;
			}
			else if (_BoyOrGirl == 0 && _Target[_BoyOrGirl].GetComponent<HealthController>().currentHealth == 0)
			{
                _BoyOrGirl = 1;
			}
			if (_Target[_BoyOrGirl] && !m_isDashAttack)
			{
				m_nav.SetDestination(_Target[_BoyOrGirl].transform.position);
			}

		}
        m_timer += Time.deltaTime;
	}

	private void CancelDashMovement()
	{
		isCollided = true;
		m_nav.SetDestination(m_nav.transform.position);
	}

	private void OnTriggerEnter(Collider other)
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

    private void OnTriggerStay(Collider other)
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

    private void OnTriggerExit(Collider other)
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

	public int Damage;
	public int Aggro;
	public GameObject _Sprite;

    private IEnumerator DashAttack()
    {
		m_isDashAttack = true;

		Vector3 direction = _Target[_BoyOrGirl].transform.position - transform.position;
		float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;

		transform.LookAt(_Target[_BoyOrGirl].transform);

		int rotation = 0;

		if (45 <= transform.eulerAngles.y && transform.eulerAngles.y < 135)
		{
			rotation = 3;
		}
		else if (0 <= transform.eulerAngles.y && transform.eulerAngles.y < 45)
		{
			rotation = 1;
		}
		else if (225 <= transform.eulerAngles.y && transform.eulerAngles.y < 315)
		{
			rotation = 4;
		}
		else
		{
			rotation = 2;
		}

		if (rotation != 0)
		{
            if(_Sprite)
			_Sprite.GetComponent<SpriteScript2>().ForcePlayerRotation(rotation);
		}

        //Look toward target and draw "warning" line
        _WarningDash.SetActive(true);

        yield return new WaitForSeconds(_WarningtoAttackCD);

        _WarningDash.SetActive(false);
        //Start the Attack
        GameObject go = Instantiate(_DashSpell, this.transform.position,Quaternion.identity);
        go.GetComponent<DashCollider>().SetTarget(_TeleportPosition);
        go.GetComponent<Bullet>().GetSpellCaster(this.gameObject);

        yield return new WaitForSeconds(_WarningtoAttackCD);
        //Reset Enemy

        transform.position = _TeleportPosition.transform.position;
        m_nav.SetDestination(transform.position);

        yield return new WaitForSeconds(_WarningtoAttackCD);

        _BoyOrGirl = Random.Range(0, 2);

        m_timer = 0;
        isCollided = false;
        m_isDashAttack = false;
    }
}

