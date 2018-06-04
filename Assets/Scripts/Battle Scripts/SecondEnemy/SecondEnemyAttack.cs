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
    private float spawnTime;
    private float teleportTime;
    private int currentTeleportPosition;

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
		
        if(spawnTime >= spawnCD)
        {
            GameObject sheep1 = Instantiate(SheepPrefab, SpawnPosition[0]);
            GameObject sheep2 = Instantiate(SheepPrefab, SpawnPosition[1]);
            GameObject sheep3 = Instantiate(SheepPrefab, SpawnPosition[2]);

            GameObject Redsheep = sheep3.GetComponent<HealthController>().Sprite;

            Redsheep.GetComponent<SpriteRenderer>().color = RedSheepcolor;

            sheep1.GetComponent<HealthController>().currentHealth = Sheephealth;
            sheep2.GetComponent<HealthController>().currentHealth = Sheephealth;
            sheep3.GetComponent<HealthController>().currentHealth = RedSheephealth;

            spawnTime = 0;
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
}

