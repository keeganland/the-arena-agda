using System.Collections;
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

    private static LoadingScreen loadingScreen;

    public static LoadingScreen Instance
    {
        get
        {
            if (!loadingScreen)
            {
                loadingScreen = FindObjectOfType(typeof(LoadingScreen)) as LoadingScreen;

                if (!loadingScreen)
                {
                    Debug.LogError("There needs to be one active EventManger script on a GameObject in your scene.");
                }
                else
                {
                    loadingScreen.Init();
                }
            }
            return loadingScreen;
        }
    }

    void Init()
    {
        loadingScreen = this;
        slider = publicVariableHolderneverUnload.loadingScreenSlider;
        loadingScreenObj = publicVariableHolderneverUnload.loadingScreen;
        fader = publicVariableHolderneverUnload.fader;
    }

    private void Awake()
    {
        loadingScreen = this;
        slider = publicVariableHolderneverUnload.loadingScreenSlider;
        loadingScreenObj = publicVariableHolderneverUnload.loadingScreen;
        fader = publicVariableHolderneverUnload.fader;
    }

    public static void LoadScene(string sceneToLoad, string sceneToUnload)
    {
        loadingScreen.StartCoroutine(loadingScreen.LoadSceneCoroutine(sceneToLoad, sceneToUnload));
    }

    private IEnumerator LoadSceneCoroutine(string sceneToLoad, string sceneToUnload)
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
    }
}
