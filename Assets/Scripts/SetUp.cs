using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetUp : MonoBehaviour {

    private static int fightToLoad;
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
    void Start () {
        EventManager.StartListening("setup", SetUpCharacterHealth);
	}

    private void OnDisable()
    {
        EventManager.StopListening("setup", SetUpCharacterHealth);
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void SetUpCharacterHealth()
    {
        /*TODO reset the players' health here*/
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
