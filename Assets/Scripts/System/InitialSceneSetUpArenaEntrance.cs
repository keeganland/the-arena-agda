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

    public AudioClip BackgroundMusic;

    new void Start()
    {
        base.Start();

        SoundManager.SetBackgroundMusic(BackgroundMusic);

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

        boyPlayer.transform.position = new Vector3(SpawnPosBoy.transform.position.x, transform.position.y, SpawnPosBoy.transform.position.z);
        girlPlayer.transform.position = new Vector3(SpawnPosGirl.transform.position.x, transform.position.y, SpawnPosGirl.transform.position.z);

        BoyNav.enabled = true;
        GirlNav.enabled = true;

        EventManager.TriggerEvent("NotInCombat");
        girlPlayer.GetComponent<BoxCollider>().enabled = false;

        MainCamera = Camera.main.gameObject;
        MainCamera.GetComponent<BetterCameraFollow>().SetFieldOfView(MainCameraFieldOfViewMin, MainCameraFieldOfViewMax);

        EventManager.TriggerEvent("setup");

        girlPlayer.GetComponent<BetterPlayer_Movement>().BoyActive = false;
        boyPlayer.GetComponent<BetterPlayer_Movement>().BoyActive = false;
        publicArenaEntrance.publicVariableHolderNeverUnload.fader.StartCoroutine("FadeIn");

        if (publicArenaEntrance.ReturnFromArena && saveManager.returnFromArena)
            StartCoroutine(ArenaArrival());

    }

    private IEnumerator ArenaArrival()
    {
        GirlNav.SetDestination(new Vector3(publicArenaEntrance.ReturnFromArena.transform.position.x, girlPlayer.transform.position.y, publicArenaEntrance.ReturnFromArena.transform.position.z));
        BoyNav.SetDestination(girlPlayer.transform.position);

        yield return new WaitForSeconds(2f);

        //TODO keegan you removed all dialogue boxes so "MoneyWon" doesn't have a reference anymore
        InventoryManager.AddMoney(moneyGainedperFight[FindObjectOfType<SaveManager>().currentFight]);
        moneyWon.text = "You won : $" + moneyGainedperFight[FindObjectOfType<SaveManager>().currentFight].ToString() + "!   Next fight unlocked";
        moneyTextGameObject.SetActive(true);

        yield return new WaitUntil(() => Input.anyKeyDown == true);

        moneyTextGameObject.SetActive(false);
    }
}
