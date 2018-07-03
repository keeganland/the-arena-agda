using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightLoaderTester : MonoBehaviour {

    public FightTracker ft;
    public static bool gameIsPaused = false;
    public GameObject fightMenuUI;

    public bool testSpecificFight;
    public int specificFightToTest = 0;

    private void Start()
    {
        if(testSpecificFight)
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
            EventManager.TriggerEvent("enterArena");
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
        }
    }
}
