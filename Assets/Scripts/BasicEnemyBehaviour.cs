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

    public GameObject attackWarning; // Alex : I don't know about this one. One enemy may have more than one "attack Warning".
    public int _BoyOrGirl;
    public GameObject _Sprite;
    public int Damage;

    /*
     * Keegan:
     * Whenever any enemy whatsoever comes into being, whether through being present in a loaded scene by default,
     * Or being spawned by a spell, etc.
     * 
     * Then there are new potential targets for the camera.
     * Whenever any enemy whatsoever ceases to be, there are fewer
     * Therefore, we need to tell the camera to refresh itself
     * 
     * NOTE: possibly not the best way to do this.
     * It COULD be a good idea to set up the camera itself as a static singleton
     */
    protected void OnEnable()
    {
        //EventManager.StartListening("cleanup", CleanUp);
        EventManager.TriggerEvent("camTargetRefresh");
    }

    protected void OnDisable()
    {
        //EventManager.StopListening("cleanup", CleanUp);
        EventManager.TriggerEvent("camTargetRefresh");
    }

    protected void Start() 
	{

        /* Keegan NTS 2018/7/2:
         * The below was originally just part of the Start method
         * //initialize player objects in all enemies
         * I've moved it to the helper method InitPlayerInformation so we can call the same information when resetting an enemy
         */
        /*
        _Target[0] = GameObject.Find("/Characters/Boy").GetComponent<Transform>();
        _Target[1] = GameObject.Find("/Characters/Girl").GetComponent<Transform>();

        m_rbEnemy = GetComponent<Rigidbody>();
        m_nav = GetComponent<NavMeshAgent>();
        _BoyOrGirl = Random.Range(0, 2);
        m_timer = 5;
        */

        InitPlayerInformation();
    }

    /*
     * Helper method - used in both Start and functions for resetting enemies to their default behaviour 
     */
    private void InitPlayerInformation()
    {
        _Target[0] = GameObject.Find("/Characters/Boy").GetComponent<Transform>();
        _Target[1] = GameObject.Find("/Characters/Girl").GetComponent<Transform>();

        m_rbEnemy = GetComponent<Rigidbody>();
        m_nav = GetComponent<NavMeshAgent>();
        _BoyOrGirl = UnityEngine.Random.Range(0, 2); //was complaining about namespace ambiguity between System and UnityEngine
        m_timer = 5;

    }

    // Update is called once per frame
    protected void Update () 
    {
        ChooseTarget();
        //RotateForAttack();

        m_timer += Time.deltaTime;
        //Debug.Log("Update is being called!");
	}

    void ChooseTarget () //enemy will choose target based on random integer
    {
        
        if (!isCollided)
        {
            if (_BoyOrGirl == 1 && _Target[_BoyOrGirl].GetComponent<HealthController>().CurrentHealth == 0)
            {
                _BoyOrGirl = 0;
            }
            else if (_BoyOrGirl == 0 && _Target[_BoyOrGirl].GetComponent<HealthController>().CurrentHealth == 0)
            {
                _BoyOrGirl = 1;
            }
        }
    }

    void RotateForAttack () 
    {
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
            if (_Sprite)
                _Sprite.GetComponent<SpriteScript2>().ForcePlayerRotation(rotation);
        }
    }

    /*
     * Keegan 2018/7/2:
     * We need a different way to load enemies. This is going to create problems with the queue that currently exists.
     * 
     * Long term, create a way of loading enenies from prefabs, rather than just enabling or disabling them in the scene.
     * Then we can completely remove them from the scene without causing a data structure issue.
     */
    /*
    public void CleanUp()
    {
        EventManager.StopListening("cleanup", CleanUp);
        Destroy(gameObject);
    }
    */

    //abstract methods
    abstract public void OnTriggerEnter(Collider other);

    abstract public void OnTriggerStay(Collider other);

    abstract public void OnTriggerExit(Collider other);
    
    /*
     * Keegan 2018/7/2:
     * Whatever information the enemy needs to have for normal behaviour out of the gate goes here.
     * This will be so when the player engages in the "same" fight again, they'll be back at full health and won't be in the middle of some AI behaviour that no longer makes sense
     */
    abstract public void ResetToDefaults();

    abstract public void Stunned(GameObject StunAnim, float StunDuration);
}
