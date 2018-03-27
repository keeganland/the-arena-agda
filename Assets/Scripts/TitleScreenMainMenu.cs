using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenMainMenu : MonoBehaviour {

    public void PlayGame()
    {
        SceneManager.LoadScene("Introduction (1)");
    }

    public void PlayCh1()
    {
        SceneManager.LoadScene("Chapter1");
    }

    public void PlayBoss()
    {
        SceneManager.LoadScene("Gameplay - BossBattle");
    }

    public void PlayNPC()
    {
        SceneManager.LoadScene("Dialog - Prototype");
    }

    public void QuitGame ()
    {
        Debug.Log("Game Quit!");
        Application.Quit();
    }

}
