/**
 * Refactor of FightLoaderTester and FightTracker into a single script
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightLoader : MonoBehaviour {

    public List<GameObject> enemies; //Like my note in FightTracker says, this is public for testing purposes. Switch to private later
    private int currentFight = -1; // negative means no current fight

    //The testing related menu in the arena itself, from FightLoaderTester
    public static bool gameIsPaused = false;
    public GameObject fightMenuUI;
    public bool testSpecificFight;
    public int specificFightToTest = 0;


	// Use this for initialization
	void Start () {
        ////////////////////////////////////////
        //The below comes from FightTracker.cs//
        ////////////////////////////////////////
        /* Refernces used:
         * https://answers.unity.com/questions/594210/get-all-children-gameobjects.html
         * https://answers.unity.com/questions/205391/how-to-get-list-of-child-game-objects.html
         */

        //how does this thing work again? it just gets the enemy transforms? how?
        foreach (Transform child in transform)
        {
            enemies.Add(child.gameObject);
        }
        /////////////////////////////////////////////
        //The below comes from FightLoaderTester.cs//
        /////////////////////////////////////////////

        specificFightToTest = SetUp.Instance.GetFightToLoad();

        if (testSpecificFight)
        {
            switch (specificFightToTest)
            {
                case 1:
                    Fight1();
                    break;
                case 2:
                    Fight2();
                    break;
                case 3:
                    Fight3();
                    break;
                default:
                    Fight1();
                    break;
            }
        }
    }

    /* Functions whose original home was in FightLoaderTester.cs:
     * Update
     * Resume
     * Pause
     * Fight1
     * Fight2
     * Fight3
     */


    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    private void Resume()
    {
        fightMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    private void Pause()
    {
        fightMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void Fight1()
    {
        try
        {
            //ft.deactivateAllFights();
            this.deactivateAllFights();
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }
        finally
        {
            EventManager.TriggerEvent("cleanup");
            //ft.activateFight(0);
            this.activateFight(0);
            Resume();
            EventManager.TriggerEvent("enterArenaFight1");
        }
    }

    public void Fight2()
    {
        try
        {
            //ft.deactivateAllFights();
            this.deactivateAllFights();
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }
        finally
        {
            EventManager.TriggerEvent("cleanup");
            //ft.activateFight(1);
            this.activateFight(1);
            Resume();
            EventManager.TriggerEvent("enterArenaFight2");
        }
    }

    public void Fight3()
    {
        try
        {
            //ft.deactivateAllFights();
            this.deactivateAllFights();
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }
        finally
        {
            EventManager.TriggerEvent("cleanup");
            //ft.activateFight(2);
            this.activateFight(2);
            Resume();
        }
    }



    /* Functions whose original home was in FightTracker.cs:
     * activateFight
     * deactivateFight
     * deactivateCurrentFight
     * isAnythingActive
     * 
     */

    public void activateFight(int n)
    {
        if (currentFight >= 0)
        {
            throw new System.Exception("Tried to activate a fight when a previous fight was already activated! Be sure to shut down the earlier fight so that only one fight is active at a time!");
        }
        enemies[n].SetActive(true);
        currentFight = n;
    }

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
        if (currentFight < 0)
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
