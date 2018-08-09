using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class ArenaEntranceDoorMenu : MonoBehaviour {

    public PublicVariableHolderArenaEntrance publicVariableHolderArenaEntrance;

    private GameObject Boy;
    private GameObject Girl;

    private GameObject BoyDoorPos;
    private GameObject GirlDoorPos;

    public static bool gameIsPaused = false;
    public GameObject fightMenuUI;
    private bool doorClicked = false;

    private void Start()
    {
        Boy = publicVariableHolderArenaEntrance.Boy;
        Girl = publicVariableHolderArenaEntrance.Girl;
        GirlDoorPos = publicVariableHolderArenaEntrance.GirlDoorPos;
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
        FindObjectOfType<EnableFight>().EnableNextFight();
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void ArenaEntranceFight1()
    {
        StartCoroutine(Fight1());
    }
    public void ArenaEntranceFight2()
    {
        Debug.Log("here");
        StartCoroutine(Fight2());
    }
    public void ArenaEntranceFight3()
    {
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

        Vector3 DoorGirlPos = new Vector3(GirlDoorPos.transform.position.x, Girl.transform.position.y, GirlDoorPos.transform.position.z);
        Girl.GetComponent<NavMeshAgent>().SetDestination(DoorGirlPos);

        yield return new WaitForSeconds(2f);

        GameObject.FindWithTag("Fader").GetComponent<ScreenFader>().StartCoroutine("FadeOut");

        yield return new WaitForSeconds(2f);

        GameObject.FindWithTag("LoadingScreen").GetComponent<LoadingScreen>().loadScene("Arena", "ArenaEntrance");

        yield return new WaitForSeconds(1f);
    }

    private IEnumerator Fight2()
    {
        SetUp.Instance.SetFightToLoad(1);
        Resume(); //oh boy can't wait for this to cause a race condition somehow

        FindObjectOfType<SaveManager>().currentFight = 1;

        EventManager.TriggerEvent("GirlInCombat");
        EventManager.TriggerEvent("StopMoving");

        Vector3 DoorGirlPos = new Vector3(GirlDoorPos.transform.position.x, Girl.transform.position.y, GirlDoorPos.transform.position.z);
        Girl.GetComponent<NavMeshAgent>().SetDestination(DoorGirlPos);

        yield return new WaitForSeconds(2f);

        GameObject.FindWithTag("Fader").GetComponent<ScreenFader>().StartCoroutine("FadeOut");

        yield return new WaitForSeconds(2f);

        GameObject.FindWithTag("LoadingScreen").GetComponent<LoadingScreen>().loadScene("Arena", "ArenaEntrance");

        yield return new WaitForSeconds(1f);

    }

    private IEnumerator Fight3()
    {
        SetUp.Instance.SetFightToLoad(2);
        Resume(); //oh boy can't wait for this to cause a race condition somehow

        FindObjectOfType<SaveManager>().currentFight = 2;

        EventManager.TriggerEvent("GirlInCombat");
        EventManager.TriggerEvent("StopMoving");

        Vector3 DoorGirlPos = new Vector3(GirlDoorPos.transform.position.x, Girl.transform.position.y, GirlDoorPos.transform.position.z);
        Girl.GetComponent<NavMeshAgent>().SetDestination(DoorGirlPos);

        yield return new WaitForSeconds(2f);

        GameObject.FindWithTag("Fader").GetComponent<ScreenFader>().StartCoroutine("FadeOut");

        yield return new WaitForSeconds(2f);

        GameObject.FindWithTag("LoadingScreen").GetComponent<LoadingScreen>().loadScene("Arena", "ArenaEntrance");

        yield return new WaitForSeconds(1f);

    }
}
