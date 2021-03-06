﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ThirdEnemy : BasicEnemyBehaviour
{  
    private int i; // 0 = BombAttack, 1 = LaserAttack, 2 = UltimateAttack;

    //bool for coroutines

    private bool Bomb;
    private bool Laser;
    private bool Ultimate;

    private GameObject meteorLaunchAnimation;
    private GameObject laserGathering;
    private GameObject laserWarningGameObject;
    private GameObject UltimateGathering;
    private GameObject Warning;
    private GameObject UltimateAttackGameObject;
    private GameObject[] WarningMeteor;

    public Slider _CastSpellSlider;
    public GameObject _CastSpellGameobject;
    public Text _SpellCasttimer;

    public Slider _StunnedSlider;
    public GameObject _StunnedGameObject;
    public Text _StunnedTimer;
    private bool isStunned;

    private float StunDurationTime;
    private float StunActualtime;

    public float LineDrawSpeed;

    private float m_warningCastTime;
    public GameObject LaserBeamHit;
    public bool m_warningCastTimeBool;
    private Vector3 m_targetPos;
    private bool m_dashingAnim;
    private bool laser;
    private LineRenderer laserWarning;
    private bool laserWarningBool;
    public int Aggro;
    public bool StopAttacking;
    private float laserCounter;
    private float laserWarningCounter;
    private float laserDist;

    [Header("SpellsPrefab (normal, bomb, laser, ultimate")]
    public GameObject[] _AttackPrefabs;
   
    [Header("Attack Cooldowns (normal, bomb, laser, ultimate")]
    public float[] _AttackCD;

    [Header("Attack Cast Time (bomb, laser, ultimate")]
    public float[] _TimeWarningForSpell; // i = 0 is BombAttack, i = 1 is Laser, i = 2 is Ultimate;

    [Header("Attack Warning FX (bomb, laser, ultimate")]
    public GameObject[] _AttackWarningPrefabs;
    [Header("Attack Animation FX (bomb, laser, ultimate")]
    public GameObject[] _AttackAnimations;

    [Header("AttackDamages (normal, bomb, laser, ultimate")]
    public int[] _AttackDamage;

    [Header("BasicSpellData")]
    public float TimeBetweenMeteors = 2;
    public int NumberOfMeteors = 4;

    private GameObject[] Attack;
    private GameObject[] AttackFx;
    public LineRenderer[] LaserBeam;
    private NavMeshAgent meshAgent;

    [SerializeField] private float m_normalAttackTimer;
    [SerializeField] private float m_bombAttackTimer;
    [SerializeField] private float m_laserAttackTimer;
    [SerializeField] private float m_ultimateAttackTimer;

    new void Start()
    {
        base.Start();

        for (int i = 0; i < LaserBeam.Length; i++)
        {
            LaserBeam[i].sortingOrder = 10; 
            LaserBeam[i].SetPosition(0, transform.position);            
        }
   
        _BoyOrGirl = Random.Range(0, 2);
    }

    new void Update()
    {
        base.Update();

        if (laserWarningBool)
        {
            transform.LookAt(_Target[_BoyOrGirl]);

            laserWarning.SetPosition(0, transform.position);

            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit))
            {
                laserDist = Vector3.Distance(transform.position, hit.point);

                /*if (hit.collider && laserCounter >= laserDist)
                {
                   
                   laserWarning.SetPosition(1, hit.point);
                    
                }*/

                if (hit.collider && laserWarningCounter < laserDist)
                {
                    laserWarningCounter += .1f / LineDrawSpeed;

                    float x = Mathf.Lerp(0, laserDist, laserWarningCounter);

                    Vector3 pointA = transform.position;
                    Vector3 pointB = hit.point;

                    Vector3 pointAlLongLine = x * Vector3.Normalize(pointB - pointA) + pointA;

                    laserWarning.SetPosition(1, pointAlLongLine);
                }
                else if (hit.collider && laserWarningCounter >= laserDist)
                {
                    laserWarning.SetPosition(1, hit.point);
                }
            }
        }

        if (m_warningCastTimeBool == true)
        {
            if (m_warningCastTime < _TimeWarningForSpell[i])
            {
                m_warningCastTime += Time.deltaTime;
            }
            _CastSpellSlider.value = m_warningCastTime / _TimeWarningForSpell[i];
            _SpellCasttimer.text = System.Math.Round((float)(_TimeWarningForSpell[i] - m_warningCastTime), 2).ToString();
        }

        if (isStunned == true)
        {
            if (StunActualtime >= 0)
            {
                StunActualtime -= Time.deltaTime;
            }
            _StunnedSlider.value = StunActualtime / StunDurationTime;
            _StunnedTimer.text = System.Math.Round((float)(StunActualtime), 2).ToString();
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

        if (laser)
        {
            transform.LookAt(_Target[_BoyOrGirl]);
            for (int i = 0; i < LaserBeam.Length; i++)
            {
                LaserBeam[i].SetPosition(0, transform.position);
            }
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit))
            {
                if (hit.collider)
                {
                    laserDist = Vector3.Distance(transform.position, hit.point);

                    /*if(LaserBeamHit && laserCounter>= laserDist)
                    { 
                    LaserBeamHit.transform.position = hit.collider.transform.position;
                    for (int i = 0; i < LaserBeam.Length; i++)
                      {
                        LaserBeam[i].SetPosition(1, hit.point);
                      }
                    }*/

                    if (LaserBeamHit && laserCounter < laserDist)
                    {
                        laserCounter += .1f / LineDrawSpeed;

                        float x = Mathf.Lerp(0, laserDist, laserCounter);

                        Vector3 pointA = transform.position;
                        Vector3 pointB = hit.point;

                        Vector3 pointAlLongLine = x * Vector3.Normalize(pointB - pointA) + pointA;

                        for (int i = 0; i < LaserBeam.Length; i++)
                        {
                            LaserBeam[i].SetPosition(1, pointAlLongLine);
                        }
                        LaserBeamHit.transform.position = pointAlLongLine;
                    }
                    else
                    {
                        for (int i = 0; i < LaserBeam.Length; i++)
                        {
                            LaserBeam[i].SetPosition(1, hit.collider.transform.position);
                            LaserBeamHit.transform.position = hit.point;

                        }
                    }

                    if (hit.collider.tag == "Player")
                    {
                        MessageHandler msgHandler = hit.collider.GetComponent<MessageHandler>();
                        DamageData dmgData = new DamageData();
                        dmgData.damage = _AttackDamage[2];
                        if (msgHandler)
                        {
                            msgHandler.GiveMessage(MessageTypes.DAMAGED, this.gameObject, dmgData);
                            GameObject go = hit.collider.GetComponent<HealthController>().Sprite;
                            Canvas[] canvas = go.GetComponentsInChildren<Canvas>();

                            for (int i = 0; i < canvas.Length; i++)
                            {
                                if (canvas[i].GetComponentInChildren<DamageDisplayScript>())
                                    canvas[i].GetComponentInChildren<DamageDisplayScript>().GetDamageText(Color.red, _AttackDamage[2]);
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < LaserBeam.Length; i++)
                    {
                        LaserBeam[i].SetPosition(1, transform.forward * 5000);
                    }
                }            
            }
            CancelAttack();
        }
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
        if (gameObject.GetComponent<HealthController>().CurrentHealth == 0)
        {
            StopAllCoroutines();
        }
    }

    private IEnumerator CastBombAttack() //Work in Prgress
    {
        StopAttacking = true; //To Reset if  
        Bomb = true;
        i = 0;
        _BoyOrGirl = Random.Range(0, 2);

        //Look toward target and warn line

        //Slider
        m_warningCastTime = 0;
        _CastSpellGameobject.SetActive(true);
        _SpellCasttimer.enabled = true;
        _SpellCasttimer.text = System.Math.Round((float)(_TimeWarningForSpell[i]), 2).ToString();
        m_warningCastTimeBool = true;

        yield return new WaitForSeconds(2);

        Vector3 meteorLaunch = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1.9f);
         meteorLaunchAnimation = Instantiate(_AttackAnimations[0], meteorLaunch, Quaternion.identity);

        yield return new WaitForSeconds(_TimeWarningForSpell[i] - 2);

        m_warningCastTimeBool = false;
        _SpellCasttimer.enabled = false;
        _CastSpellGameobject.SetActive(false);

        //Start the Attack
        Vector3[] MeteorTargetpos = new Vector3[NumberOfMeteors];
        GameObject[] meteors = new GameObject[NumberOfMeteors];
        WarningMeteor = new GameObject[NumberOfMeteors];

        for (int i = 0; i < meteors.Length; i++)
        {
            MeteorTargetpos[i] = _Target[_BoyOrGirl].transform.position;
            WarningMeteor[i] = Instantiate(_AttackWarningPrefabs[0], MeteorTargetpos[i], Quaternion.identity);
            Destroy(WarningMeteor[i], 1.5f + TimeBetweenMeteors);
            yield return new WaitForSeconds(TimeBetweenMeteors);
            meteors[i] = Instantiate(_AttackPrefabs[1] , MeteorTargetpos[i], Quaternion.identity);
            meteors[i].GetComponent<Bullet>().Damage = _AttackDamage[1];
            yield return new WaitForSeconds(TimeBetweenMeteors + 0.5f);
        }

        m_bombAttackTimer = 0;
        Destroy(meteorLaunchAnimation);
        Bomb = false;
        StopAttacking = false;
    }

    private IEnumerator CastLaserAttack()
    {
        StopAttacking = true;
        Laser = true;
        i = 1;
        _BoyOrGirl = Random.Range(0, 2);

        //Look toward target and warn line

        //Slider
        m_warningCastTime = 0;
        _CastSpellGameobject.SetActive(true);
        _SpellCasttimer.enabled = true;
        _SpellCasttimer.text = System.Math.Round((float)(_TimeWarningForSpell[i]), 2).ToString();
        m_warningCastTimeBool = true;

        yield return new WaitForSeconds(1f);

        laserGathering = Instantiate(_AttackAnimations[1], transform.position, Quaternion.identity);

        laserWarningBool = true;

        laserWarningGameObject = Instantiate(_AttackWarningPrefabs[1], transform.position,Quaternion.identity);
        laserWarning = laserWarningGameObject.GetComponent<LineRenderer>();
        laserWarning.sortingOrder = 10;

        yield return new WaitForSeconds(_TimeWarningForSpell[i] - 1f);

        Destroy(laserGathering, 2f);

        m_warningCastTimeBool = false;
        _SpellCasttimer.enabled = false;
        _CastSpellGameobject.SetActive(false);
    
        //Start the Attack
    
        laser = true;
        LaserBeamHit.SetActive(true);
        //LaserBeamHit.GetComponent<ParticleSystem>().Play(); ;

        yield return new WaitForEndOfFrame();

        laserWarningBool = false;
        Destroy(laserWarningGameObject);

        _AttackAnimations[2].GetComponent<ParticleSystem>().Play();

        for (int i = 0; i < LaserBeam.Length; i++)
        {
            LaserBeam[i].enabled = true;
        }


        yield return new WaitForSeconds(5);


        for (int i = 0; i < LaserBeam.Length; i++)
        {
            LaserBeam[i].enabled = false;
        }

        LaserBeamHit.SetActive(false);
        //LaserBeamHit.GetComponent<ParticleSystem>().Stop(); ;
        laser = false;

        _AttackAnimations[2].GetComponent<ParticleSystem>().Stop();

        m_laserAttackTimer = 0;
        laserCounter = 0;
        laserWarningCounter = 0;
        Laser = false;
         StopAttacking = false;
    }

    private IEnumerator CastUltimateAttack()
    {
        StopAttacking = true;
        Ultimate = true;

        i = 2;

        m_warningCastTime = 0;
        _CastSpellGameobject.SetActive(true);
        _SpellCasttimer.enabled = true;
        _SpellCasttimer.text = System.Math.Round((float)(_TimeWarningForSpell[i]), 2).ToString();
        m_warningCastTimeBool = true;

        UltimateGathering = Instantiate(_AttackAnimations[3], transform);

        Warning = Instantiate(_AttackWarningPrefabs[2], transform.position, Quaternion.identity);       

        yield return new WaitForSeconds(_TimeWarningForSpell[i]-5f);

        CameraShake cameraShake = GameObject.FindWithTag("CameraHolder").GetComponent<CameraShake>();
        StartCoroutine(cameraShake.LaserShake(8f, .15f));

        yield return new WaitForSeconds(5f);

        m_warningCastTimeBool = false;
        _SpellCasttimer.enabled = false;
        _CastSpellGameobject.SetActive(false);

        Destroy(UltimateGathering);

        UltimateAttackGameObject = Instantiate(_AttackAnimations[4], transform);

        //Start the Attack


        yield return new WaitForSeconds(1f);

        ScreenFader.PlayLaserDevastation();

        yield return new WaitForSeconds(1.5f);


        foreach (Transform player in _Target)
        {
            MessageHandler msgHandler = player.GetComponent<MessageHandler>();
            DamageData dmgData = new DamageData();
            dmgData.damage = _AttackDamage[3];
            if (msgHandler)
            {
                msgHandler.GiveMessage(MessageTypes.DAMAGED, this.gameObject, dmgData);
                GameObject go = player.GetComponent<HealthController>().Sprite;
                Canvas[] canvas = go.GetComponentsInChildren<Canvas>();

                for (int i = 0; i < canvas.Length; i++)
                {
                    if (canvas[i].GetComponentInChildren<DamageDisplayScript>())
                        canvas[i].GetComponentInChildren<DamageDisplayScript>().GetDamageText(Color.red, _AttackDamage[3]);
                }
            }
        }
        Destroy(UltimateAttackGameObject);
        Destroy(Warning);

        m_ultimateAttackTimer = 0;
        Ultimate = false;
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
    } //Finished

    public override void ResetToDefaults()
    {
        throw new System.NotImplementedException();
    }

    public override void Stunned(GameObject StunAnim, float StunDuration)
    {
        //      Alex Stun Enemy3:

        //    All:

        //        StopAttacking->False if (Bomb, Laser, Ultimate && true).
        //m_warningCastTimeBool->False if (Bomb, Laser, Ultimate && true)
        //_SpellCasttimer.enabled->False if (Bomb, Laser, Ultimate && true)
        //_CastSpellGameobject.SetActive->False if (Bomb, Laser, Ultimate && true)

        //Bomb:

        //        m_bonbAttackTimer = 0 if (Bomb)

        //        Laser:

        //            LaserWarningBool = false if (Laser && true)
        //            laser = false if (Laser && true)
        //            LaserBeamHit.SetActive = false if (Laser && true)
        //            LaserBeam[i].enabled = false if (Laser && true)

        //            m_laserAttackTimer = 0 if (Laser)
        //            laserCounter = 0 if (Laser)
        //            laserWarningCounter = 0 if (Laser)

        //            _AttackAnimations[2].GetComponent<ParticleSystem>().Stop() if (Laser)

        //        Ultimate:

        //            Destroy “UltimateGatherning”
        //Destroy “Warning” 
        //Destroy(UltimateAttack)

        //m_ultimateAttackTimer = 0;

        StunDurationTime = StunDuration;
        StunActualtime = StunDuration;

        if(Bomb)
        {
            StopCoroutine("CastBombAttack");

            if(meteorLaunchAnimation)
                Destroy(meteorLaunchAnimation);
            
            //for (int i = 0; i < WarningMeteor.Length;i++)
            //{
            //    if (WarningMeteor[i])
            //        Destroy(WarningMeteor[i]);
            //}
            
            m_bombAttackTimer = 0;

        }

        if(Laser)
        {
            StopCoroutine("CastLaserAttack");

            if (laserWarningBool)
                laserWarningBool = false;
            if (laser)
                laser = false;
            if (LaserBeamHit.activeSelf == true)
                LaserBeamHit.SetActive(false);
            for (int i = 0; i < LaserBeam.Length; i++)
            {
                if(LaserBeam[i].enabled)
                LaserBeam[i].enabled = false;
            }

            if (laserGathering)
                Destroy(laserGathering);
            if (laserWarningGameObject)
                Destroy(laserWarningGameObject);
            if (UltimateAttackGameObject)
                Destroy(UltimateAttackGameObject);

            laserCounter = 0;
            laserWarningCounter = 0;
            if (_AttackAnimations[2].GetComponent<ParticleSystem>().isPlaying == true)
                _AttackAnimations[2].GetComponent<ParticleSystem>().Stop();
            
            m_laserAttackTimer = 0;
        }

        if(Ultimate)
        {
            StopCoroutine("CastUltimateAttack");

            if (UltimateGathering)
                Destroy(UltimateGathering);
            if (Warning)
                Destroy(Warning);
            
            m_ultimateAttackTimer = 0;
        }

        if (m_warningCastTimeBool)
            m_warningCastTimeBool = false;
        if (_SpellCasttimer.enabled)
            _SpellCasttimer.enabled = false;
        if (_CastSpellGameobject.activeSelf == true)
            _CastSpellGameobject.SetActive(false);
        
        StartCoroutine(StunCoroutine(StunAnim));
    }

    private IEnumerator StunCoroutine(GameObject Stun)
    {
        _StunnedGameObject.SetActive(true);
        _StunnedTimer.enabled = true;

        isStunned = true;

        Debug.Log("Stun start");
        GameObject StunAnim = Instantiate(Stun, transform.position + new Vector3(0,0,1.4f), Quaternion.Euler(0, 35, 0));
        yield return new WaitForSeconds(StunDurationTime);
        StunAnim.GetComponent<ParticleSystem>().Stop();
        Destroy(StunAnim, 1f);
        Debug.Log("Stun end");
        StopAttacking = false;
        isStunned = false;

        _StunnedGameObject.SetActive(false);
        _StunnedTimer.enabled = false;
    }

    //Not used but need to be inherited from BasicEnemyBehaviour
    public override void OnTriggerEnter(Collider other)
    {
        throw new System.NotImplementedException();
    }

    public override void OnTriggerStay(Collider other)
    {
        throw new System.NotImplementedException();
    }

    public override void OnTriggerExit(Collider other)
    {
        throw new System.NotImplementedException();
    }
}

