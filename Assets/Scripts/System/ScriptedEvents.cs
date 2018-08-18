﻿using System.Collections;
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

    [SerializeField] private GameObject _InitialPositionBoy;
    [SerializeField] private GameObject _InitialPositionGirl;
    [SerializeField] private GameObject _InitialPositionEnemy; 
    

    [SerializeField] private NavMeshAgent boyNavMeshAgent;
    [SerializeField] private NavMeshAgent girlNavMeshAgent;
    [SerializeField] private GameObject m_boy;
    [SerializeField] private GameObject m_girl;

    public GameObject enemy;
    public GameObject enemyUI;

    public GameObject enemy2;
    private GameObject Camera;

    private int roundNumber;
    private ScreenFader screenFader;

    private GameObject ReadyText;
    private GameObject FightText;
    private GameObject YouWonText;
    private GameObject PlayerUI;

    // Use this for initialization
    private void Start()
    {
        publicVariableHolder = GameObject.Find("/PublicVariableHolderNeverUnload").GetComponent<PublicVariableHolderneverUnload>();
        screenFader = GameObject.Find("/ScreenFader").GetComponent<ScreenFader>();

        Camera = publicVariableHolder.MainCamera;
        boyNavMeshAgent = publicVariableHolder.BoynavMeshAgent;
        girlNavMeshAgent = publicVariableHolder.GirlnavMeshAgent;

        m_boy = publicVariableHolder.Boy;
        m_girl = publicVariableHolder.Girl;

        _InitialPositionBoy = _PublicVariableHolderArena._InitialPositionBoy;
        _InitialPositionGirl = _PublicVariableHolderArena._InitialPositionGirl;
        _InitialPositionEnemy = _PublicVariableHolderArena._InitialPositionEnemy;

        ReadyText = _PublicVariableHolderArena.ReadyText;
        FightText = _PublicVariableHolderArena.FightText;
        YouWonText = _PublicVariableHolderArena.YouWonText;
        PlayerUI = publicVariableHolder.PlayerUI;
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
        EventManager.StartListening("victoryEvent", VictoryEvent);
    }

    private void OnDisable()
    {
        EventManager.StopListening("enterArenaFight1", EnterArenaFight1);
        EventManager.StopListening("enterArenaFight2", EnterArenaFight2);
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
        Camera.transform.position = new Vector3(_InitialPositionBoy.transform.position.x, Camera.transform.position.y, _InitialPositionBoy.transform.position.z);

        yield return new WaitForSeconds(.4f);
        m_boy.transform.position =_InitialPositionBoy.transform.position;
        m_girl.transform.position = _InitialPositionGirl.transform.position;
        enemy.transform.position = _InitialPositionEnemy.transform.position;

        publicVariableHolder.BoyUIGameObject.SetActive(true);
        publicVariableHolder.GirlUIGameObject.SetActive(true);

        boyNavMeshAgent.SetDestination(_PublicVariableHolderArena._EnterArenaWaypointsBoy[0].transform.position);
        girlNavMeshAgent.SetDestination(_PublicVariableHolderArena._EnterArenaWaypointsGirl[0].transform.position);

        yield return new WaitForSeconds(.4f);

        screenFader.StartCoroutine("FadeIn");

        yield return new WaitForSeconds(4.5f);

        screenFader.StartCoroutine("FadeOut");

        yield return new WaitForSeconds(2.5f);
        Camera.transform.position = new Vector3(_InitialPositionEnemy.transform.position.x, Camera.transform.position.y, _InitialPositionEnemy.transform.position.z);

        screenFader.StartCoroutine("FadeIn");

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
        yield return new WaitForSeconds(2);
        ReadyText.SetActive(false);
        FightText.SetActive(true);
        yield return new WaitForSeconds(2);
        FightText.SetActive(false);

        PlayerUI.SetActive(true);
        PlayerUI.GetComponent<UISpellSwap>().HiddeSpells();
                
        EventManager.TriggerEvent("StartMoving");
        enemy.GetComponentInChildren<FirstEnemyAttack2>().isEnemyMoving = true;
        enemy.GetComponentInChildren<FirstEnemyAttack2>().StopAttacking = false;
    }

    IEnumerator EnterArenaFight2Coroutine()
    {
        Debug.Log("here");
        publicVariableHolder.BoyUIGameObject.SetActive(false);
        publicVariableHolder.GirlUIGameObject.SetActive(false);
        enemy2.GetComponent<SecondEnemyAttack>().enabled = false;

        PlayerUI.SetActive(false);
        EventManager.TriggerEvent("setup");
        EventManager.TriggerEvent("InCombat");
        EventManager.TriggerEvent("StopMoving");
        enemy.GetComponentInChildren<FirstEnemyAttack2>().isEnemyMoving = false;
        enemy.GetComponentInChildren<FirstEnemyAttack2>().StopAttacking = true;

        enemyUI.SetActive(false);
        Camera.transform.position = new Vector3(_InitialPositionBoy.transform.position.x, Camera.transform.position.y, _InitialPositionBoy.transform.position.z);

        yield return new WaitForSeconds(.4f);
        m_boy.transform.position = _InitialPositionBoy.transform.position;
        m_girl.transform.position = _InitialPositionGirl.transform.position;
        enemy.transform.position = _InitialPositionEnemy.transform.position;

        publicVariableHolder.BoyUIGameObject.SetActive(true);
        publicVariableHolder.GirlUIGameObject.SetActive(true);

        boyNavMeshAgent.SetDestination(_PublicVariableHolderArena._EnterArenaWaypointsBoy[0].transform.position);
        girlNavMeshAgent.SetDestination(_PublicVariableHolderArena._EnterArenaWaypointsGirl[0].transform.position);

        yield return new WaitForSeconds(.4f);

        screenFader.StartCoroutine("FadeIn");

        yield return new WaitForSeconds(4.5f);


        yield return new WaitForSeconds(2.5f);

        //Keegannote 2018/8/17: im just adding the stuff below to see if it fixes the entrance to the second fight. Delete if no

        ReadyText.SetActive(true);
        yield return new WaitForSeconds(2);
        ReadyText.SetActive(false);
        FightText.SetActive(true);
        yield return new WaitForSeconds(2);
        FightText.SetActive(false);

        PlayerUI.SetActive(true);
        PlayerUI.GetComponent<UISpellSwap>().HiddeSpells();

        EventManager.TriggerEvent("StartMoving");
        enemy.GetComponentInChildren<FirstEnemyAttack2>().isEnemyMoving = true;
        enemy.GetComponentInChildren<FirstEnemyAttack2>().StopAttacking = false;
        enemy2.GetComponent<SecondEnemyAttack>().enabled = true;
    }

    IEnumerator victoryEventCoroutine()
    {
        EventManager.StopListening("victoryEvent", VictoryEvent);
        PlayerUI.SetActive(false);
        EventManager.TriggerEvent("StopMoving");      
        YouWonText.SetActive(true);
        yield return new WaitForSeconds(4);
        boyNavMeshAgent.SetDestination(_InitialPositionBoy.transform.position);
        girlNavMeshAgent.SetDestination(_InitialPositionGirl.transform.position);
        yield return new WaitForSeconds(3);
        YouWonText.SetActive(false);
        screenFader.StartCoroutine("FadeOut");
        yield return new WaitForSeconds(1.5f);
        saveManager.returnFromArena = true;
        GameObject.FindWithTag("LoadingScreen").GetComponent<LoadingScreen>().loadScene("ArenaEntrance", "Arena");
    }
}
