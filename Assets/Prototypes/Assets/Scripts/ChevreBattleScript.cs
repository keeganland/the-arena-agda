using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChevreBattleScript : MonoBehaviour {

    NavMeshAgent m_nav;
    int i;
    bool isCollided;

    private GameObject[] _Targets;
    private bool m_isChevreSpell;
    private float m_soundFX;

    private AudioSource m_audioSource;
    public AudioClip[] _AudioClip;


	// Use this for initialization
	void Start () {

        m_audioSource = GetComponent<AudioSource>();
        _Targets = GameObject.FindGameObjectsWithTag("Player");
        m_nav = GetComponent<NavMeshAgent>();
        i = Random.Range(0, 2);
	}
	
	// Update is called once per frame
	void Update () {
        m_soundFX += Time.deltaTime;
        if (!isCollided)
        {
            if (_Targets[i])
            {
                m_nav.SetDestination(_Targets[i].transform.position);
            }
            if( i == 1 && !_Targets[i])
            {
                i = 0;
            }
            else if( i == 0 && !_Targets[i])
            {
                i = 1;
            }
        }

        //Debug.Log("MeleeDamage: Damage Target" + m_target.name);
        AttackTimer += Time.deltaTime; //including this although I'm not sure I have to. Should the animation Timer do this for me?
        if (AttackTimer >= AttackSpeed && (this.GetComponentInChildren<RangeChecker>().InRange(_Targets[i])) && _Targets[i] != null)
        {
            if (!m_isChevreSpell)
            {
                StartCoroutine(CastChevreSpell());
            }
        }

        if (m_soundFX > 10)
        {
            m_audioSource.PlayOneShot(_AudioClip[1]);
            m_soundFX = 0;
        }

    }

    private void CancelChevreMovement()
    {
        isCollided = true;
        m_nav.SetDestination(m_nav.transform.position);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (_Targets[i])
        {
            if (other == _Targets[i].GetComponent<Collider>())
            {
                //Debug.Log("Target in Range " + curTarget.name);
                CancelChevreMovement();
                //Pass attack function here?
            }
            else return;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_Targets[i])
        {
            //Debug.Log("TriggerExit" + other.name);
            if (other == _Targets[i].GetComponent<Collider>())
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


    //-----------------Alex Modifications-----------//
    [SerializeField]
    private GameObject[] spellPrefab;
    // Use this for initialization

    private IEnumerator CastChevreSpell()
    {
        m_isChevreSpell = true;

        yield return new WaitForSeconds(1);

        Vector3 direction = _Targets[i].transform.position - transform.position;
        float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;

        transform.LookAt(_Targets[i].transform);

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
            _Sprite.GetComponent<SpriteScript2>().ForcePlayerRotation(rotation);
        }

        GameObject go = Instantiate(spellPrefab[0], _Targets[i].transform.position, Quaternion.Euler(0, -angle, 0));

        DamageData dmgData = new DamageData();
        dmgData.damage = Damage;

        MessageHandler msgHandler = _Targets[i].GetComponent<MessageHandler>();
        m_audioSource.PlayOneShot(_AudioClip[0]);

        if (msgHandler)
        {
            msgHandler.GiveMessage(MessageTypes.DAMAGED, this.gameObject, dmgData);
        }
        AttackTimer = 0.0f;

        go.gameObject.GetComponent<Spell>().SetTarget(_Targets[i]);
        go.gameObject.GetComponent<Bullet>().SpellFlare(angle);
        go.gameObject.GetComponent<Bullet>().GetSpellCaster(this.gameObject);

        m_isChevreSpell = false;
    }


}
