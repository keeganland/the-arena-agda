using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenMainMenu : MonoBehaviour {

    public GameObject tutorialScreen;

    [SerializeField] private AudioClip MouseConfirm;
    [SerializeField] private float ScaleFactor;

    private AudioSource m_audioSource;

    private void Awake()
    {
        EventManager.TriggerEvent("HideUI");
        if (this.gameObject.name != "Pause Canvas")
            EventManager.TriggerEvent("HiddeUIButtons");
        MouseConfirm = Resources.Load("SFX/ui_menu_button_confirm_15") as AudioClip;
        m_audioSource = GetComponent<AudioSource>();
        SoundManager.onSoundChangedCallback += UpdateSound;
    }

    private void OnDisable()
    {
        if (tutorialScreen)
        {
            if (tutorialScreen.activeSelf == false)
                EventManager.TriggerEvent("ShowUI");
        }

        SoundManager.onSoundChangedCallback -= UpdateSound;
    }

    public void PlayGame()
    {
        m_audioSource.PlayOneShot(MouseConfirm);
        LoadingScreen.LoadScene("Introduction (1)", "TitleScreen");
        EventManager.TriggerEvent("setup");
    }

    public void PlayArenaEntrance()
    {
        m_audioSource.PlayOneShot(MouseConfirm);
        LoadingScreen.LoadScene("ArenaEntrance", "TitleScreen");
        EventManager.TriggerEvent("setup");
    }

    public void PlayArena()
    {
        m_audioSource.PlayOneShot(MouseConfirm);
        LoadingScreen.LoadScene("Arena", "TitleScreen");
        EventManager.TriggerEvent("setup");
    }

    public void PlayBoss()
    {
        m_audioSource.PlayOneShot(MouseConfirm);
        //SceneManager.LoadScene("Gameplay - BossBattle");
        SceneManager.UnloadSceneAsync("TitleScreen");
        SceneManager.LoadSceneAsync("Gameplay - BossBattle", LoadSceneMode.Additive);
        EventManager.TriggerEvent("setup");
    }

    public void PlayNPC()
    {
        m_audioSource.PlayOneShot(MouseConfirm);
        //SceneManager.LoadScene("Dialog - Prototype");
        SceneManager.UnloadSceneAsync("TitleScreen");
        //SceneManager.UnloadSceneAsync("NeverUnload");
        SceneManager.LoadSceneAsync("Dialog - Prototype", LoadSceneMode.Additive);
        EventManager.TriggerEvent("setup");
    }

    public void PlayHowToPlay()
    {
        m_audioSource.PlayOneShot(MouseConfirm);
        if (tutorialScreen)
        tutorialScreen.SetActive(true);
    }

    public void QuitGame ()
    {
        m_audioSource.PlayOneShot(MouseConfirm);
        Debug.Log("Game Quit!");
        Application.Quit();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.collider.name);
            }
        }   
    }

    void UpdateSound()
    {
        m_audioSource.volume = (SoundManager.SFXVolume * ScaleFactor) / 100;
    }
}
