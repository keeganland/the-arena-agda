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
