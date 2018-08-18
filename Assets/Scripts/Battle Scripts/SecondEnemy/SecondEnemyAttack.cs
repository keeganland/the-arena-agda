using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SecondEnemyAttack : BasicEnemyBehaviour {

    public Slider _CastSpellSlider;
    public GameObject _CastSpellGameobject;
    public Text _SpellCasttimer;

    public PublicVariableHolderArena publicVariableHolderArena;

    public Color RedSheepcolor;

    private GameObject SheepPrefab;
    private Transform[] TeleportPosition;
    private Transform[] SpawnPosition;
    private ParticleSystem _SpawnAnimationPrefab;
    private ParticleSystem TeleportStart;
    private ParticleSystem TeleportArrives;
    [SerializeField] private float spawnTime;
    [SerializeField] private float teleportTime;
    private int currentTeleportPosition;
    private bool isCoroutineStarted;
    private float m_castTime;
    private bool TeleportStarted; //bool for the "Beginning animation of teleportation
    private bool TeleportEndStarted; //bool for the "Arrival animation" of teleportation


    [Header("Totem Characteristics")]
    public float spawnCD = 30;
    public float teleportCD = 180;
    public float castingTime;
    public float spawnParticleDelay;
    public float teleportAnimStart; //float that set the start of the animation (you want a to start before for a smooth anim)
    public float teleportAnimEnd; //same as before but for the "end" animation
    [Header("Sheep Characteristics")]
    public int Sheephealth = 15;
    public int RedSheephealth = 55;

    private void OnEnable()
    {
        ResetToDefaults();
    }
    
	// Update is called once per frame
	void Update () {
		
        if(spawnTime >= spawnCD && !isCoroutineStarted)
        {
            StartCoroutine("SpawnSheeps");
        }

        if(teleportTime >= (teleportCD - teleportAnimStart) && !TeleportStarted)
        {
            Debug.Log("here");
            TeleportStart.Play();
            TeleportStarted = true;
        }

        if(teleportTime >= teleportCD - teleportAnimEnd)
        {
            if (!TeleportEndStarted)
            {
                StartCoroutine("teleportArrives");
                TeleportEndStarted = true;
            }

            if (teleportTime >= teleportCD)
            {
                int i = Random.Range(0, TeleportPosition.Length);
                if (currentTeleportPosition != i)
                {
                    transform.position = new Vector3(TeleportPosition[i].position.x, transform.position.y, TeleportPosition[i].position.z);
                    currentTeleportPosition = i;
                    teleportTime = 0;
                    TeleportStarted = false;
                }
            }
        }

        spawnTime += Time.deltaTime;
        teleportTime += Time.deltaTime;
	}

    private IEnumerator teleportArrives()
    {
        int i = Random.Range(0, TeleportPosition.Length);
        while (currentTeleportPosition == i)
        {
            i = Random.Range(0, TeleportPosition.Length);
        }
        TeleportArrives.transform.position = new Vector3(TeleportPosition[i].position.x, transform.position.y, TeleportPosition[i].position.z);
        TeleportArrives.Play();
        yield return new WaitForSeconds(teleportAnimEnd);

        transform.position = new Vector3(TeleportPosition[i].position.x, transform.position.y, TeleportPosition[i].position.z);
        currentTeleportPosition = i;    
        teleportTime = 0;
        TeleportStarted = false;
        TeleportEndStarted = false;
    }

    private void FixedUpdate()
    {
        if(isCoroutineStarted == true)
        {
            if (m_castTime < castingTime)
            {
                m_castTime += Time.fixedDeltaTime;
            }
            _CastSpellSlider.value = m_castTime / castingTime;
            _SpellCasttimer.text = System.Math.Round((float)(castingTime - m_castTime), 2).ToString();
        }
    }

    IEnumerator SpawnSheeps()
    {
        isCoroutineStarted = true;

        m_castTime = 0;
        _CastSpellGameobject.SetActive(true);
        _SpellCasttimer.enabled = true;

        yield return new WaitForSeconds(castingTime - spawnParticleDelay);

        ParticleSystem spawnparticle1 = Instantiate(_SpawnAnimationPrefab, SpawnPosition[0].position, Quaternion.Euler(-90,0,0));
        ParticleSystem spawnparticle2 = Instantiate(_SpawnAnimationPrefab, SpawnPosition[1].position, Quaternion.Euler(-90,0,0));
        ParticleSystem spawnparticle3 = Instantiate(_SpawnAnimationPrefab, SpawnPosition[2].position, Quaternion.Euler(-90,0,0));

        spawnparticle1.transform.SetParent(null);
        spawnparticle2.transform.SetParent(null);
        spawnparticle3.transform.SetParent(null);

        spawnparticle1.Play();
        spawnparticle2.Play();
        spawnparticle3.Play();
        //SpawnParticle[0].Play();
        //SpawnParticle[1].Play();
        //SpawnParticle[2].Play();

        yield return new WaitForSeconds(spawnParticleDelay);

        _CastSpellGameobject.SetActive(false);
        _SpellCasttimer.enabled = false;

        GameObject sheep1 = Instantiate(SheepPrefab, SpawnPosition[0].position, Quaternion.identity);
        GameObject sheep2 = Instantiate(SheepPrefab, SpawnPosition[1].position, Quaternion.identity);
        GameObject sheep3 = Instantiate(SheepPrefab, SpawnPosition[2].position, Quaternion.identity);

        /*
         * Keegan NTS 2018/7/2:
         * Because the sheep doesn't move by default now
         */

        sheep1.GetComponentInChildren<FirstEnemyAttack2>().isEnemyMoving = true;
        sheep2.GetComponentInChildren<FirstEnemyAttack2>().isEnemyMoving = true;
        sheep3.GetComponentInChildren<FirstEnemyAttack2>().isEnemyMoving = true;

        sheep1.transform.SetParent(null);
        sheep2.transform.SetParent(null);
        sheep3.transform.SetParent(null);

        sheep1.GetComponentInChildren<HealthController>().currentHealth = Sheephealth;
        sheep2.GetComponentInChildren<HealthController>().currentHealth = Sheephealth;
        sheep3.GetComponentInChildren<HealthController>().currentHealth = RedSheephealth;

        sheep1.GetComponentInChildren<HealthController>().totalHealth = Sheephealth;
        sheep2.GetComponentInChildren<HealthController>().totalHealth = Sheephealth;
        sheep3.GetComponentInChildren<HealthController>().totalHealth = RedSheephealth;

        sheep1.GetComponentInChildren<HealthController>().GameObjectName = "Small Sheep";
        sheep2.GetComponentInChildren<HealthController>().GameObjectName = "Small Sheep";
        sheep3.GetComponentInChildren<HealthController>().GameObjectName = "Red Sheep";

        sheep3.GetComponentInChildren<SpriteRenderer>().color = RedSheepcolor;

        sheep1.GetComponentInChildren<HealthUI>().UpdateUi(Sheephealth, Sheephealth);
        sheep2.GetComponentInChildren<HealthUI>().UpdateUi(Sheephealth, Sheephealth);
        sheep3.GetComponentInChildren<HealthUI>().UpdateUi(RedSheephealth, RedSheephealth);

        sheep1.GetComponentInChildren<HealthController>().isBoss = false;
        sheep2.GetComponentInChildren<HealthController>().isBoss = false;
        sheep3.GetComponentInChildren<HealthController>().isBoss = false;

        Destroy(spawnparticle1, 2f);
        Destroy(spawnparticle2, 2f);
        Destroy(spawnparticle3, 2f);

        yield return new WaitForSeconds(1);

        spawnTime = 0;
        isCoroutineStarted = false;
    }


    /*
     * Unimplemented
     * 
     */

    public override void OnTriggerEnter(Collider other)
    {
    }

    public override void OnTriggerStay(Collider other)
    {
    }

    public override void OnTriggerExit(Collider other)
    {
    }

    public override void ResetToDefaults()
    {
        InitializeEnemy();
    }

    private void InitializeEnemy()
    {
        SheepPrefab = publicVariableHolderArena._SheepPrefab;
        TeleportPosition = publicVariableHolderArena._TotemTeleportPos;
        SpawnPosition = publicVariableHolderArena._Spawnposition;
        //SpawnParticle = publicVariableHolderArena._SpawnAnimation;
        TeleportStart = publicVariableHolderArena._TeleportStart;
        TeleportArrives = publicVariableHolderArena._TeleportArrives;
        _SpawnAnimationPrefab = publicVariableHolderArena._SpawnSheepAnim;

        TeleportArrives.transform.SetParent(null);

        spawnTime = 20;

    }

    public override void Stunned(GameObject StunAnim, float StunDuration)
    {
        throw new System.NotImplementedException();
    }
}

