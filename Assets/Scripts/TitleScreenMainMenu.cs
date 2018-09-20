using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenMainMenu : MonoBehaviour {

    public GameObject tutorialScreen;

    private void Awake()
    {
        EventManager.TriggerEvent("HideUI");
    }

    private void Start()
    {
        ScreenFader.fadeIn();
    }
    private void OnDisable()
    {
        if(tutorialScreen.activeSelf == false)
        EventManager.TriggerEvent("ShowUI");
    }

    public void PlayGame()
    {
        LoadingScreen.LoadScene("Introduction (1)", "TitleScreen");
        EventManager.TriggerEvent("setup");
    }

    public void PlayArenaEntrance()
    {
        LoadingScreen.LoadScene("ArenaEntrance", "TitleScreen");
        EventManager.TriggerEvent("setup");
    }

    public void PlayArena()
    {
        LoadingScreen.LoadScene("Arena", "TitleScreen");
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
        tutorialScreen.SetActive(true);
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
