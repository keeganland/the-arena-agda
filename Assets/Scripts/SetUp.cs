using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetUp : MonoBehaviour {

    private static SetUp setUp;
    [SerializeField]
    private int fightToLoad;

    public GameObject[] PlayersGameObject;

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
        EventManager.StartListening("setup", SetUpCharacterHealth);
	}

    private void OnDisable()
    {
        EventManager.StopListening("setup", SetUpCharacterHealth);
    }

    public void SetUpCharacterHealth()
    {
        /*TODO reset the players' health here*/
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
