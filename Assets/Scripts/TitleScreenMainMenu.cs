using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenMainMenu : MonoBehaviour {
    
    private void Start()
    {
        GameObject.Find("PlayerUI").SetActive(false);
    }

    public void PlayGame()
    {
        //SceneManager.LoadScene("Introduction (1)");
        /**
         * Holy shit the stuff below just works painlessly
         */
        SceneManager.UnloadSceneAsync("TitleScreen");
        SceneManager.LoadSceneAsync("Introduction (1)", LoadSceneMode.Additive);
        EventManager.TriggerEvent("setup");
    }

    public void PlayArena()
    {
        SceneManager.UnloadSceneAsync("TitleScreen");
        SceneManager.LoadSceneAsync("Arena", LoadSceneMode.Additive);
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
        SceneManager.UnloadSceneAsync("TitleScreen");
        SceneManager.LoadSceneAsync("HowToPlay", LoadSceneMode.Additive);
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
