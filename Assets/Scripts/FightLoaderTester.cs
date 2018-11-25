/**
 * Keegan Note 2018/8/3 - This is going to turn into some wild bullshit very quickly.
 * I'm redesigning it to load into different setups of the Arena scene FROM the ArenaEntrance.
 * 
 * Keep in mind that this whole script was never intended to be permanent.
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class FightLoaderTester : MonoBehaviour {

    public FightTracker ft;
    public static bool gameIsPaused = false;
    public GameObject fightMenuUI;

    //public bool testSpecificFight;
    public int specificFightToTest = 0;

    private void Start()
    {
        specificFightToTest = SetUp.Instance.GetFightToLoad();

        //if(testSpecificFight)
        //{
        switch (specificFightToTest)
            {
                case 0:
                    Fight1();
                    break;
                case 1:
                    Fight2();
                    break;
                case 2:
                    Fight3();
                    break;
                default:
                    Fight1();
                    break;
            }
        //}
    }

    // Update is called once per frame
    void Update () {

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

    /*
     * Keegan NTS 2018/7/1:
     * wanted to use deactivateCurrentFight instead, but since i'm not sure what fight initialization will look like in the relatively final product,
     * I'm doing deactivateAllFights instead. This is, hypothetically speaking, less efficient - but harmless since we only have a search space of 3-5 things.
     * 
     * Still, it might just be good hygeine to replace the deactivateAllFights, once we transplant the needed code into a more "final" menu.
     */
    public void Fight1()
    {
        try
        {
            ft.deactivateAllFights();
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);   
        }
        finally
        {
            EventManager.TriggerEvent("cleanup");
            ft.activateFight(0);
            Resume();
            EventManager.TriggerEvent("enterArenaFight1");
        }
    }

    public void Fight2()
    {
        try
        {
            ft.deactivateAllFights();
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }
        finally
        {   
            EventManager.TriggerEvent("cleanup");
            ft.activateFight(1);
            Resume();
            EventManager.TriggerEvent("enterArenaFight2");
        }
    }

    public void Fight3()
    {
        try
        {
            ft.deactivateAllFights();
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }
        finally
        {
            EventManager.TriggerEvent("cleanup");
            ft.activateFight(2);
            Resume();
            EventManager.TriggerEvent("enterArenaFight3");
        }
    }
}
