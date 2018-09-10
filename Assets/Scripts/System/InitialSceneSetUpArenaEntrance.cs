using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitialSceneSetUpArenaEntrance : InitialSceneSetup {

    private GameObject SpawnPosBoy;
    private GameObject SpawnPosGirl;

    private GameObject ReturnFromArenaGirlPos;
    private GameObject ReturnFromArenaBoyPos;

    public int[] moneyGainedperFight;

    public Text moneyWon;
    public GameObject moneyTextGameObject;

    new void Start()
    {
        base.Start();

        ReturnFromArenaBoyPos = publicArenaEntrance.ReturnFromArenaBoyPos;
        ReturnFromArenaGirlPos = publicArenaEntrance.ReturnFromArenaGirlPos;

        if (ReturnFromArenaGirlPos != null && ReturnFromArenaBoyPos != null && saveManager.returnFromArena)
        {
            SpawnPosBoy = publicArenaEntrance.ReturnFromArenaBoyPos;
            SpawnPosGirl = publicArenaEntrance.ReturnFromArenaGirlPos;
        }
        else
        {
            SpawnPosBoy = publicArenaEntrance.SpawnPosBoy;
            SpawnPosGirl = publicArenaEntrance.SpawnPosGirl;
        }

        Boy.transform.position = new Vector3(SpawnPosBoy.transform.position.x, transform.position.y, SpawnPosBoy.transform.position.z);
        Girl.transform.position = new Vector3(SpawnPosGirl.transform.position.x, transform.position.y, SpawnPosGirl.transform.position.z);

        BoyNav.enabled = true;
        GirlNav.enabled = true;

        Boy.GetComponent<BetterPlayer_Movement>().isCombat = false;
        Girl.GetComponent<BetterPlayer_Movement>().isCombat = false;
        Girl.GetComponent<BoxCollider>().enabled = false;

        MainCamera = publicArenaEntrance.MainCamera;

        MainCamera.GetComponent<BetterCameraFollow>()._FieldOfViewMin = MainCameraFieldOfViewMin;
        MainCamera.GetComponent<BetterCameraFollow>()._FieldOfViewMax = MainCameraFieldOfViewMax;

        EventManager.TriggerEvent("setup");

        publicArenaEntrance.publicVariableHolderNeverUnload.PlayerUI.SetActive(false);

        Girl.GetComponent<BetterPlayer_Movement>().boyActive = false;
        Boy.GetComponent<BetterPlayer_Movement>().boyActive = false;
        publicArenaEntrance.publicVariableHolderNeverUnload.fader.StartCoroutine("FadeIn");

        if (publicArenaEntrance.ReturnFromArena && saveManager.returnFromArena)
            StartCoroutine(ArenaArrival());

    }

    private IEnumerator ArenaArrival()
    {
        GirlNav.SetDestination(new Vector3(publicArenaEntrance.ReturnFromArena.transform.position.x, Girl.transform.position.y, publicArenaEntrance.ReturnFromArena.transform.position.z));

        yield return new WaitForSeconds(2f);

        InventoryManager.AddMoney(moneyGainedperFight[FindObjectOfType<SaveManager>().currentFight]);
        moneyWon.text = "You won : $" + moneyGainedperFight[FindObjectOfType<SaveManager>().currentFight].ToString() + "!   Next fight unlocked";
        moneyTextGameObject.SetActive(true);

        yield return new WaitUntil(() => Input.anyKeyDown == true);

        moneyTextGameObject.SetActive(false);
    }
}
