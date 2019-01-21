using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PublicVariableHolderArenaEntrance : MonoBehaviour
{

    public PublicVariableHolderneverUnload publicVariableHolderNeverUnload;

    [Header("GeneralCharacteristics")]
    public GameObject MainCamera;

    [Header("SpawnPosition")]
    public GameObject SpawnPosBoy;
    public GameObject SpawnPosGirl;
    public GameObject ScriptedPosBoyIntro;

    [Header("InteractiveObjectsEvents")]
    public GameObject DungeonDoorFloor1Boy;
    public GameObject DungeonDoorFloor1Girl;

    [Header("BossTrigger")]
    public DirectionalLight directionalLight;
    public GameObject BoyPosBoss;
    public GameObject GirlPosBoss;
    public GameObject BossPos;
    public GameObject[] LightsBossRoom;
    public GameObject[] LightsBossRoomCollidersSpawnPos; //Spawn Positions at each of the lights
    public GameObject[] LightInteractiveParticles;
    public GameObject[] EnemiesSpawnPos; //pos 0 = boss, 1-4 small enemies;
    public GameObject smallEnemies;
    public GameObject boss;
    public GameObject TorchCollider; //The colliders around the torches
    public GameObject DarknessCollider; //The large collider over the whole boss room
    public GameObject DarknessColliderSpawnPos; //Where the darkness collider spawns

    [Header("Walk To Door, Arena Entrance")]
    public GameObject BoyDoorPos;
    public GameObject GirlDoorPos;
    public GameObject ReturnFromArenaGirlPos;
    public GameObject ReturnFromArenaBoyPos;
    public GameObject ReturnFromArena;

    [Header("Exit floor 1")]
    public GameObject ExitDoor;
    public Animator Floor2Door;

    private void Awake()
    {
        publicVariableHolderNeverUnload = GameObject.Find("/PublicVariableHolderNeverUnload").GetComponent<PublicVariableHolderneverUnload>();
        MainCamera = Camera.main.gameObject;
    }
}