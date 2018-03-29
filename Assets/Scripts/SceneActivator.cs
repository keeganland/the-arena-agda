/**
 * Okay, this is a hacky way I have figured out to set the active scene correctly.
 * 
 * doing this "properly" is difficult. Should probably flag as a longer-term project
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneActivator : MonoBehaviour {

    public string SceneToBeActive;
	
	// Update is called once per frame
	void Update () {

        if (SceneManager.GetActiveScene().name != SceneToBeActive)
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(SceneToBeActive));
        }
    }
}
