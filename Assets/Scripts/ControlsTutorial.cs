using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlsTutorial : MonoBehaviour {

    public PublicVariableHolderneverUnload publicVariableHolderneverUnload;
    private LoadingScreen loadingScreen;

    private void Awake()
    {
        publicVariableHolderneverUnload = GameObject.Find("PublicVariableHolderNeverUnload").GetComponent<PublicVariableHolderneverUnload>();
    }

    // Use this for initialization
    void Start () {
        publicVariableHolderneverUnload.PlayerUI.SetActive(false);
        loadingScreen = publicVariableHolderneverUnload.loadingScreenControler;
	}
    private void OnDisable()
    {
        publicVariableHolderneverUnload.PlayerUI.SetActive(true);
    }

    public void Back()
    {
        loadingScreen.loadScene("TitleScreen", "HowToPlay");
        //SceneManager.UnloadSceneAsync("HowToPlay");
        //SceneManager.LoadSceneAsync("TitleScreen", LoadSceneMode.Additive);
        EventManager.TriggerEvent("setup");
    }
}
