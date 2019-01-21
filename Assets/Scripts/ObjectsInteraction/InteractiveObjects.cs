using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class InteractiveObjects : MonoBehaviour {

    public PublicVariableHolderArenaEntrance publicVariableHolderArenaEntrance;

    public bool isAttackable;

    private PublicVariableHolderneverUnload publicVariableHolderNeverUnload;

    private int totalHealth;
    private int currentHealth;
    public GameObject _DestroyEffect;
    public GameObject Door;
    [Header("Case : 0 DoorDungeonFloor1; Case 1 = BossScriptedEvent; Case 3 = ExitFloor1")]
    public int Case;

    private GameObject DungeonDoorFloor1Boy;
    private GameObject DungeonDoorFloor1Girl;

    private GameObject boyPlayer;
    private GameObject girlPlayer;

    private GameObject boySpriteGameobject;
    private GameObject girlSpriteGameobject;

    private GameObject BoyPosBoss;
    private GameObject GirlPosBoss;
    private GameObject BossPos;

    private GameObject[] LightRoom;
    private GameObject[] EnemiesSpawnPos;
    private GameObject[] LightInteractiveParticles;

    private GameObject smallEnemy;
    private GameObject boss;

    private GameObject ExitDoor;

    private DirectionalLight directionalLight;

    private MessageHandler m_messageHandler;

    private GameObject ActionSender;

	void Start () {

        publicVariableHolderNeverUnload = GameObject.Find("PublicVariableHolderNeverUnload").GetComponent<PublicVariableHolderneverUnload>();

        if (GetComponent<HealthController>() == true)
        {
            totalHealth = GetComponent<HealthController>().TotalHealth;
            currentHealth = GetComponent<HealthController>().CurrentHealth;
        }

        girlPlayer = GameObject.FindGameObjectWithTag("Player/Girl");
        boyPlayer = GameObject.FindGameObjectWithTag("Player/Boy");

        DungeonDoorFloor1Boy = publicVariableHolderArenaEntrance.DungeonDoorFloor1Boy;
        DungeonDoorFloor1Girl = publicVariableHolderArenaEntrance.DungeonDoorFloor1Girl;

        BoyPosBoss = publicVariableHolderArenaEntrance.BoyPosBoss;
        GirlPosBoss = publicVariableHolderArenaEntrance.GirlPosBoss;

        girlSpriteGameobject = GameObject.FindGameObjectWithTag("Sprite/Girl");
        boySpriteGameobject = GameObject.FindGameObjectWithTag("Sprite/Boy");

        LightRoom = publicVariableHolderArenaEntrance.LightsBossRoom;
        LightInteractiveParticles = publicVariableHolderArenaEntrance.LightInteractiveParticles;

        BossPos = publicVariableHolderArenaEntrance.BossPos;

        EnemiesSpawnPos = publicVariableHolderArenaEntrance.EnemiesSpawnPos;

        smallEnemy = publicVariableHolderArenaEntrance.smallEnemies;
        boss = publicVariableHolderArenaEntrance.boss;

        ExitDoor = publicVariableHolderArenaEntrance.ExitDoor;

        directionalLight = publicVariableHolderArenaEntrance.directionalLight;

        m_messageHandler = GetComponent<MessageHandler>();

        if (m_messageHandler)
        {
            m_messageHandler.RegisterDelegate(RecieveMessage);
        }
	}


    public void DoAction(GameObject sender)
    {
        ActionSender = sender;
        Action(Case);
    }

    private void Action(int c)
    {
        /*Alex : Case 1 is for Dungeon floor 1 door;
         * 
         * 
         */

        switch(c)
        {
         
            case 0:

                StartCoroutine("DungeonFloor1Door");
                break; 

            case 1:

                StartCoroutine("DungeonFloor1Boss");
                break;

            case 2:

                DungeonFloor1Exit();
                break;

            case 3:

                DungeonFloor1Torches();
                break;
        }   
    }

    void RecieveMessage(MessageTypes msgType, GameObject go, MessageData msgData)
    {
        switch (msgType)
        {
            case MessageTypes.DAMAGED:
                DamageData dmgData = msgData as DamageData;

                if (gameObject.tag == "Objects")
                {
                    if (dmgData != null)
                    {
                        ApplyDamage(dmgData.damage, go);
                    }
                }
                break;
        }
    }

    public void ApplyDamage(int damage, GameObject go)
    {
        currentHealth -= damage;
    }

    IEnumerator DungeonFloor1Door()
    {
        EventManager.TriggerEvent("InCombat");
        EventManager.TriggerEvent("StopMoving");

        Vector3 BoyNewPos = new Vector3(DungeonDoorFloor1Boy.transform.position.x, boyPlayer.transform.position.y, DungeonDoorFloor1Boy.transform.position.z);
        Vector3 GirlNewPos = new Vector3(DungeonDoorFloor1Girl.transform.position.x, girlPlayer.transform.position.y, DungeonDoorFloor1Girl.transform.position.z);

        boyPlayer.GetComponent<NavMeshAgent>().SetDestination(DungeonDoorFloor1Boy.transform.position);
        girlPlayer.GetComponent<NavMeshAgent>().SetDestination(DungeonDoorFloor1Girl.transform.position);

        while (boyPlayer.transform.position!= BoyNewPos && girlPlayer.transform.position != GirlNewPos)
        {
            yield return null; 
        }

        yield return new WaitForSeconds(.5f);
        boyPlayer.GetComponent<BetterPlayer_Movement>().SetCurTarget(this.gameObject);
        Debug.Log(currentHealth);
        yield return new WaitUntil(() => currentHealth <=0);
        Debug.Log("It did work");
        currentHealth = 0;

        if(_DestroyEffect)
        {
            ScreenFader.PlayLaserDevastation();
            yield return new WaitForSeconds(.5f);
            GameObject sfx = Instantiate(_DestroyEffect, transform.position, Quaternion.identity);
            Destroy(sfx, 5f);
        }

        SpriteRenderer[] DoorSprite = Door.GetComponentsInChildren<SpriteRenderer>();
        for (int i = 0; i < DoorSprite.Length; i++)
        {
            DoorSprite[i].enabled = false;
        }

        Destroy(Door);

        EventManager.TriggerEvent("NotInCombat");
    }

    IEnumerator DungeonFloor1Boss()
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

        Debug.Log(boynav.velocity);
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

    void DungeonFloor1Exit()
    {
        EventManager.TriggerEvent("InCombat");
        EventManager.TriggerEvent("StopMoving");

        boyPlayer.GetComponent<NavMeshAgent>().SetDestination(new Vector3(ExitDoor.transform.position.x, boyPlayer.transform.position.y, ExitDoor.transform.position.z));
        girlPlayer.GetComponent<NavMeshAgent>().SetDestination(new Vector3(ExitDoor.transform.position.x, girlPlayer.transform.position.y, ExitDoor.transform.position.z));

        publicVariableHolderNeverUnload.PlayerUI.SetActive(false);
    }

    private void OnDestroy()
    {
        ChangeRoom[] cghroom = this.GetComponentsInParent<ChangeRoom>();
        for (int i = 0; i < cghroom.Length; i++)
        {
            cghroom[i].ResetChangeRoom();
        }
        EventManager.TriggerEvent("StartMoving");
    }

    private void DungeonFloor1Torches()
    {
        this.gameObject.GetComponent<TorchesBoss>().StartCoroutine("LightUpCoroutine", ActionSender);
    }
}
