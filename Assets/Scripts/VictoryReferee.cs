using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VictoryReferee : MonoBehaviour {


    private UnityAction victoryAction;
    private UnityAction victoryCondition;
    private UnityAction bossDeathAction;
    private int bossesKilled;
    private bool playerWon;

    public GameObject victoryUI;

    private void Awake()
    {
        bossesKilled = 0;
        victoryAction = new UnityAction(Victory);
        bossDeathAction = new UnityAction(BossDied);
    }
    private void OnEnable()
    {
        EventManager.StartListening("victory", victoryAction);
        EventManager.StartListening("bossDied", bossDeathAction);
    }
    private void OnDisable()
    {
        EventManager.StopListening("victory", victoryAction);
        EventManager.StopListening("bossDied", bossDeathAction);
        EventManager.StopListening("checkVictory", victoryCondition);
    }

    private void Start()
    {
        //At the beginning of the scene, victory condition is set to 0 - i.e., "kill one boss"
        //This is so that there is ALWAYS a victory condition. 
        this.SetVictoryCondition(0);
    }

    private void Update()
    {
        //In theory, we could do a check victory event every single frame.
        //In practice, that would probably bog things down.
        //I'm going to put check victory events with the death of enemies in HealthController.cs, instead of here
        //The same should be done for every possible event that could possibly, though not necessarily, trigger a victory
        //Don't uncomment the line below. It exists just to remind Keegan what an "obsessive" victory checker would look like
        //EventManager.TriggerEvent("checkVictory");
        if (playerWon)
        {
            EventManager.TriggerEvent("victory");
        }
    }

    public void Victory()
    {
        bossesKilled = 0;
        victoryUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResetGame()
    {
        victoryUI.SetActive(false);
        Time.timeScale = 1f;
        AnyManager.anyManager.ResetGame();
    }

    private void BossDied()
    {
        Debug.Log("A boss was killed!");
        bossesKilled++;
    }

    public bool GetPlayerWon()
    {
        return playerWon;
    }

    public void SetPlayerWon(bool n)
    {
        playerWon = n;
    }

    public void SetVictoryCondition(int n)
    {
        switch(n)
        {
            /* Keegan NTS: 
             * Extensible for creation of alternative victory conditions
             * 
             * To create a new victory condition, the idea is to create a new private function
             * This 
             */
            case 0:
                victoryCondition = new UnityAction(KillOneBossVictory);
                break;
            default:
                victoryCondition = new UnityAction(KillOneBossVictory);
                throw new System.Exception("You tried to set an invalid victory condition! Game set to KillOneBossVictory by default");
        }

        EventManager.StartListening("checkVictory", victoryCondition);
    }

    private void KillOneBossVictory()
    {
        SetPlayerWon(bossesKilled > 0);
    }
}
