using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SecondEnemyAttack : MonoBehaviour {

    public Slider _CastSpellSlider;
    public GameObject _CastSpellGameobject;
    public Text _SpellCasttimer;

    public PublicVariableHolderArena publicVariableHolderArena;

    public Color RedSheepcolor;

    private GameObject SheepPrefab;
    private Transform[] TeleportPosition;
    private Transform[] SpawnPosition;
    private ParticleSystem[] SpawnParticle;
    [SerializeField] private float spawnTime;
    [SerializeField] private float teleportTime;
    private int currentTeleportPosition;
    private bool isCoroutineStarted;
    private float m_castTime;

    [Header("Totem Characteristics")]
    public float spawnCD = 30;
    public float teleportCD = 180;
    public float castingTime;
    public float spawnParticleDelay;
    [Header("Sheep Characteristics")]
    public int Sheephealth = 15;
    public int RedSheephealth = 55;

	// Use this for initialization
	void Start () { 

        SheepPrefab = publicVariableHolderArena._SheepPrefab;
        TeleportPosition = publicVariableHolderArena._TotemTeleportPos;
        SpawnPosition = publicVariableHolderArena._Spawnposition;
        SpawnParticle = publicVariableHolderArena._SpawnAnimation;

        spawnTime = 20;
	}
	
	// Update is called once per frame
	void Update () {
		
        if(spawnTime >= spawnCD && !isCoroutineStarted)
        {
            StartCoroutine("SpawnSheeps");
        }

        if(teleportTime >= teleportCD)
        {
            int i = Random.Range(0, TeleportPosition.Length + 1);
            if (currentTeleportPosition != i)
            {
                transform.position = TeleportPosition[i].position;
                currentTeleportPosition = i;
                teleportTime = 0;
            }
        }

        spawnTime += Time.deltaTime;
        teleportTime += Time.deltaTime;
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

        SpawnParticle[0].Play();
        SpawnParticle[1].Play();
        SpawnParticle[2].Play();

        yield return new WaitForSeconds(spawnParticleDelay);

        _CastSpellGameobject.SetActive(false);
        _SpellCasttimer.enabled = false;

        GameObject sheep1 = Instantiate(SheepPrefab, SpawnPosition[0].position, Quaternion.identity);
        GameObject sheep2 = Instantiate(SheepPrefab, SpawnPosition[1].position, Quaternion.identity);
        GameObject sheep3 = Instantiate(SheepPrefab, SpawnPosition[2].position, Quaternion.identity);

        sheep1.transform.SetParent(null);
        sheep2.transform.SetParent(null);
        sheep3.transform.SetParent(null);

        sheep1.GetComponentInChildren<HealthController>().currentHealth = Sheephealth;
        sheep2.GetComponentInChildren<HealthController>().currentHealth = Sheephealth;
        sheep3.GetComponentInChildren<HealthController>().currentHealth = RedSheephealth;

        sheep1.GetComponentInChildren<HealthUI>().UpdateUi(Sheephealth, Sheephealth);
        sheep2.GetComponentInChildren<HealthUI>().UpdateUi(Sheephealth, Sheephealth);
        sheep3.GetComponentInChildren<HealthUI>().UpdateUi(RedSheephealth, RedSheephealth);


        yield return new WaitForSeconds(1);

        spawnTime = 0;
        isCoroutineStarted = false;
    }
}

