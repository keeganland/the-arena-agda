using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.Events;


public class InitialSetUpDungeonFloor1 : InitialSceneSetup {

    private GameObject SpawnPosBoy;
    private GameObject SpawnPosGirl;

    private GameObject ScriptedPosBoyIntro;
    private SpriteScript2 Egnevy;
    private SpriteScript2 Eva;

    private UnityAction victoryEvent;

    private void Awake()
    {
        victoryEvent = new UnityAction(VictoryEvent);
        saveManager = FindObjectOfType<SaveManager>();
    }

    private void OnEnable()
    {
        EventManager.StartListening("victoryEvent", VictoryEvent);
    }

    private void OnDisable()
    {
        EventManager.StopListening("victoryEvent", VictoryEvent);
    }


    new void Start()
    {
        base.Start();

        SpawnPosBoy = publicArenaEntrance.SpawnPosBoy;
        SpawnPosGirl = publicArenaEntrance.SpawnPosGirl;
        ScriptedPosBoyIntro = publicArenaEntrance.ScriptedPosBoyIntro;

        Boy.transform.position = new Vector3(SpawnPosBoy.transform.position.x, transform.position.y, SpawnPosBoy.transform.position.z);
        Girl.transform.position = new Vector3(SpawnPosGirl.transform.position.x, transform.position.y, SpawnPosGirl.transform.position.z);

        Boy.GetComponent<BetterPlayer_Movement>().SwapGirl();
        Girl.GetComponent<BetterPlayer_Movement>().SwapGirl();


        Egnevy = Boy.GetComponent<HealthController>().Sprite.GetComponent<SpriteScript2>();
        Eva = Girl.GetComponent<HealthController>().Sprite.GetComponent<SpriteScript2>();

        MainCamera = publicArenaEntrance.MainCamera;

        MainCamera.GetComponent<BetterCameraFollow>().SetFieldOfView(MainCameraFieldOfViewMin, MainCameraFieldOfViewMax);

        publicArenaEntrance.publicVariableHolderNeverUnload.fader.StartCoroutine("FadeIn");

        StartCoroutine(ArrivalScriptedEvent());

        VictoryReferee.ResetEnemyList();
        VictoryReferee.SetVictoryCondition(2);  
	}

    IEnumerator ArrivalScriptedEvent()
    {
        Boy.GetComponent<NavMeshAgent>().enabled = true;
        Girl.GetComponent<NavMeshAgent>().enabled = true;

        Girl.GetComponent<BetterPlayer_Movement>().IsCombat = false;
        Girl.GetComponent<BetterPlayer_Movement>().CancelParticles();


        Boy.GetComponent<HealthController>().Sprite.GetComponent<Animator>().SetBool("Death", true);
        Boy.GetComponent<HealthController>().Sprite.GetComponent<Animator>().Play("Death");
        Girl.GetComponent<HealthController>().Sprite.GetComponent<Animator>().SetBool("Death", true);
        Girl.GetComponent<HealthController>().Sprite.GetComponent<Animator>().Play("Death");

        yield return new WaitForSeconds(5f);

        Boy.GetComponent<HealthController>().Sprite.GetComponent<Animator>().SetBool("Death", false);

        Egnevy.ForcePlayerRotation(4);
        yield return new WaitForSeconds(0.5f);
        Egnevy.ForcePlayerRotation(3);
        yield return new WaitForSeconds(1.5f);
        Egnevy.ForcePlayerRotation(1);
        yield return new WaitForSeconds(1.5f);
        Egnevy.ForcePlayerRotation(2);
        yield return new WaitForSeconds(1f);
        Boy.GetComponent<NavMeshAgent>().SetDestination(new Vector3(ScriptedPosBoyIntro.transform.position.x, transform.position.y, ScriptedPosBoyIntro.transform.position.z));
        while(Boy.transform.position != new Vector3(ScriptedPosBoyIntro.transform.position.x, Boy.transform.position.y, ScriptedPosBoyIntro.transform.position.z))
        {
            yield return null;
        }

                //Debug.Log("here");
        Egnevy.ForcePlayerRotation(4);
        yield return new WaitForSeconds(0.5f);
        Egnevy.ForcePlayerRotation(3);
        yield return new WaitForSeconds(1.5f);
        Boy.GetComponent<BetterPlayer_Movement>().IsCombat = false;
        yield return new WaitForSeconds(1f);
        Girl.GetComponent<HealthController>().Sprite.GetComponent<Animator>().SetBool("Death", false);
        Eva.ForcePlayerRotation(4);
        yield return new WaitForSeconds(0.5f);
        Eva.ForcePlayerRotation(3);
        yield return new WaitForSeconds(1.5f);
        Eva.ForcePlayerRotation(1);
        yield return new WaitForSeconds(0.5f);
        Eva.ForcePlayerRotation(3);

        EventManager.TriggerEvent("StartMoving");

        StartCoroutine(ArrivalDialogue());
    }

    IEnumerator ArrivalDialogue()
    {
        GetComponent<TextFloor1Arrival>().PlayerEnableText(true);

        while(GameObject.Find("TextBoxManager").GetComponent<TextBoxManager>().eventStart != true)
        {
            yield return null;
        }

        GameObject.Find("TextBoxManager").GetComponent<TextBoxManager>().eventStart = false;

        Girl.GetComponent<HealthController>().Sprite.GetComponent<Animator>().SetBool("Death", false);

        //yield return new WaitUntil(() => Input.anyKeyDown == true);
    }

    private void VictoryEvent()
    {
        EventManager.StopListening("victoryEvent", VictoryEvent);
        EventManager.TriggerEvent("NotInCombat");
        EventManager.TriggerEvent("StopMoving");

        StartCoroutine(VictoryEventCoroutine());
    }

    private IEnumerator VictoryEventCoroutine()
    {
        float t = 0.0f;
        MainCamera.GetComponent<BetterCameraFollow>().enabled = false;

        //Debug.Log(new Vector3(publicArenaEntrance.Floor2Door.gameObject.transform.position.x, MainCamera.transform.position.y, publicArenaEntrance.Floor2Door.gameObject.transform.position.z));

        float a = publicArenaEntrance.Floor2Door.gameObject.transform.position.x;
        float c = publicArenaEntrance.Floor2Door.gameObject.transform.position.z;

        while(Mathf.Abs(MainCamera.transform.position.x - a) > .1f && Mathf.Abs(MainCamera.transform.position.z - c) > .1f)
        {
            MainCamera.transform.position = Vector3.Lerp(MainCamera.transform.position, new Vector3(publicArenaEntrance.Floor2Door.gameObject.transform.position.x, MainCamera.transform.position.y, publicArenaEntrance.Floor2Door.gameObject.transform.position.z), t*0.02f);
            MainCamera.GetComponent<Camera>().orthographicSize = Mathf.Lerp(MainCamera.GetComponent<Camera>().orthographicSize, 6f, t*0.02f);
            //Debug.Log(t);
            t += Time.deltaTime;
            //Debug.Log(MainCamera.transform.position);
            yield return null;
        }
        EventManager.TriggerEvent("camTargetRefresh");
        publicArenaEntrance.Floor2Door.Play("OpenDoorFloor2");

        yield return new WaitForSeconds(1.5f);
        MainCamera.GetComponent<BetterCameraFollow>().enabled = true;
        yield return new WaitForSeconds(0.5f);

        publicArenaEntrance.ExitDoor.SetActive(true);

        EventManager.TriggerEvent("StartMoving");
    }
}
