using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class PublicVariableHolderArena : MonoBehaviour {

    [Header("Genera Information")]
    public GameObject[] _Enemies;

    [Header("Scripted Events/Waypoints")]
    public GameObject[] _EnterArenaWaypointsBoy; //Goes with "/ScritableEvent:EnterArena/Waypoints/BoyWaypoint_n"
    public GameObject[] _EnterArenaWaypointsGirl; //Goes with "/ScritableEvent:EnterArena/Waypoints/GirlWaypoint_n"
    public GameObject[] _EnterArenaWaypointsEnemy; //Goes with "/ScritableEvent:EnterArena/Waypoints/EnemyWaypoint_n"
    [Header("Scripted Events/InitialPositions")]
    public GameObject _InitialPositionBoy; //Goes with "InitialPositionBoy"
    public GameObject _InitialPositionGirl; //Goes with "InitialPositionGirl"
    public GameObject _InitialPositionEnemy; //Goes with "InitialPositionEnemy"

}
