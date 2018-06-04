using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SecondEnemyAttack : MonoBehaviour {

    public PublicVariableHolderArena publicVariableHolderArena;
    public float spawnCD;
    public float teleportCD;
    public Slider _CastSpellSlider;
    public GameObject _CastSpellGameobject;
    public Text _SpellCasttimer;

    private GameObject SheepPrefab;
    private Transform[] TeleportPosition;
    private Transform[] SpawnPosition;
    private float time;

	// Use this for initialization
	void Start () { 

        SheepPrefab = publicVariableHolderArena._SheepPrefab;
        TeleportPosition = publicVariableHolderArena._TotemTeleportPos;
	}
	
	// Update is called once per frame
	/*void Update () {
		
        if(time>= spawnCD)
        {
            GameObject sheep1 = Instantiate(SheepPrefab);
            GameObject sheep2 = Instantiate(SheepPrefab);
            GameObject sheep3 = Instantiate(SheepPrefab);
        }
	}*/
}

