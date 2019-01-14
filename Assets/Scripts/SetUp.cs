using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Alex Regactor : 01/13
//TODO:Keegan can you valid the changes for the SetUp script Refactoring?

public class SetUp : MonoBehaviour {

    //Config
    [SerializeField] private int fightToLoad;

    //Cached component  references
    [SerializeField] private GameObject[] PlayersGameObject;
    private static SetUp setUp;


    public static SetUp Instance
    {
        get
        {
            if (!setUp)
            {
                setUp = FindObjectOfType(typeof(SetUp)) as SetUp;

                if (!setUp)
                {
                    Debug.LogError("There needs to be one active SetUp script on a GameObject in your scene.");
                }
                else
                {
                    setUp.Init();
                }
            }
            return setUp;
        }
    }

    void Init()
    {
        fightToLoad = 0;
    }

    // Use this for initialization
    private void OnEnable () 
    {
        PlayersGameObject = GameObject.FindGameObjectsWithTag("Player");
        EventManager.StartListening("setup", SetUpCharacterHealth);
	}

    private void OnDisable()
    {
        EventManager.StopListening("setup", SetUpCharacterHealth);
    }

    public void SetUpCharacterHealth()
    {
        foreach (GameObject players in PlayersGameObject)
        {
            players.GetComponent<HealthController>().currentHealth = players.GetComponent<HealthController>().totalHealth;
            players.GetComponent<HealthController>().UndoDeath();
        }

        VictoryReferee.SetPlayerWon(false);

        EventManager.TriggerEvent("refreshUI");
        EventManager.TriggerEvent("ResetTargets");
    }

    public int GetFightToLoad()
    {
        return fightToLoad;
    }

    public void SetFightToLoad(int n)
    {
        fightToLoad = n;
    }
}
