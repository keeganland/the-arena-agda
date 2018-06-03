using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

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

    private int roundNumber;
    private ScreenFader screenFader;
    // Use this for initialization
    private void Start()
    {
      
    }

    private void Awake()
    {
        enterArena = new UnityAction(EnterArena);
        publicVariableHolder = GameObject.Find("/PublicVariableHolderNeverUnload").GetComponent<PublicVariableHolderneverUnload>();

        boyNavMeshAgent = publicVariableHolder.BoynavMeshAgent;
        girlNavMeshAgent = publicVariableHolder.GirlnavMeshAgent;

        m_boy = publicVariableHolder.Boy;
        Debug.Log(m_boy.name);
        m_girl = publicVariableHolder.Girl;

        _InitialPositionBoy = _PublicVariableHolderArena._InitialPositionBoy;
        _InitialPositionGirl = _PublicVariableHolderArena._InitialPositionGirl;
        _InitialPositionEnemy = _PublicVariableHolderArena._InitialPositionEnemy;
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

        EventManager.TriggerEvent("StopMoving"); //Player doesn't move with SetDestination if Triggered. 
        enemy.GetComponent<FirstEnemyAttack2>().isEnemyMoving = true;
       
        m_boy.transform.position =_InitialPositionBoy.transform.position;
        m_girl.transform.position = _InitialPositionGirl.transform.position;
        enemy.transform.position = _InitialPositionEnemy.transform.position;

        boyNavMeshAgent.SetDestination(_PublicVariableHolderArena._EnterArenaWaypointsBoy[0].transform.position);
        girlNavMeshAgent.SetDestination(_PublicVariableHolderArena._EnterArenaWaypointsGirl[0].transform.position);
        Debug.Log("here");

        yield return new WaitForSeconds(5); 

        //Continue with the Event : -Enemy Enters, -Text Appears, -Release Control to Player.


    }
}
