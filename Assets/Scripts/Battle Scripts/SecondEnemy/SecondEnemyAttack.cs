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
    [SerializeField] private float spawnTime;
    [SerializeField] private float teleportTime;
    private int currentTeleportPosition;
    private bool isCoroutineStarted;

    [Header("Totem Characteristics")]
    public float spawnCD = 30;
    public float teleportCD = 180;
    [Header("Sheep Characteristics")]
    public int Sheephealth = 10;
    public int RedSheephealth = 55;

	// Use this for initialization
	void Start () { 

        SheepPrefab = publicVariableHolderArena._SheepPrefab;
        TeleportPosition = publicVariableHolderArena._TotemTeleportPos;
        SpawnPosition = publicVariableHolderArena._Spawnposition;

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

    IEnumerator SpawnSheeps()
    {
        isCoroutineStarted = true;

        GameObject sheep1 = Instantiate(SheepPrefab, SpawnPosition[0].position, Quaternion.identity);
        GameObject sheep2 = Instantiate(SheepPrefab, SpawnPosition[1].position, Quaternion.identity);
        GameObject sheep3 = Instantiate(SheepPrefab, SpawnPosition[2].position, Quaternion.identity);

        Debug.Log(sheep1.name);
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

