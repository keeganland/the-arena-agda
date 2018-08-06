using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlsTutorial : MonoBehaviour {

    public GameObject TitleScreen;

    private void OnEnable()
    {
        TitleScreen.SetActive(false);
    }
    public void Back()
    {
        TitleScreen.SetActive(true);
        gameObject.SetActive(false);
    }
}
