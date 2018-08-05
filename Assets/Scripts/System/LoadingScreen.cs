﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*Alex : I inspired the code from https://www.youtube.com/watch?v=rXnZE8MwK-E;
*/


public class LoadingScreen : MonoBehaviour {

    public PublicVariableHolderneverUnload publicVariableHolderneverUnload;

    private Slider slider;
    private GameObject loadingScreenObj;
    private ScreenFader fader;

    AsyncOperation async;

    private void Start()
    {
        slider = publicVariableHolderneverUnload.loadingScreenSlider;
        loadingScreenObj = publicVariableHolderneverUnload.loadingScreen;
        fader = publicVariableHolderneverUnload.fader;
    }

    public void loadScene(string sceneToLoad, string sceneToUnload)
    {
        StartCoroutine(loadSceneCoroutine(sceneToLoad, sceneToUnload));
    }

    private IEnumerator loadSceneCoroutine(string sceneToLoad, string sceneToUnload)
    {
        loadingScreenObj.SetActive(true);
        SceneManager.UnloadSceneAsync(sceneToUnload);
        async = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
        async.allowSceneActivation = false;

        while(async.isDone == false)
        {
            slider.value = async.progress;
            if (async.progress >= 0.9f)
            {
                slider.value = 1.0f;
                async.allowSceneActivation = true;
            }
            yield return null;
        }
        loadingScreenObj.SetActive(false);
        fader.StartCoroutine("FadeIn");
    }
}
