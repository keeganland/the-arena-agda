/*
 * Click play --> arena entrance scene
 * Click door, select battle (or just go to first)
 * Return to the entrance when battle is done.
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VictoryReferee : MonoBehaviour {

    public PublicVariableHolderneverUnload publicVariableHoldernever;

    private UnityAction victoryAction;
    private UnityAction victoryCondition;
    private UnityAction bossDeathAction;
    private int bossesKilled;
    private bool playerWon;

    [SerializeField] private List<GameObject> EnemiesLeft;
    [SerializeField] private bool TriggerKillAllbool = false;

    public GameObject victoryUI;
    public bool victoryDebugging;
    public int victoryMode;

    [Header("Lose Condition")]
    public HealthController Boy;
    public HealthController Girl;

    private static VictoryReferee victoryReferee;

    public static VictoryReferee Instance
    {
        get
        {
            if (!victoryReferee)
            {
                victoryReferee = FindObjectOfType(typeof(VictoryReferee)) as VictoryReferee;

                if (!victoryReferee)
                {
                    Debug.LogError("There needs to be one active EventManger script on a GameObject in your scene.");
                }
                else
                {
                    victoryReferee.Init();
                }
            }
            return victoryReferee;
        }
    }

    void Init()
    {
    }

    private void Awake()
    {
        victoryReferee = this;
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
        SetVictoryCondition(0);
    }

    private void Update()
    {
        if (victoryDebugging)
        {
            if (Input.GetKeyDown("v"))
            {
                SetVictoryCondition(1); //Whenever a checkVictory happens, you automatically win.
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

        if(Boy.currentHealth <= 0 && Girl.currentHealth <=0)
        {
            StartCoroutine(Lost());
        }

        KillAllVictory(); //Check for kill all victory ?
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
        EventManager.TriggerEvent("resetPlayer"); //Alex : Reset Player is in HealthControler.cs... IT DOESN'T WORK ! //Alex : Why did I write that ? need to be investigated! Lol.
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

    public static void SetPlayerWon(bool n)
    {
        victoryReferee.playerWon = n;
    }

    public static void SetVictoryCondition(int n)
    {
        //First, we clean out any old victory conditions that happened to exist. We only want one in the dictory at once
        EventManager.StopListening("checkVictory", victoryReferee.victoryCondition);

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
                victoryReferee.victoryCondition = new UnityAction(victoryReferee.KillOneBossVictory);
                victoryReferee.victoryMode = 0;
                break;
            case 1:
                victoryReferee.victoryCondition = new UnityAction(victoryReferee.AutoVictory); //for debugging
                victoryReferee.victoryMode = 1;

                break;
            case 2:
                victoryReferee.victoryCondition = new UnityAction(victoryReferee.KillAllVictory);
                victoryReferee.victoryMode = 2;

                break;
            default:
                victoryReferee.victoryCondition = new UnityAction(victoryReferee.KillOneBossVictory);
                throw new System.Exception("You tried to set an invalid victory condition! Game set to KillOneBossVictory by default");
        }
        EventManager.StartListening("checkVictory", victoryReferee.victoryCondition);
    }

    /* Alex: I'm trying to find a good data structure but I don't see anything else than an array or list? Stacks or queues seems to linear. The only goal is to store a gameobject and check if a container contains it? 
     * 
     * I'll do a list for now then we can change if it is better: 
     * 
     *  - I create a list where I'll store all Gameobjects that needs to be killed for the victory condition,
     *  - Start triggering the victory contidion once the list is not empty anymore
     *  
     *  What does this allows us to do? 
     *  
     * - Switch the victory case at the beginning of the level, 
     * - Wait until the List starts to be populated,
     * - Trigger the event when the list is empty. 
     * 
     * Thank to this, we can have enemies that does NOT trigger the victory event, and enemies that does. 
     * 
     * PS :If you find a better way to do so, please go for it. 
     * 
     */

    private void KillOneBossVictory()
    {
        Debug.Log("Entered KillOneBossVictory");
        SetPlayerWon(bossesKilled > 0);
    }

    private void KillAllVictory()
    {
        if (EnemiesLeft.Count <= 0 && TriggerKillAllbool)
        {
            Debug.Log("here, Victory is true !");
            TriggerKillAllbool = false;
            SetPlayerWon(true);
        }
    }

    public static void AddEnemiesCount(GameObject enemy)
    {
        victoryReferee.EnemiesLeft.Add(enemy);
        victoryReferee.TriggerKillAllbool = true;
    }

    public static void RemoveEnemy(GameObject enemy)
    {
        victoryReferee.EnemiesLeft.Remove(enemy);
    }

    public static void ResetEnemyList()
    {
        victoryReferee.EnemiesLeft.Clear();
        victoryReferee.TriggerKillAllbool = false;
    }

    private void AutoVictory()
    {
        Debug.Log("Entered AutoVictory");
        SetPlayerWon(true);
    }

    private void NeverVictory()
    {
        Debug.Log("Entered NeverVictory");
        SetPlayerWon(false);
    }

    private IEnumerator Lost()
    {
        publicVariableHoldernever.fader.StartCoroutine("FadeOut");
        EventManager.TriggerEvent("HideUI");
        yield return new WaitForSeconds(1f);
        publicVariableHoldernever.DeathCanvas.SetActive(true);
    }
}
