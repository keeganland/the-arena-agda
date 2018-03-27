using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenMainMenu : MonoBehaviour {

    public void PlayGame()
    {
        SceneManager.LoadScene("Introduction (1)");
    }


}
