using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*Alex : I inspired the code from https://www.youtube.com/watch?v=rXnZE8MwK-E;
*/

/*Alex : Refactored 01/13
 * 
 * How to use the script: 
 *
 * Call LoadSceneManager.Load("Name of the Scene to load", "Name of the scene to unload / OR "" if no scene to unload"); from any other function when you want to change scene
 *
 * The Coroutine LoadScene takes two inputs as stings, the name of the scene to load and the name of the scene to unload. If there is no scene to unload, then input "".
 * It will(in order) : Create a loading screen, load the scene and unload the scene, remove the loading screen and trigger the "SetUpScene" event. 
 * 
 * For exemple, I use LoadSceneManager.LoadScene("Level1", ""); from the script anymanager to load level1 on the intro (will be changed to TitleScreen once we have one).
 *              If I want to change Scene from level1 to level2 I would use LoadSceneManager.LoadScene("Level2", "Level1");
 * 
 * COMMENT: We could also use the int related to the scene in the build option (NeverUnload = 0, Level1 = 1) but I think using the names of scene is more appropriate.
*/
public class LoadingScreen : MonoBehaviour
{
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
    }

    private void Awake()
    {
        loadingScreen = this;
    }

    public static void LoadScene(string sceneToLoad)
    {
        loadingScreen.StartCoroutine(loadingScreen.LoadSceneCoroutine(sceneToLoad));
    }
    public static void LoadScene(string sceneToLoad, string sceneToUnload)
    {
        loadingScreen.StartCoroutine(loadingScreen.LoadSceneCoroutine(sceneToLoad, sceneToUnload));
    }

    private IEnumerator LoadSceneCoroutine(string sceneToLoad)
    {
        var loadingScreen = Instantiate(Resources.Load("Prefabs/LoadingScreen")) as GameObject;
        var slider = loadingScreen.GetComponentInChildren<Slider>();

        AsyncOperation async = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
        async.allowSceneActivation = false;

        while (async.isDone == false)
        {
            slider.value = async.progress;
            if (async.progress >= 0.9f)
            {
                slider.value = 1.0f;
                async.allowSceneActivation = true;
            }
            yield return null;
        }

        Destroy(loadingScreen);
        EventManager.TriggerEvent("SetUpScene");
    }
    private IEnumerator LoadSceneCoroutine(string sceneToLoad, string sceneToUnload)
    {
        var loadingScreen = Instantiate(Resources.Load("Prefabs/LoadingScreen")) as GameObject;
        var slider = loadingScreen.GetComponentInChildren<Slider>();

        if (sceneToUnload != "")
            SceneManager.UnloadSceneAsync(sceneToUnload);

        AsyncOperation async = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
        async.allowSceneActivation = false;

        while (async.isDone == false)
        {
            slider.value = async.progress;
            if (async.progress >= 0.9f)
            {
                slider.value = 1.0f;
                async.allowSceneActivation = true;
            }
            yield return null;
        }

        Destroy(loadingScreen);
        EventManager.TriggerEvent("SetUpScene");
    }
}