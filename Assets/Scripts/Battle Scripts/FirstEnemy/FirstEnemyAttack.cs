using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FirstEnemyAttack : MonoBehaviour {

    public Vector3 m_targetpos;

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

    private GameObject[] _Targets;
    private bool m_isChevreSpell;
    private float m_soundFX;

    private AudioSource m_audioSource;
    public AudioClip[] _AudioClip;

    public int Damage;
    public int Aggro;
    public float AttackSpeed;
    public GameObject _Sprite;
    private float AttackTimer;


    //-----------------Alex Modifications-----------//
    [SerializeField]
    private GameObject[] spellPrefab;

	void Update ()
    {
        if (!isCollided)
        {
            if (i == 1 && _Targets[i].GetComponent<HealthController>().currentHealth == 0)
            {
                i = 0;
            }
            else if (i == 0 && _Targets[i].GetComponent<HealthController>().currentHealth == 0)
            {
                i = 1;
            }
            if (_Targets[i])
            {
                m_nav.SetDestination(_Targets[i].transform.position);
            }
        }

        //Debug.Log("MeleeDamage: Damage Target" + m_target.name);
        AttackTimer += Time.deltaTime; //including this although I'm not sure I have to. Should the animation Timer do this for me?
        if (AttackTimer >= AttackSpeed && (this.GetComponentInChildren<RangeChecker>().InRange(_Targets[i])) && _Targets[i] != null)
        {
            if (!m_isChevreSpell)
            {
                StartCoroutine(DashAttack());
            }
        }

	}

    private IEnumerator DashAttack()
    {
        m_isChevreSpell = true;

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


        //Start Attack
        GameObject go = Instantiate(spellPrefab[0], _Targets[i].transform.position, Quaternion.Euler(0, -angle, 0)); //warning

        yield return new WaitForSeconds(_WarningtoAttackCD);

        AttackTimer = 0.0f;   
        m_isChevreSpell = false;
    }
}


