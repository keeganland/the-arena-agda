using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using TMPro;


public class PublicVariableHolderArena : MonoBehaviour {

    [Header("Genera Information")]
    public GameObject[] _Enemies; //Goes with "Enemies" (arena)

    [Header("Scripted Events/Waypoints")]
    public GameObject[] _EnterArenaWaypointsBoy; //Goes with "/ScritableEvent:EnterArena/Waypoints/BoyWaypoint_n" (arena)
    public GameObject[] _EnterArenaWaypointsGirl; //Goes with "/ScritableEvent:EnterArena/Waypoints/GirlWaypoint_n" (arena)
    public GameObject[] _EnterArenaWaypointsEnemy; //Goes with "/ScritableEvent:EnterArena/Waypoints/EnemyWaypoint_n" (arena)

    [Header("Scripted Events/InitialPositions")]
    public GameObject _InitialPositionBoy; //Goes with "InitialPositionBoy" (arena)
    public GameObject _InitialPositionGirl; //Goes with "InitialPositionGirl" (arena)
    public GameObject _InitialPositionEnemy; //Goes with "InitialPositionEnemy" (arena)

    [Header("Scripted Events/UI")]
    public GameObject ReadyText; //Goes with "Ready"
    public GameObject FightText; //Goes with "Figh!t"
    public GameObject YouWonText; //Goes witH "You Won!"

    [Header("Enemies/SecondEnemyPrefab/SecondEnemyAttack")]
    public GameObject _SheepPrefab; //Goes with "FirstEnemy" (assets)
    public Transform[] _TotemTeleportPos; //GOes with "/SecondEnemyTeleportPosition/..." (arena)
    public Transform[] _Spawnposition;
    public ParticleSystem _SpawnSheepAnim;
    public ParticleSystem[] _TeleportSpawn;
    public ParticleSystem _TeleportStart;
    public ParticleSystem _TeleportArrives;

    [Header("Enemies/ThirdEnemy-Agent/ThirdEnemy")]
    public LineRenderer[] LaserBeam;
}
