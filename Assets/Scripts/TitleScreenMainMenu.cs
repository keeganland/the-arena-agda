using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenMainMenu : MonoBehaviour {
    

    private void Awake()
    {
        EventManager.TriggerEvent("HideUI");
    }

    private void OnDisable()
    {
            EventManager.TriggerEvent("ShowUI");
    }

    public void PlayGame()
    {
        //SceneManager.LoadScene("Introduction (1)");
        /**
         * Holy shit the stuff below just works painlessly
         */

        GameObject.FindWithTag("LoadingScreen").GetComponent<LoadingScreen>().loadScene("Introduction (1)", "TitleScreen");
        EventManager.TriggerEvent("setup");
    }

    public void PlayArena()
    {
        GameObject.FindWithTag("LoadingScreen").GetComponent<LoadingScreen>().loadScene("Arena", "TitleScreen");
        EventManager.TriggerEvent("setup");
    }

    public void PlayBoss()
    {
        //SceneManager.LoadScene("Gameplay - BossBattle");
        SceneManager.UnloadSceneAsync("TitleScreen");
        SceneManager.LoadSceneAsync("Gameplay - BossBattle", LoadSceneMode.Additive);
        EventManager.TriggerEvent("setup");
    }

    public void PlayNPC()
    {
        //SceneManager.LoadScene("Dialog - Prototype");
        SceneManager.UnloadSceneAsync("TitleScreen");
        //SceneManager.UnloadSceneAsync("NeverUnload");
        SceneManager.LoadSceneAsync("Dialog - Prototype", LoadSceneMode.Additive);
        EventManager.TriggerEvent("setup");
    }
    public void PlayHowToPlay()
    {
        GameObject.FindWithTag("LoadingScreen").GetComponent<LoadingScreen>().loadScene("HowToPlay", "TitleScreen");
        EventManager.TriggerEvent("setup");
    }

    public void QuitGame ()
    {
        Debug.Log("Game Quit!");
        Application.Quit();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.collider.name);
            }
        }   
    }
}
