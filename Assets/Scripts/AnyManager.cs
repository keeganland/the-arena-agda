/**
 * Keegan note: This may get obselete quick, but I'm trying out this Additive Scene Loading tutorial
 * https://www.youtube.com/watch?v=i2W5O5qxCuo
 * and copying the stuff involved in that video
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnyManager : MonoBehaviour {

    public static AnyManager anyManager;
    public static string sceneToBeActive;
    public bool normalGameplay;

    private bool gameStart = false;

    private void Awake()
    {

        anyManager = this;
    }


    private void Start()
    {
        //Debug.Log("AnyManager Start function");
        if (!gameStart && normalGameplay || (SceneManager.sceneCount == 1))
        {
            Debug.Log("The game has not started, and we are doing normal gameplay, or else ONLY NeverUnload is loaded. Let's start things up by going to the title screen!");
            SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
            gameStart = true;
        }
    }


    /*
     * Returns the whole game to the title screen.
     */

    public void ResetGame()
    {       
        EventManager.TriggerEvent("cleanup");

        int c = SceneManager.sceneCount;

        for (int i = 0; i < c; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);

            if (scene.name != "NeverUnload")
            {
                SceneManager.UnloadSceneAsync(scene);
            }
        }
        SceneManager.LoadSceneAsync("TitleScreen", LoadSceneMode.Additive);
        GameObject.FindWithTag("Fader").GetComponent<Animator>().Play("ForceFadeIn");
    }

    /*
     * The only function that should ever unload NeverUnload. Hard resets the entire game, as if it were just initialized for the first time ever.
     */
    public void ResetGameHard()
    {
        SceneManager.LoadScene("NeverUnload");
    }

    /*
    //Seems unnecessary with current version of Unity

    public void UnloadScene(int scene)
    {
        StartCoroutine(Unload(scene));
    }

    IEnumerator Unload(int scene)
    {
        yield return null;

        SceneManager.UnloadSceneAsync(scene);
        //Note: video suggested I do this with UnloadScene because of UnloadScene's tendency to cause things to crash
        //But the documentation associated with UnloadScene suggests that UnloadSceneAsync is a "safe" new variant
        //Look into this. Maybe this co-routine is unnecessary.
    }
    */

}
