using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightTracker : MonoBehaviour {

    public List<GameObject> enemies; //Keegan 2018/7/1: switch to private once testing is done

    //private bool fightActivated = false; //useless, var below's -1 is logically the same thing
    private int currentFight = -1; // -1 means no current fight
    

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

    public void activateFight(int n)
    {
        if(currentFight >= 0)
        {
            throw new System.Exception("Tried to activate a fight when a previous fight was already activated! Be sure to shut down the earlier fight so that only one fight is active at a time!");
        }
        enemies[n].SetActive(true);
        currentFight = n;
    }

    /*
     * Might not bee too good, because we have to look at every single enemy to make sure none are activated at an inapprorpriate time.
     * But then the size of our enemy list shouldn't ever get too out of control.
     */
    public void deactivateFight(int n)
    {
        enemies[n].SetActive(false);
        if (!isAnythingActive())
        {
            currentFight = -1;
        }
    }

    public void deactivateCurrentFight()
    {
        if(currentFight < 0)
        {
            throw new System.Exception("Tried to deactivate a fight with no active fight");
        }

        enemies[currentFight].SetActive(false);
        currentFight = -1;
    }

    public void deactivateAllFights()
    {
        foreach (GameObject enemy in enemies)
        {
            enemy.SetActive(false);
        }
        currentFight = -1;
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
