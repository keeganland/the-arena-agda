using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArenaEntranceDoorMenu : MonoBehaviour {
    public static bool gameIsPaused = false;
    public GameObject fightMenuUI;
    private bool doorClicked = false;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (doorClicked)
        {
            doorClicked = false;
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

    public void ArenaEntranceFight1()
    {
        //Temporary until we get Alex's new loading screens working
        SetUp.Instance.SetFightToLoad(0);
        Resume(); //oh boy can't wait for this to cause a race condition somehow
        SceneManager.UnloadSceneAsync("ArenaEntrance");
        SceneManager.LoadSceneAsync("Arena", LoadSceneMode.Additive);

        //GameObject.FindWithTag("LoadingScreen").GetComponent<LoadingScreen>().loadScene("Arena", "TitleScreen"); //keegannote: bugged
    }
    public void ArenaEntranceFight2()
    {
        SetUp.Instance.SetFightToLoad(1);
        Resume();
        SceneManager.UnloadSceneAsync("ArenaEntrance");
        SceneManager.LoadSceneAsync("Arena", LoadSceneMode.Additive);

        //GameObject.FindWithTag("LoadingScreen").GetComponent<LoadingScreen>().loadScene("Arena", "TitleScreen");
    }
    public void ArenaEntranceFight3()
    {
        SetUp.Instance.SetFightToLoad(2);
        Resume();
        SceneManager.UnloadSceneAsync("ArenaEntrance");
        SceneManager.LoadSceneAsync("Arena", LoadSceneMode.Additive);

        //GameObject.FindWithTag("LoadingScreen").GetComponent<LoadingScreen>().loadScene("Arena", "TitleScreen");
    }

    public void DoorClicked()
    {
        doorClicked = true;
    }

}
