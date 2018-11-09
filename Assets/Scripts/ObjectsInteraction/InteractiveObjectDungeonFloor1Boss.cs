using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InteractiveObjectDungeonFloor1Boss : InteractiveObjectAbstract {

    public PublicVariableHolderArenaEntrance publicVariableHolderArenaEntrance;

    private GameObject Boy;
    private GameObject Girl;

    private GameObject Momo;
    private GameObject Bella;
    private GameObject BoyPosBoss;
    private GameObject GirlPosBoss;
    private GameObject BossPos;

    private GameObject smallEnemy;
    private GameObject boss;

    private GameObject[] LightRoom;
    private GameObject[] EnemiesSpawnPos;
    private GameObject[] LightInteractiveParticles;

    private DirectionalLight directionalLight;

    // Use this for initialization
    new void Start () 
    {
        base.Start();

        Boy = publicVariableHolderArenaEntrance.Boy;
        Girl = publicVariableHolderArenaEntrance.Girl;

        BoyPosBoss = publicVariableHolderArenaEntrance.BoyPosBoss;
        GirlPosBoss = publicVariableHolderArenaEntrance.GirlPosBoss;

        Momo = publicVariableHolderNeverUnload._BoySpriteGameObject;
        Bella = publicVariableHolderNeverUnload._GirlSpriteGameObject;

        LightRoom = publicVariableHolderArenaEntrance.LightsBossRoom;
        LightInteractiveParticles = publicVariableHolderArenaEntrance.LightInteractiveParticles;
	
        BossPos = publicVariableHolderArenaEntrance.BossPos;
        EnemiesSpawnPos = publicVariableHolderArenaEntrance.EnemiesSpawnPos;

        smallEnemy = publicVariableHolderArenaEntrance.smallEnemies;
        boss = publicVariableHolderArenaEntrance.boss;

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

        Vector3 BoyNewPos = new Vector3(BoyPosBoss.transform.position.x, Boy.transform.position.y, BoyPosBoss.transform.position.z);
        Vector3 GirlNewPos = new Vector3(GirlPosBoss.transform.position.x, Girl.transform.position.y, GirlPosBoss.transform.position.z);

        Boy.GetComponent<NavMeshAgent>().SetDestination(BoyNewPos);
        Girl.GetComponent<NavMeshAgent>().SetDestination(GirlNewPos);

        yield return new WaitForSeconds(1f);

        NavMeshAgent boynav = Boy.GetComponent<NavMeshAgent>();
        NavMeshAgent girlnav = Girl.GetComponent<NavMeshAgent>();

        Debug.Log(boynav.velocity);
        yield return new WaitUntil(() => boynav.velocity == Vector3.zero && girlnav.velocity == Vector3.zero);

        Momo.GetComponent<SpriteScript2>().ForcePlayerRotation(1);
        Bella.GetComponent<SpriteScript2>().ForcePlayerRotation(1);

        for (int i = 0; i < LightRoom.Length; i++)
        {
            LightRoom[i].GetComponent<TorcheLighten>().OpenLight();
            i += 1;
            LightRoom[i].GetComponent<TorcheLighten>().OpenLight();
            yield return new WaitForSeconds(0.5f);
        }

        Bella.GetComponent<SpriteScript2>().ForcePlayerRotation(4);
        yield return new WaitForSeconds(0.5f);
        Momo.GetComponent<SpriteScript2>().ForcePlayerRotation(3);
        yield return new WaitForSeconds(1.5f);
        Bella.GetComponent<SpriteScript2>().ForcePlayerRotation(1);
        Momo.GetComponent<SpriteScript2>().ForcePlayerRotation(1);
        yield return new WaitForSeconds(1f);
        Girl.GetComponent<NavMeshAgent>().SetDestination(BossPos.transform.position);

        ScreenFader.fadeOut();

        yield return new WaitForSeconds(3f);

        ScreenFader.fadeIn();
        publicVariableHolderNeverUnload.PlayerUI.SetActive(true);

        yield return new WaitForSeconds(1f);

        //GameObject[] Enemiesgo = new GameObject[EnemiesSpawnPos.Length];

        //Enemiesgo[0] = Instantiate(boss, EnemiesSpawnPos[0].transform.position, Quaternion.identity);
        //Enemiesgo[0].SetActive(true);
        //for (int i = 1; i < EnemiesSpawnPos.Length - 1; i++)
        //{
        //    Enemiesgo[i] = Instantiate(smallEnemy, EnemiesSpawnPos[i].transform.position, Quaternion.identity);
        //}

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
