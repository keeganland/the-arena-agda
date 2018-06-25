using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ThirdEnemy : BasicEnemyBehaviour
{
    private int i; // 0 = BombAttack, 1 = LaserAttack, 2 = UltimateAttack;

    public Slider _CastSpellSlider;
    public GameObject _CastSpellGameobject;
    public Text _SpellCasttimer;

    private float m_warningCastTime;
    public bool m_warningCastTimeBool;
    private Vector3 m_targetPos;
    private bool m_dashingAnim;

    public int Aggro;
    public bool StopAttacking;

    [Header("SpellsPrefab (normal, bomb, laser, ultimate")]
    public GameObject[] _AttackPrefabs;
   
    [Header("Attack Cooldowns (normal, bomb, laser, ultimate")]
    public float[] _AttackCD;

    [Header("Attack Cast Time (bomb, laser, ultimate")]
    public float[] _TimeWarningForSpell; // i = 0 is BombAttack, i = 1 is Laser, i = 2 is Ultimate;

    [Header("Attack Warning FX (bomb, laser, ultimate")]
    public GameObject[] _AttackWarningPrefabs;

    [Header("AttackDamages (normal, bomb, laser, ultimate")]
    public int[] _AttackDamage;

    private GameObject[] Attack;
    private GameObject[] AttackFx;

    private NavMeshAgent meshAgent;

    [SerializeField] private float m_normalAttackTimer;
    [SerializeField] private float m_bombAttackTimer;
    [SerializeField] private float m_laserAttackTimer;
    [SerializeField] private float m_ultimateAttackTimer;

    new void Start()
    {
        base.Start();

        _BoyOrGirl = Random.Range(0, 2);
    }

    new void Update()
    {
        base.Update();

        if (m_warningCastTimeBool == true)
        {
            if (m_warningCastTime < _TimeWarningForSpell[i])
            {
                m_warningCastTime += Time.deltaTime;
            }
            _CastSpellSlider.value = m_warningCastTime / _TimeWarningForSpell[i];
            _SpellCasttimer.text = System.Math.Round((float)(_TimeWarningForSpell[i] - m_warningCastTime), 2).ToString();
        }

        if (!StopAttacking)
        {
            m_normalAttackTimer += Time.deltaTime;
            m_bombAttackTimer += Time.deltaTime;
            m_laserAttackTimer += Time.deltaTime; 
            m_ultimateAttackTimer += Time.deltaTime;

            NormalAttack();
            BombAttack();
            LaserAttack();
            UltimateAttack();
        }

        CancelAttack();
    }

    private void NormalAttack()
    {
        if(m_normalAttackTimer >= _AttackCD[0])
        {
            CastNormalAttack();
            m_normalAttackTimer = 0;
        }
    }

    private void BombAttack()
    {
        if (m_bombAttackTimer >= _AttackCD[1])
        {
            StartCoroutine("CastBombAttack");
        }
    }

    private void LaserAttack()
    {
        if (m_laserAttackTimer >= _AttackCD[2])
        {
            StartCoroutine("CastLaserAttack");
        }   
    }

    private void UltimateAttack()
    {
        if (m_ultimateAttackTimer >= _AttackCD[3])
        {
            StartCoroutine("CastUltimateAttack");
        }
    }

    private void CancelAttack()
    {
        if (gameObject.GetComponent<HealthController>().currentHealth == 0)
        {
            StopAllCoroutines();
        }
    }

    public override void OnTriggerEnter(Collider other)
    {
        //if (_Target[_BoyOrGirl] && !StopAttacking)
        //{
        //    if (other == _Target[_BoyOrGirl].GetComponent<Collider>())
        //    {
        //        //Debug.Log("Target in Range " + curTarget.name)
        //        if (m_timer >= _AttackCD[i])
        //        {
        //            //Debug.Log("here");
        //            CancelDashMovement();
        //            StartCoroutine("BombAttack");
        //        }
        //    }
        //    else return;
        //}
        return;
    }

    public override void OnTriggerStay(Collider other)
    {
        ////Debug.Log(other.name);
        //if (_Target[_BoyOrGirl] && !StopAttacking)
        //{
        //    if (other == _Target[_BoyOrGirl].GetComponent<Collider>())
        //    {
        //        CancelDashMovement();
        //        //Debug.Log("Target in Range " + curTarget.name)
        //        if (m_timer >= _AttackCD[i])
        //        {
        //            transform.LookAt(_Target[_BoyOrGirl]);
        //            StartCoroutine("BombAttack");
        //        }
        //    }
        //    else return;
        //}
        return;
    }

    public override void OnTriggerExit(Collider other)
    {
        //if (_Target[_BoyOrGirl])
        //{
        //    //Debug.Log("TriggerExit" + other.name);
        //    if (other == _Target[_BoyOrGirl].GetComponent<Collider>())
        //    {
        //        isCollided = false;
        //        //will have player chase target once target leaves attack range trigger
        //        //Debug.Log("Target out of range " + curTarget.name);
        //    }
        //}
        return;
    }

    private IEnumerator CastBombAttack()
    {
        StopAttacking = true;
        i = 0;
        _BoyOrGirl = Random.Range(0, 2);

        //Look toward target and warn line

        //Slider
        m_warningCastTime = 0;
        _CastSpellGameobject.SetActive(true);
        _SpellCasttimer.enabled = true;
        _SpellCasttimer.text = System.Math.Round((float)(_TimeWarningForSpell[i]), 2).ToString();
        m_warningCastTimeBool = true;

        yield return new WaitForSeconds(_TimeWarningForSpell[i]);

        m_warningCastTimeBool = false;
        _SpellCasttimer.enabled = false;
        _CastSpellGameobject.SetActive(false);

        //Start the Attack

        m_bombAttackTimer = 0;
        StopAttacking = false;
    }

    private IEnumerator CastLaserAttack()
    {
        StopAttacking = true;
        i = 1;
        _BoyOrGirl = Random.Range(0, 2);

        //Look toward target and warn line

        //Slider
        m_warningCastTime = 0;
        _CastSpellGameobject.SetActive(true);
        _SpellCasttimer.enabled = true;
        _SpellCasttimer.text = System.Math.Round((float)(_TimeWarningForSpell[i]), 2).ToString();
        m_warningCastTimeBool = true;

        yield return new WaitForSeconds(_TimeWarningForSpell[i]);

        m_warningCastTimeBool = false;
        _SpellCasttimer.enabled = false;
        _CastSpellGameobject.SetActive(false);

        //Start the Attack

        m_laserAttackTimer = 0;
        StopAttacking = false;
    }

    private IEnumerator CastUltimateAttack()
    {
        StopAttacking = true;
        i = 2;
        _BoyOrGirl = Random.Range(0, 2);

        //Look toward target and warn line

        //Slider
        m_warningCastTime = 0;
        _CastSpellGameobject.SetActive(true);
        _SpellCasttimer.enabled = true;
        _SpellCasttimer.text = System.Math.Round((float)(_TimeWarningForSpell[i]), 2).ToString();
        m_warningCastTimeBool = true;

        yield return new WaitForSeconds(_TimeWarningForSpell[i]);

        m_warningCastTimeBool = false;
        _SpellCasttimer.enabled = false;
        _CastSpellGameobject.SetActive(false);

        //Start the Attack


        m_ultimateAttackTimer = 0;
        StopAttacking = false;
    }

    private void CastNormalAttack()
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
            _Sprite.GetComponent<SpriteScript2>().ForcePlayerRotation(rotation);
        }

        GameObject go = Instantiate(_AttackPrefabs[0], transform.position, Quaternion.Euler(0, -angle, 0));
        go.gameObject.GetComponent<Spell>().SetTarget(_Target[_BoyOrGirl].gameObject);
        //go.gameObject.GetComponent<Bullet>().GetAggro(Aggro);
        go.gameObject.GetComponent<Bullet>().SpellFlare(angle);
        go.gameObject.GetComponent<Bullet>().SetSpellCaster(this.gameObject);


        _BoyOrGirl = Random.Range(0, 2);
    }
}

