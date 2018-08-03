using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlsTutorial : MonoBehaviour {
    
    // Use this for initialization
    void Start () {
        if(GameObject.Find("PlayerUI").activeSelf == true)
        GameObject.Find("PlayerUI").SetActive(false);
	}

    public void Back()
    {
        SceneManager.UnloadSceneAsync("HowToPlay");
        SceneManager.LoadSceneAsync("TitleScreen", LoadSceneMode.Additive);
        EventManager.TriggerEvent("setup");
    }
}
