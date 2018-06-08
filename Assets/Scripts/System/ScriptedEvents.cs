using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;
using TMPro;

public class ScriptedEvents : MonoBehaviour {

    private UnityAction enterArena;

    private PublicVariableHolderneverUnload publicVariableHolder;
    public PublicVariableHolderArena _PublicVariableHolderArena;


    [SerializeField] private GameObject _InitialPositionBoy;
    [SerializeField] private GameObject _InitialPositionGirl;
    [SerializeField] private GameObject _InitialPositionEnemy; 
    

    [SerializeField] private NavMeshAgent boyNavMeshAgent;
    [SerializeField] private NavMeshAgent girlNavMeshAgent;
    [SerializeField] private GameObject m_boy;
    [SerializeField] private GameObject m_girl;

    public GameObject enemy;
    public GameObject enemyUI;
    private GameObject Camera;

    private int roundNumber;
    private ScreenFader screenFader;

    private GameObject ReadyText;
    private GameObject FightText;
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
        PlayerUI = publicVariableHolder.PlayerUI;
    }

    private void Awake()
    {
        enterArena = new UnityAction(EnterArena);
    }

    private void OnEnable()
    {
        EventManager.StartListening("enterArena", EnterArena);
    }

    private void OnDisable()
    {
        EventManager.StopListening("enterArena", EnterArena);
    }

    void EnterArena()
    {
        EventManager.StopListening("enterArena", EnterArena);
        StartCoroutine("EnterArenaCoroutine");
    }

    IEnumerator EnterArenaCoroutine () {

        PlayerUI.SetActive(false);
        EventManager.TriggerEvent("StopMoving");
        publicVariableHolder.StopAllActions = true;
        enemy.GetComponentInChildren<FirstEnemyAttack2>().isEnemyMoving = false;
        enemy.GetComponentInChildren<FirstEnemyAttack2>().StopAttacking = true;
        
        enemyUI.SetActive(false);
       
        m_boy.transform.position =_InitialPositionBoy.transform.position;
        m_girl.transform.position = _InitialPositionGirl.transform.position;
        enemy.transform.position = _InitialPositionEnemy.transform.position;

        boyNavMeshAgent.SetDestination(_PublicVariableHolderArena._EnterArenaWaypointsBoy[0].transform.position);
        girlNavMeshAgent.SetDestination(_PublicVariableHolderArena._EnterArenaWaypointsGirl[0].transform.position);

        yield return new WaitForSeconds(4.5f);

        screenFader.StartCoroutine("FadeOut");

        yield return new WaitForSeconds(2.5f);
        Camera.transform.position = new Vector3(_InitialPositionEnemy.transform.position.x, Camera.transform.position.y, _InitialPositionEnemy.transform.position.z);


        screenFader.StartCoroutine("FadeIn");
        yield return new WaitForSeconds(2f);
        enemy.GetComponentInChildren<NavMeshAgent>().SetDestination(_PublicVariableHolderArena._EnterArenaWaypointsEnemy[0].transform.position);
        enemyUI.SetActive(true);

        yield return new WaitForSeconds(6);
        ReadyText.SetActive(true);
        yield return new WaitForSeconds(2);
        ReadyText.SetActive(false);
        FightText.SetActive(true);
        yield return new WaitForSeconds(2);
        FightText.SetActive(false);

        PlayerUI.SetActive(true);

        EventManager.TriggerEvent("StartMoving");
        publicVariableHolder.StopAllActions = false;
        enemy.GetComponentInChildren<FirstEnemyAttack2>().isEnemyMoving = true;
        enemy.GetComponentInChildren<FirstEnemyAttack2>().StopAttacking = false;
    }
}
