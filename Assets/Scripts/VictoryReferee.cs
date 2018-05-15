using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryReferee : MonoBehaviour {

    private bool youWon = false;
    public GameObject victoryUI;
    public static bool gameIsPaused = false;
    public VictoryCondition vc;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (vc.getYouWon())
        {
            Pause();
        }
    }

    /*
    private void Resume()
    {
        victoryUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }*/

    private void Pause()
    {
        victoryUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }
}
