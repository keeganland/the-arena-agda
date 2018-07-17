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
    public bool victoryDebugging;
    public int victoryMode;

    private void Awake()
    {
        bossesKilled = 0;
        victoryAction = new UnityAction(Victory);
        victoryCondition = new UnityAction(NeverVictory); //Just to avoid null references. victoryCondition will be a very frequently changing variable
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
        if (victoryDebugging)
        {
            if (Input.GetKeyDown("v"))
            {
                this.SetVictoryCondition(1); //Whenever a checkVictory happens, you automatically win.
                Debug.Log("You set the victory condition to 1");
            }
            if (Input.GetKeyDown("b"))
            {
                Debug.Log("Time to trigger a checkVictory event");
                EventManager.TriggerEvent("checkVictory"); //Make a checkVictory happen
            }       
        }

        //In theory, we could do a check victory event every single frame.
        //In practice, that would probably bog things down.
        //I'm going to put check victory events with the death of enemies in HealthController.cs, instead of here
        //The same should be done for every possible event that could possibly, though not necessarily, trigger a victory
        //Don't uncomment the line below. It exists just to remind Keegan what an "obsessive" victory checker would look like
        //EventManager.TriggerEvent("checkVictory");
        if (playerWon)
        {
            //Debug.Log("Player has won!");
            EventManager.TriggerEvent("victory");
        }
    }

    public void Victory()
    {
        bossesKilled = 0;

        /*
         * Keegan note to Alex 2018/6/10:
         * You may want to comment out the two lines below. All they do is pop up the old victory menu, then pause the game.
         * Triggers for things to start happening in ScriptedEvents are to be called in this function.
         */
        //victoryUI.SetActive(true);
        // Time.timeScale = 0f;
        EventManager.TriggerEvent("victoryEvent"); /*Alex note to Keegan 2018/6/12:
                                                    * The "victory" event will be different for each "scene" later in the game,
                                                    * I think we'll have to create specific events (with the same keyword).
                                                    */


        /*
         * Keegan note, 2018/7/1
         * 
         * Possibly desirable to put an EventManager.StopListening("victoryEvent", whatever_the_relevant_delegate_is_called)
         * to accomplish what alex is talking about in the above note if not here than in something triggered by the 
         */
    }

    /* Keegan Note 2018/6/10:
     * Presently used for the buttons on the victory screen
     */
    public void ResetGame()
    {
        victoryUI.SetActive(false);
        Time.timeScale = 1f;
        AnyManager.anyManager.ResetGame();
    }

    private void BossDied()
    {
        //Debug.Log("A boss was killed!");
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
        //First, we clean out any old victory conditions that happened to exist. We only want one in the dictory at once
        EventManager.StopListening("checkVictory", victoryCondition);

        switch(n)
        {
            /* Keegan NTS: 
             * Extensible for creation of alternative victory conditions
             * 
             * To create a new victory condition, the idea is to create a new private function with no args that calls SetPlayerWon
             * This function will contain the necessary boolean logic for whether the player has satisfied the conditions for victory.
             * This switch case will associate the variable victoryCondition with a delegate to a particular function.
             * 
             * Keep in mind that victory conditions are just elaborate setters and can't trigger a victory on their own.
             * The game only checks for victory when a "checkVictory" event is called - which, in its current implementation,
             * happens only when you kill an enemy or when using the victory debug mode.
             *
             * Therefore, even AutoVictory will require you to kill an enemy in most circumstances.
             * 
             */
            case 0:
                victoryCondition = new UnityAction(KillOneBossVictory);
                break;
            case 1:
                victoryCondition = new UnityAction(AutoVictory); //for debugging
                break;
            default:
                victoryCondition = new UnityAction(KillOneBossVictory);
                throw new System.Exception("You tried to set an invalid victory condition! Game set to KillOneBossVictory by default");
        }
        EventManager.StartListening("checkVictory", victoryCondition);
    }

    private void KillOneBossVictory()
    {
     //   Debug.Log("Entered KillOneBossVictory");
        SetPlayerWon(bossesKilled > 0);
    }

    private void AutoVictory()
    {
      //  Debug.Log("Entered AutoVictory");
        SetPlayerWon(true);
    }

    private void NeverVictory()
    {
       // Debug.Log("Entered NeverVictory");
        SetPlayerWon(false);
    }
}
