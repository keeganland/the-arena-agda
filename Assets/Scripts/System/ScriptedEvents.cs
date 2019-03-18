using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;
using TMPro;

public class ScriptedEvents : MonoBehaviour {

    SaveManager saveManager;

    private UnityAction enterArena;
    private UnityAction victoryEvent;

    private PublicVariableHolderneverUnload publicVariableHolder;
    public PublicVariableHolderArena _PublicVariableHolderArena;

    public GameObject tutorial1;
    public GameObject tutorial2;
    public GameObject tutorial3;
    public GameObject tutorial4;
    public GameObject tutorial5;
    public GameObject tutorial6;

    [SerializeField] private GameObject _InitialPositionBoy;
    [SerializeField] private GameObject _InitialPositionGirl;
    [SerializeField] private GameObject _InitialPositionEnemy;
    [SerializeField] private GameObject _DungeonPositionGirl;
    [SerializeField] private GameObject _DungeonPositionBoy;


    [SerializeField] private NavMeshAgent boyNavMeshAgent;
    [SerializeField] private NavMeshAgent girlNavMeshAgent;
    [SerializeField] private GameObject boyPlayer;
    [SerializeField] private GameObject girlPlayer;

    public GameObject enemy;
    public GameObject enemyUI;

    public GameObject enemy2;
    public GameObject enemy2UI;
    public GameObject enemy2Sprite;

    public GameObject enemy3;
    public GameObject enemy3UI;

    private GameObject camera;

    private int roundNumber;

    private GameObject ReadyText;
    private GameObject FightText;
    private GameObject YouWonText;
    private GameObject PlayerUI;

    // Use this for initialization
    private void Start()
    {
        publicVariableHolder = GameObject.Find("/PublicVariableHolderNeverUnload").GetComponent<PublicVariableHolderneverUnload>();

        camera = Camera.main.gameObject;
        boyNavMeshAgent = publicVariableHolder.BoynavMeshAgent;
        girlNavMeshAgent = publicVariableHolder.GirlnavMeshAgent;

        girlPlayer = GameObject.FindGameObjectWithTag("Player/Girl");
        boyPlayer = GameObject.FindGameObjectWithTag("Player/Boy");

        _InitialPositionBoy = _PublicVariableHolderArena._InitialPositionBoy;
        _InitialPositionGirl = _PublicVariableHolderArena._InitialPositionGirl;
        _InitialPositionEnemy = _PublicVariableHolderArena._InitialPositionEnemy;
        _DungeonPositionGirl = _PublicVariableHolderArena._DungeonPositionGirl;
        _DungeonPositionBoy = _PublicVariableHolderArena._DungeonPositionBoy;

        ReadyText = _PublicVariableHolderArena.ReadyText;
        FightText = _PublicVariableHolderArena.FightText;
        YouWonText = _PublicVariableHolderArena.YouWonText;
        PlayerUI = publicVariableHolder.PlayerUI;

        girlPlayer.GetComponent<BoxCollider>().enabled = true;
    }

    private void Awake()
    {
        enterArena = new UnityAction(EnterArenaFight1);
        victoryEvent = new UnityAction(VictoryEvent);
        saveManager = FindObjectOfType<SaveManager>();
    }

    private void OnEnable()
    {
        EventManager.StartListening("enterArenaFight1", EnterArenaFight1);
        EventManager.StartListening("enterArenaFight2", EnterArenaFight2);
        EventManager.StartListening("enterArenaFight3", EnterArenaFight3);
        EventManager.StartListening("victoryEvent", VictoryEvent);
    }

    private void OnDisable()
    {
        EventManager.StopListening("enterArenaFight1", EnterArenaFight1);
        EventManager.StopListening("enterArenaFight2", EnterArenaFight2);
        EventManager.StopListening("enterArenaFight3", EnterArenaFight3);
        EventManager.StopListening("victoryEvent", VictoryEvent);
    }

    void VictoryEvent()
    {
        EventManager.StopListening("victoryEvent", VictoryEvent);
        StartCoroutine("victoryEventCoroutine");
    }

    void EnterArenaFight1()
    {
        EventManager.StopListening("enterArenaFight1", EnterArenaFight1);
        StartCoroutine("EnterArenaFight1Coroutine");
    }

    void EnterArenaFight2()
    {
        EventManager.StopListening("enterArenaFight2", EnterArenaFight2);
        StartCoroutine("EnterArenaFight2Coroutine");
    }

    void EnterArenaFight3()
    {
        EventManager.StopListening("enterArenaFight3", EnterArenaFight3);
        StartCoroutine("EnterArenaFight3Coroutine");
    }

    IEnumerator EnterArenaFight1Coroutine () {

        publicVariableHolder.BoyUIGameObject.SetActive(false);
        publicVariableHolder.GirlUIGameObject.SetActive(false);
        PlayerUI.SetActive(false);
        EventManager.TriggerEvent("setup");
        EventManager.TriggerEvent("InCombat");
        EventManager.TriggerEvent("StopMoving");
        enemy.GetComponentInChildren<FirstEnemyAttack2>().isEnemyMoving = false;
        enemy.GetComponentInChildren<FirstEnemyAttack2>().StopAttacking = true;
        
        enemyUI.SetActive(false);
        camera.transform.position = new Vector3(_InitialPositionBoy.transform.position.x, camera.transform.position.y, _InitialPositionBoy.transform.position.z);

        SoundManager.SetBackgroundSFX(Resources.Load("BackgroundMusic/Windsound") as AudioClip);
        SoundManager.FadeInBackgroundSFX();
        //SetBackgroundMusic("BackgroundMusic/BattleArena-Drums only");
        yield return new WaitForSeconds(.4f);

        boyPlayer.transform.position =_InitialPositionBoy.transform.position;
        girlPlayer.transform.position = _InitialPositionGirl.transform.position;
        enemy.transform.position = _InitialPositionEnemy.transform.position;

        publicVariableHolder.BoyUIGameObject.SetActive(true);
        publicVariableHolder.GirlUIGameObject.SetActive(true);

        boyNavMeshAgent.SetDestination(_PublicVariableHolderArena._EnterArenaWaypointsBoy[0].transform.position);
        girlNavMeshAgent.SetDestination(_PublicVariableHolderArena._EnterArenaWaypointsGirl[0].transform.position);

        yield return new WaitForSeconds(.4f);

        ScreenFader.fadeIn();

        yield return new WaitForSeconds(4.5f);

        ScreenFader.fadeOut();

        yield return new WaitForSeconds(2.5f);
        camera.transform.position = new Vector3(_InitialPositionEnemy.transform.position.x, camera.transform.position.y, _InitialPositionEnemy.transform.position.z);

        ScreenFader.fadeIn();

        yield return new WaitForSeconds(2f);
        enemy.GetComponentInChildren<NavMeshAgent>().SetDestination(_PublicVariableHolderArena._EnterArenaWaypointsEnemy[0].transform.position);
        enemyUI.SetActive(true);

        yield return new WaitForSeconds(6);

        if (tutorial1)
        {
            tutorial1.SetActive(true);

            while (tutorial1 != null)
            {
                yield return null;
            }
        }

        if (tutorial2)
        {
            tutorial2.SetActive(true);
            while (tutorial2 != null)
            {
                yield return null;
            }
        }
        ReadyText.SetActive(true);

        yield return new WaitForSeconds(1);
        SoundManager.ExitScene();
        yield return new WaitForSeconds(1);

        ReadyText.SetActive(false);
        FightText.SetActive(true);
        SetBackgroundMusic("BackgroundMusic/BattleArena");
        yield return new WaitForSeconds(2);
        SoundManager.FadeOutBackgroundSFX();
        FightText.SetActive(false);
        PlayerUI.SetActive(true);
        PlayerUI.GetComponent<UISpellSwap>().HiddeSpells();
                
        EventManager.TriggerEvent("StartMoving");
        enemy.GetComponentInChildren<FirstEnemyAttack2>().isEnemyMoving = true;
        enemy.GetComponentInChildren<FirstEnemyAttack2>().StopAttacking = false;
    }

    IEnumerator EnterArenaFight2Coroutine()
    {
        VictoryReferee.ResetEnemyList();
        VictoryReferee.SetVictoryCondition(2);
        publicVariableHolder.BoyUIGameObject.SetActive(false);
        publicVariableHolder.GirlUIGameObject.SetActive(false);
        enemy2.GetComponent<SecondEnemyAttack>().enabled = false;
        enemy2Sprite.SetActive(false);

        PlayerUI.SetActive(false);
        EventManager.TriggerEvent("setup");
        EventManager.TriggerEvent("InCombat");
        EventManager.TriggerEvent("StopMoving");
        EventManager.TriggerEvent("ResetTargets");

        enemy2UI.SetActive(false);
        camera.transform.position = new Vector3(_InitialPositionBoy.transform.position.x, camera.transform.position.y, _InitialPositionBoy.transform.position.z);

        yield return new WaitForSeconds(.4f);
        boyPlayer.transform.position = _InitialPositionBoy.transform.position;
        girlPlayer.transform.position = _InitialPositionGirl.transform.position;
        enemy.transform.position = _InitialPositionEnemy.transform.position;

        publicVariableHolder.BoyUIGameObject.SetActive(true);
        publicVariableHolder.GirlUIGameObject.SetActive(true);

        boyNavMeshAgent.SetDestination(_PublicVariableHolderArena._EnterArenaWaypointsBoy[0].transform.position);
        girlNavMeshAgent.SetDestination(_PublicVariableHolderArena._EnterArenaWaypointsGirl[0].transform.position);

        yield return new WaitForSeconds(.4f);

        ScreenFader.fadeIn();

        yield return new WaitForSeconds(4.5f);

        enemy2Sprite.SetActive(true);
        enemy2UI.SetActive(true);
        enemy2Sprite.GetComponent<Animator>().Play("Enemy2Entrance");

        yield return new WaitForSeconds(2f);
        if (tutorial3)
        {
            tutorial3.SetActive(true);

            while (tutorial3 != null)
            {
                yield return null;
            }
        }

        if (tutorial4)
        {
            tutorial4.SetActive(true);
            while (tutorial4 != null)
            {
                yield return null;
            }
        }

        //Keegannote 2018/8/17: im just adding the stuff below to see if it fixes the entrance to the second fight. Delete if no

        ReadyText.SetActive(true);
        yield return new WaitForSeconds(2);
        ReadyText.SetActive(false);
        FightText.SetActive(true);
        SetBackgroundMusic("BackgroundMusic/BattleArena");

        yield return new WaitForSeconds(2);
        FightText.SetActive(false);

        PlayerUI.SetActive(true);
        PlayerUI.GetComponent<UISpellSwap>().HiddeSpells();
        PlayerUI.GetComponent<UISpellSwap>().ShowGirlSpellsOnly();

        EventManager.TriggerEvent("StartMoving");

        enemy.GetComponentInChildren<FirstEnemyAttack2>().isEnemyMoving = true;
        enemy.GetComponentInChildren<FirstEnemyAttack2>().StopAttacking = false;
        enemy2.GetComponent<SecondEnemyAttack>().enabled = true;
    }

    IEnumerator EnterArenaFight3Coroutine()
    {
        VictoryReferee.ResetEnemyList();
        VictoryReferee.SetVictoryCondition(0);

        publicVariableHolder.BoyUIGameObject.SetActive(false);
        publicVariableHolder.GirlUIGameObject.SetActive(false);
        PlayerUI.SetActive(false);
        EventManager.TriggerEvent("setup");
        EventManager.TriggerEvent("InCombat");
        EventManager.TriggerEvent("StopMoving");
        EventManager.TriggerEvent("ResetTargets");
        enemy3.GetComponentInChildren<ThirdEnemy>().StopAttacking = true;

        enemy3UI.SetActive(false);
        camera.transform.position = new Vector3(_InitialPositionBoy.transform.position.x, camera.transform.position.y, _InitialPositionBoy.transform.position.z);

        yield return new WaitForSeconds(.4f);
        boyPlayer.transform.position = _InitialPositionBoy.transform.position;
        girlPlayer.transform.position = _InitialPositionGirl.transform.position;
        enemy3.transform.position = _InitialPositionEnemy.transform.position;

        publicVariableHolder.BoyUIGameObject.SetActive(true);
        publicVariableHolder.GirlUIGameObject.SetActive(true);

        boyNavMeshAgent.SetDestination(_PublicVariableHolderArena._EnterArenaWaypointsBoy[0].transform.position);
        girlNavMeshAgent.SetDestination(_PublicVariableHolderArena._EnterArenaWaypointsGirl[0].transform.position);

        yield return new WaitForSeconds(.4f);

        ScreenFader.fadeIn();

        yield return new WaitForSeconds(4.5f);

        ScreenFader.fadeOut();

        yield return new WaitForSeconds(2.5f);
        camera.transform.position = new Vector3(_InitialPositionEnemy.transform.position.x, camera.transform.position.y, _InitialPositionEnemy.transform.position.z);

        ScreenFader.fadeIn();

        yield return new WaitForSeconds(2f);
        enemy3.GetComponentInChildren<NavMeshAgent>().SetDestination(_PublicVariableHolderArena._EnterArenaWaypointsEnemy[0].transform.position);
        enemy3UI.SetActive(true);

        yield return new WaitForSeconds(6);

        if (tutorial5)
        {
            tutorial5.SetActive(true);

            while (tutorial5 != null)
            {
                yield return null;
            }
        }

        if (tutorial6)
        {
            tutorial6.SetActive(true);
            while (tutorial6 != null)
            {
                yield return null;
            }
        }

        ReadyText.SetActive(true);
        yield return new WaitForSeconds(2);
        ReadyText.SetActive(false);
        FightText.SetActive(true);
        SetBackgroundMusic("BackgroundMusic/BattleArena");

        yield return new WaitForSeconds(2);
        FightText.SetActive(false);

        PlayerUI.SetActive(true);
        PlayerUI.GetComponent<UISpellSwap>().ShowSpells();

        EventManager.TriggerEvent("StartMoving");
        enemy3.GetComponentInChildren<ThirdEnemy>().StopAttacking = false;
    }

    IEnumerator victoryEventCoroutine()
    {
        if (saveManager.currentFight != 2)
        {
            EventManager.StopListening("victoryEvent", VictoryEvent);
            SoundManager.ExitScene();

            PlayerUI.SetActive(false);
            EventManager.TriggerEvent("StopMoving");

            AudioSource audio = gameObject.AddComponent<AudioSource>();
            audio.volume = 0.05f;
            audio.PlayOneShot(Resources.Load("BackgroundMusic/BRPG_Victory_Stinger") as AudioClip);

            YouWonText.SetActive(true);
            yield return new WaitForSeconds(4);
            boyNavMeshAgent.SetDestination(_InitialPositionBoy.transform.position);
            girlNavMeshAgent.SetDestination(_InitialPositionGirl.transform.position);
            yield return new WaitForSeconds(3);
            Destroy(gameObject.GetComponent<AudioSource>());
            YouWonText.SetActive(false);

            ScreenFader.fadeOut();
            yield return new WaitForSeconds(1.5f);
            saveManager.returnFromArena = true;
            LoadingScreen.LoadScene("ArenaEntrance", "Arena");
        }
        else if(saveManager.currentFight == 2)
        {
            EventManager.StopListening("victoryEvent", VictoryEvent);
            PlayerUI.SetActive(false);
            EventManager.TriggerEvent("StopMoving");
            SoundManager.ExitScene();
            YouWonText.SetActive(true);
            yield return new WaitForSeconds(4);
            boyNavMeshAgent.SetDestination(_DungeonPositionBoy.transform.position);
            girlNavMeshAgent.SetDestination(_DungeonPositionGirl.transform.position);
            yield return new WaitForSeconds(3);
            YouWonText.SetActive(false);
            ScreenFader.fadeOut();
            yield return new WaitForSeconds(1.5f);
            EventManager.TriggerEvent("setup");
            saveManager.returnFromArena = true;
            LoadingScreen.LoadScene("ArenaDungeonFloor1", "Arena");
        }
    }

    private void SetBackgroundMusic(string name, bool EnterScene = false)
    {
        SoundManager.SetBackgroundMusic(Resources.Load(name) as AudioClip);
        SoundManager.Loop(true);
        SoundManager.Instance.ScaleFactor = 0.05f;
        SoundManager.Instance.ExitSpeed = 0.05f;
        SoundManager.EnterScene();
    }
}

