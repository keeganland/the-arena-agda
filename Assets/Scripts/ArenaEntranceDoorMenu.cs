 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class ArenaEntranceDoorMenu : MonoBehaviour {

    public PublicVariableHolderArenaEntrance publicVariableHolderArenaEntrance;

    private GameObject boyPlayer;
    private GameObject girlPlayer;

    private GameObject BoyDoorPos;
    private GameObject GirlDoorPos;

    public static bool gameIsPaused = false;
    public GameObject fightMenuUI;
    private bool doorClicked = false;

    public AudioClip SelectFight;
    public float SoundScaleFactor;
    private AudioSource m_audioSource;

    private void Start()
    {
        girlPlayer = GameObject.FindGameObjectWithTag("Player/Girl");
        boyPlayer = GameObject.FindGameObjectWithTag("Player/Boy");
        GirlDoorPos = publicVariableHolderArenaEntrance.GirlDoorPos;

        m_audioSource = GetComponent<AudioSource>();
        SoundManager.onSoundChangedCallback += UpdateSound;
    }
    // Update is called once per frame
    void Update () {
        
        if (doorClicked)
        {
            doorClicked = false;

            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    private void Resume()
    {
        fightMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    private void Pause()
    {
        fightMenuUI.SetActive(true);
        Debug.Log("Alex : Enable Next Fight after return");
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void ArenaEntranceFight1()
    {
        FindObjectOfType<EnableFight>().EnableNextFight(1);
        m_audioSource.PlayOneShot(SelectFight);
        Debug.Log("here");
        StartCoroutine(Fight1());
    }
    public void ArenaEntranceFight2()
    {
        FindObjectOfType<EnableFight>().EnableNextFight(2);
        m_audioSource.PlayOneShot(SelectFight);
        StartCoroutine(Fight2());
    }
    public void ArenaEntranceFight3()
    {
        m_audioSource.PlayOneShot(SelectFight);
        StartCoroutine(Fight3());     
    }

    public void DoorClicked()
    {
        FindObjectOfType<TextBoxManager>().DisableCue();
        FindObjectOfType<TextBoxManager>().DisableTextBox();
        doorClicked = true;
    }

    private IEnumerator Fight1()
    {
        SetUp.Instance.SetFightToLoad(0);
        Resume(); //oh boy can't wait for this to cause a race condition somehow

        FindObjectOfType<SaveManager>().currentFight = 0;

        EventManager.TriggerEvent("GirlInCombat");
        EventManager.TriggerEvent("StopMoving");

        Vector3 DoorGirlPos = new Vector3(GirlDoorPos.transform.position.x, girlPlayer.transform.position.y, GirlDoorPos.transform.position.z);
        girlPlayer.GetComponent<NavMeshAgent>().SetDestination(DoorGirlPos);

        yield return new WaitForSeconds(2f);

        SoundManager.ExitScene = true;
        ScreenFader.fadeOut();

        yield return new WaitForSeconds(2f);

        LoadingScreen.LoadScene("Arena", "ArenaEntrance");

        yield return new WaitForSeconds(1f);
    }

    private IEnumerator Fight2()
    {
        SetUp.Instance.SetFightToLoad(1);
        Resume(); //oh boy can't wait for this to cause a race condition somehow

        FindObjectOfType<SaveManager>().currentFight = 1;

        EventManager.TriggerEvent("GirlInCombat");
        EventManager.TriggerEvent("StopMoving");

        Vector3 DoorGirlPos = new Vector3(GirlDoorPos.transform.position.x, girlPlayer.transform.position.y, GirlDoorPos.transform.position.z);
        girlPlayer.GetComponent<NavMeshAgent>().SetDestination(DoorGirlPos);

        yield return new WaitForSeconds(2f);

        SoundManager.ExitScene = true;
        ScreenFader.fadeOut();

        yield return new WaitForSeconds(2f);

        LoadingScreen.LoadScene("Arena", "ArenaEntrance");

        yield return new WaitForSeconds(1f);

    }

    private IEnumerator Fight3()
    {
        SetUp.Instance.SetFightToLoad(2);
        Resume(); //oh boy can't wait for this to cause a race condition somehow

        FindObjectOfType<SaveManager>().currentFight = 2;

        EventManager.TriggerEvent("GirlInCombat");
        EventManager.TriggerEvent("StopMoving");

        Vector3 DoorGirlPos = new Vector3(GirlDoorPos.transform.position.x, girlPlayer.transform.position.y, GirlDoorPos.transform.position.z);
        girlPlayer.GetComponent<NavMeshAgent>().SetDestination(DoorGirlPos);

        yield return new WaitForSeconds(2f);

        ScreenFader.fadeOut();
        SoundManager.ExitScene = true;

        yield return new WaitForSeconds(2f);

        LoadingScreen.LoadScene("Arena", "ArenaEntrance");

        yield return new WaitForSeconds(1f);
    }

    void UpdateSound()
    {
        m_audioSource.volume = (SoundManager.SFXVolume * SoundScaleFactor) / 100;
    }
}
