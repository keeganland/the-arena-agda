﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenMainMenu : MonoBehaviour {

    public void PlayGame()
    {
        //SceneManager.LoadScene("Introduction (1)");
        /**
         * Holy shit the stuff below just works painlessly
         */
        SceneManager.UnloadSceneAsync("TitleScreen");
        SceneManager.LoadSceneAsync("Introduction (1)", LoadSceneMode.Additive);
    }

    public void PlayCh1()
    {
        SceneManager.LoadScene("Chapter1");
        /*
        SceneManager.UnloadSceneAsync("TitleScreen");
        SceneManager.LoadSceneAsync("Chapter1", LoadSceneMode.Additive);
        */
    }

    public void PlayBoss()
    {
        //SceneManager.LoadScene("Gameplay - BossBattle");
        SceneManager.UnloadSceneAsync("TitleScreen");
        SceneManager.LoadSceneAsync("Gameplay - BossBattle", LoadSceneMode.Additive);

    }

    public void PlayNPC()
    {
        //SceneManager.LoadScene("Dialog - Prototype");
        SceneManager.UnloadSceneAsync("TitleScreen");
        //SceneManager.UnloadSceneAsync("NeverUnload");
        SceneManager.LoadSceneAsync("Dialog - Prototype", LoadSceneMode.Single);
        
    }

    public void QuitGame ()
    {
        Debug.Log("Game Quit!");
        Application.Quit();
    }

}
