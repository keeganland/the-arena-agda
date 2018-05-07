/* Very lazy creation of a victory screen script. Long term solution is to put a listener of some sort into pause menu
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryScreen : MonoBehaviour {

    public static bool youWon = false;
    public static bool gameIsPaused = false;
    public GameObject victoryMenuUI;

    // Update is called once per frame
    void Update()
    {

        if (youWon)
        {
            Debug.Log("VictoryScreen should have just entered you won conditional");
            Pause();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
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

    public void Resume()
    {
        victoryMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
        youWon = false;
    }

    public void Pause()
    {
        victoryMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
        youWon = false;
    }

    public void ResetGame()
    {
        AnyManager.anyManager.ResetGame();
        this.Resume();
    }

    public void QuitGame()
    {
        Debug.Log("Game Quit!");
        Application.Quit();
    }

}
