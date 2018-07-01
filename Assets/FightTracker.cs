using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightTracker : MonoBehaviour {

    public List<GameObject> enemies; //Keegan 2018/7/1: switch to private once testing is done

    private bool fightActivated = false;
    

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

    public void activateFight(int n)
    {
        if(fightActivated)
        {
            throw new System.Exception("Tried to activate a fight when a previous fight was already activated! Be sure to shut down the earlier fight so that only one fight is active at a time!");
        }
        enemies[n].SetActive(true);
        fightActivated = true;
    }

    /*
     * Might not bee too good, because we have to look at every single enemy to make sure none are activated at an inapprorpriate time.
     * But then the size of our enemy list shouldn't ever get too out of control.
     */
    public void deactivateFight(int n)
    {
        enemies[n].SetActive(false);
        fightActivated = isAnythingActive();
    }


    public bool isAnythingActive()
    {
        foreach (GameObject enemy in enemies)
        {
            if (enemy.activeInHierarchy)
            {
                return true;
            }
        }

        return false;
    }
}
