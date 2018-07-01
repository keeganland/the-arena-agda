using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightTracker : MonoBehaviour {

    public List<GameObject> enemies; //Keegan 2018/7/1: switch to private once testing is done
    

    protected void OnEnable()
    {
        enemies = new List<GameObject>();

    }

    // Use this for initialization
    void Start() {
        /* Refernces used:
         * https://answers.unity.com/questions/594210/get-all-children-gameobjects.html
         * https://answers.unity.com/questions/205391/how-to-get-list-of-child-game-objects.html
         */

        foreach (Transform child in transform)
        {
            enemies.Add(child.gameObject);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
