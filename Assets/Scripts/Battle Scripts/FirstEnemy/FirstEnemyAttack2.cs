using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FirstEnemyAttack2 : MonoBehaviour {

    public Vector3 m_targetpos;
	private Rigidbody m_rbEnemy;

    [Header("Targeting and Attacks Data")]
    public Transform[] _Target;
    public int _BoyOrGirl;

    public float _AttackCD;
    public float _WarningtoAttackCD;

    private bool m_isAttacking;
    private float m_timer; 

	NavMeshAgent m_nav;
	int i;
	bool isCollided;
	private bool m_isDashAttack;

	public GameObject _WarningDash;

    private void Start()
    {
        m_rbEnemy = GetComponent<Rigidbody>();
        m_nav = GetComponent<NavMeshAgent>();
        _BoyOrGirl = Random.Range(0, 2);
    }

    // Update is called once per frame
    void Update ()
    {
		if (!isCollided)
		{
			if (i == 1 && _Target[i].GetComponent<HealthController>().currentHealth == 0 )
			{
				i = 0;
			}
			else if (i == 0 && _Target[i].GetComponent<HealthController>().currentHealth == 0)
			{
				i = 1;
			}
			if (_Target[i])
			{
				m_nav.SetDestination(_Target[_BoyOrGirl].transform.position);
			}

		}
		
		if (!m_isAttacking && m_timer >= _AttackCD)
        {
            Debug.Log("here");
            m_isAttacking = true;
            StartCoroutine("DashAttack");
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
				//Debug.Log("Target in Range " + curTarget.name);
				CancelDashMovement();
				//Pass attack function here?
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
	public float AttackSpeed;
	public GameObject _Sprite;
	private float AttackTimer;

    private IEnumerator DashAttack()
    {
		m_isDashAttack = true;

		yield return new WaitForSeconds(1);

		Vector3 direction = _Target[i].transform.position - transform.position;
		float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;

		transform.LookAt(_Target[i].transform);

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

		_WarningDash.SetActive(true);

		DamageData dmgData = new DamageData();
		dmgData.damage = Damage;

		MessageHandler msgHandler = _Target[_BoyOrGirl].GetComponent<MessageHandler>();

		if (msgHandler)
		{
			msgHandler.GiveMessage(MessageTypes.DAMAGED, this.gameObject, dmgData);
		}
		AttackTimer = 0.0f;

		m_isDashAttack = false;


        Debug.Log("Coroutine Started");

        //We select the position for the spell
        _BoyOrGirl = Random.Range(0, 2);
        m_targetpos = _Target[_BoyOrGirl].position;


        //Look toward target and draw "warning" line
        transform.LookAt(_Target[_BoyOrGirl]);
        _WarningDash.SetActive(true);


        yield return new WaitForSeconds(_WarningtoAttackCD); 

        //Start the Attack
		m_rbEnemy.AddForce(transform.forward * 750);
    }
}

