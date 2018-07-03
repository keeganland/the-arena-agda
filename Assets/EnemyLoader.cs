using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLoader : MonoBehaviour {

    public PublicVariableHolderArena publicVariableHolderArena;

    [SerializeField] private List<GameObject> EnemyPrefabs;


    private void OnEnable()
    {
        /*
         * Keegan note: im doing this like an idiot until i get used to Resources.Load
         */

        GameObject enemy1 = Resources.Load("Prefabs/Arena/FirstEnemy") as GameObject;
        GameObject enemy2 = Resources.Load("Prefabs/Arena/SecondEnemyPrefab") as GameObject;
        GameObject enemy3 = Resources.Load("Prefabs/Arena/ThirdEnemy") as GameObject;

        EnemyPrefabs = new List<GameObject>();
        EnemyPrefabs.Add(enemy1);
        EnemyPrefabs.Add(enemy2);
        EnemyPrefabs.Add(enemy3);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

    }
}
