using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public abstract class BasicEnemyBehaviour : MonoBehaviour 
{

    protected bool isCollided;
    protected Transform[] _Target = new Transform[2];
    protected Rigidbody m_rbEnemy;
    protected NavMeshAgent m_nav;
    protected float m_timer; 

    public GameObject attackWarning;
    public int _BoyOrGirl;

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
        ChooseTarget();

        m_timer += Time.deltaTime;
	}

    void ChooseTarget () 
    {
        
        if (!isCollided)
        {
            if (_BoyOrGirl == 1 && _Target[_BoyOrGirl].GetComponent<HealthController>().currentHealth == 0)
            {
                _BoyOrGirl = 0;
            }
            else if (_BoyOrGirl == 0 && _Target[_BoyOrGirl].GetComponent<HealthController>().currentHealth == 0)
            {
                _BoyOrGirl = 1;
            }
        }
    }

    abstract public void OnTriggerStay(Collider other);

    abstract public void OnTriggerExit(Collider other);
}
