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
    private bool gameStart = false;

    private void Awake()
    {
        if (!gameStart)
        {
            anyManager = this;

            SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);

            gameStart = true;
        }
    }

    /*
     * Returns the whole game to the title screen.
     */

    public void ResetGame()
    {
        //Ugh. Buggy.

        /*
        Debug.Log("Resetting game!");

        int c = SceneManager.sceneCount;
        Debug.Log("There are " + c + " scenes total");

        //Start at i = 1, because we never want 0 to be unloaded
        for (int i = 0; i < c; i++)
        {
            Debug.Log("For lop iteration " + i);
            if (SceneManager.GetSceneAt(i).name != "NeverUnload")
            {
                Debug.Log("About to unload " + SceneManager.GetSceneAt(i).name);
                SceneManager.UnloadSceneAsync(i);
            }
        }
        SceneManager.LoadSceneAsync("TitleScreen", LoadSceneMode.Additive);

        */
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
