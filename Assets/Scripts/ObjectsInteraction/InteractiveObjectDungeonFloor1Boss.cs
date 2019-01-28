using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InteractiveObjectDungeonFloor1Boss : InteractiveObjectAbstract {

    public PublicVariableHolderArenaEntrance publicVariableHolderArenaEntrance;

    private GameObject boyPlayer;
    private GameObject girlPlayer;

    private GameObject boySpriteGameobject;
    private GameObject girlSpriteGameobject;

    private GameObject BoyPosBoss;
    private GameObject GirlPosBoss;
    private GameObject BossPos;

    private GameObject smallEnemy;
    private GameObject boss;
    private GameObject LightCollider;

    private GameObject[] LightRoom;
    private GameObject[] LightRoomCollidersSpawnPos;
    private GameObject[] EnemiesSpawnPos;
    private GameObject[] LightInteractiveParticles;

    private GameObject DarknessColliderSpawnPos;
    private GameObject DarknessCollider;
    private GameObject Darknessgo;

    private DirectionalLight directionalLight;

    // Use this for initialization
    new void Start () 
    {
        base.Start();

        girlPlayer = GameObject.FindGameObjectWithTag("Player/Girl");
        boyPlayer = GameObject.FindGameObjectWithTag("Player/Boy");

        girlSpriteGameobject = GameObject.FindGameObjectWithTag("Sprite/Girl");
        boySpriteGameobject = GameObject.FindGameObjectWithTag("Sprite/Boy");

        BoyPosBoss = publicVariableHolderArenaEntrance.BoyPosBoss;
        GirlPosBoss = publicVariableHolderArenaEntrance.GirlPosBoss;

        LightRoom = publicVariableHolderArenaEntrance.LightsBossRoom;
        LightRoomCollidersSpawnPos = publicVariableHolderArenaEntrance.LightsBossRoomCollidersSpawnPos;
        LightInteractiveParticles = publicVariableHolderArenaEntrance.LightInteractiveParticles;
	
        BossPos = publicVariableHolderArenaEntrance.BossPos;
        EnemiesSpawnPos = publicVariableHolderArenaEntrance.EnemiesSpawnPos;

        smallEnemy = publicVariableHolderArenaEntrance.smallEnemies;
        boss = publicVariableHolderArenaEntrance.boss;
        LightCollider = publicVariableHolderArenaEntrance.TorchCollider;

        DarknessCollider = publicVariableHolderArenaEntrance.DarknessCollider;
        DarknessColliderSpawnPos = publicVariableHolderArenaEntrance.DarknessColliderSpawnPos;

        directionalLight = publicVariableHolderArenaEntrance.directionalLight;
    }

    public override void DoAction(GameObject sender)
    {
        StartCoroutine("Action", sender);
    }

    public override IEnumerator Action(GameObject sender)
    {
        EventManager.TriggerEvent("InCombat");
        EventManager.TriggerEvent("StopMoving");

        Vector3 BoyNewPos = new Vector3(BoyPosBoss.transform.position.x, boyPlayer.transform.position.y, BoyPosBoss.transform.position.z);
        Vector3 GirlNewPos = new Vector3(GirlPosBoss.transform.position.x, girlPlayer.transform.position.y, GirlPosBoss.transform.position.z);

        boyPlayer.GetComponent<NavMeshAgent>().SetDestination(BoyNewPos);
        girlPlayer.GetComponent<NavMeshAgent>().SetDestination(GirlNewPos);

        yield return new WaitForSeconds(1f);

        NavMeshAgent boynav = boyPlayer.GetComponent<NavMeshAgent>();
        NavMeshAgent girlnav = girlPlayer.GetComponent<NavMeshAgent>();

        //Debug.Log(boynav.velocity);
        yield return new WaitUntil(() => boynav.velocity == Vector3.zero && girlnav.velocity == Vector3.zero);

        boySpriteGameobject.GetComponent<SpriteScript2>().ForcePlayerRotation(1);
        girlSpriteGameobject.GetComponent<SpriteScript2>().ForcePlayerRotation(1);

        for (int i = 0; i < LightRoom.Length; i++)
        {
            LightRoom[i].GetComponent<TorcheLighten>().OpenLight();
            i += 1;
            LightRoom[i].GetComponent<TorcheLighten>().OpenLight();
            yield return new WaitForSeconds(0.5f);
        }

        girlSpriteGameobject.GetComponent<SpriteScript2>().ForcePlayerRotation(4);
        yield return new WaitForSeconds(0.5f);
        boySpriteGameobject.GetComponent<SpriteScript2>().ForcePlayerRotation(3);
        yield return new WaitForSeconds(1.5f);
        girlSpriteGameobject.GetComponent<SpriteScript2>().ForcePlayerRotation(1);
        boySpriteGameobject.GetComponent<SpriteScript2>().ForcePlayerRotation(1);
        yield return new WaitForSeconds(1f);
        girlPlayer.GetComponent<NavMeshAgent>().SetDestination(BossPos.transform.position);

        ScreenFader.fadeOut();

        yield return new WaitForSeconds(3f);

        ScreenFader.fadeIn();
        publicVariableHolderNeverUnload.PlayerUI.SetActive(true);

        yield return new WaitForSeconds(1f);

        GameObject[] Enemiesgo = new GameObject[EnemiesSpawnPos.Length];

        Enemiesgo[0] = Instantiate(boss, EnemiesSpawnPos[0].transform.position, Quaternion.identity); //Spawns the boss
        Enemiesgo[0].SetActive(true);
        //for (int i = 1; i < EnemiesSpawnPos.Length - 1; i++) //Spawns the Skeletons
        //{
        //    Enemiesgo[i] = Instantiate(smallEnemy, EnemiesSpawnPos[i].transform.position, Quaternion.identity);
        //}

        //Instantiating the colliders required to cause the damage in the dark
        //Quite sure I don't want to instantiate the light colliders here; WHY??? I'm doing it until I figure it out
        GameObject[] LightCollidersgo = new GameObject[LightRoomCollidersSpawnPos.Length];
        for(int i = 0; i < LightRoomCollidersSpawnPos.Length; i++)
        {
            LightCollidersgo[0] = Instantiate(LightCollider, LightRoomCollidersSpawnPos[i].transform.position, Quaternion.identity);
        }

        Darknessgo = Instantiate(DarknessCollider, DarknessColliderSpawnPos.transform.position, Quaternion.identity);

        GameObject.FindObjectOfType<ColliderDarknessDamage>().Ghoul = Enemiesgo[0];

        for (int i = 0; i < LightRoom.Length; i++)
        {
            LightInteractiveParticles[i].SetActive(true);
            LightRoom[i].GetComponent<Collider>().enabled = true;
        }

        directionalLight.LightDown();

        Destroy(this.gameObject);
    }

    public override void ActionFunction(GameObject sender)
    { 
        throw new System.NotImplementedException(); 
    }

    public override void CancelAction(GameObject sender)
    {
        throw new System.NotImplementedException();
    }
}
