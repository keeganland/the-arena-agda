using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLoader : MonoBehaviour {

    public PublicVariableHolderArena publicVariableHolderArena;

    private List<GameObject> EnemyPrefabs;


    private void OnEnable()
    {
        EnemyPrefabs = new List<GameObject>();
        EnemyPrefabs.Add(publicVariableHolderArena._SheepPrefab);
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

    }
}
