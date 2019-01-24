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

        boyPlayer.transform.position = new Vector3(SpawnPosBoy.transform.position.x, transform.position.y, SpawnPosBoy.transform.position.z);
        girlPlayer.transform.position = new Vector3(SpawnPosGirl.transform.position.x, transform.position.y, SpawnPosGirl.transform.position.z);

        boyPlayer.GetComponent<BetterPlayer_Movement>().SwapGirl();
        girlPlayer.GetComponent<BetterPlayer_Movement>().SwapGirl();


        Egnevy = boyPlayer.GetComponent<HealthController>().Sprite.GetComponent<SpriteScript2>();
        Eva = girlPlayer.GetComponent<HealthController>().Sprite.GetComponent<SpriteScript2>();

        MainCamera = Camera.main.gameObject;

        MainCamera.GetComponent<BetterCameraFollow>().SetFieldOfView(MainCameraFieldOfViewMin, MainCameraFieldOfViewMax);

        publicArenaEntrance.publicVariableHolderNeverUnload.fader.StartCoroutine("FadeIn");

        StartCoroutine(ArrivalScriptedEvent());

        VictoryReferee.ResetEnemyList();
        VictoryReferee.SetVictoryCondition(2);  
	}

    IEnumerator ArrivalScriptedEvent()
    {
        boyPlayer.GetComponent<NavMeshAgent>().enabled = true;
        girlPlayer.GetComponent<NavMeshAgent>().enabled = true;

        girlPlayer.GetComponent<BetterPlayer_Movement>().IsCombat = false;
        girlPlayer.GetComponent<BetterPlayer_Movement>().CancelParticles();


        boyPlayer.GetComponent<HealthController>().Sprite.GetComponent<Animator>().SetBool("Death", true);
        boyPlayer.GetComponent<HealthController>().Sprite.GetComponent<Animator>().Play("Death");
        girlPlayer.GetComponent<HealthController>().Sprite.GetComponent<Animator>().SetBool("Death", true);
        girlPlayer.GetComponent<HealthController>().Sprite.GetComponent<Animator>().Play("Death");

        yield return new WaitForSeconds(5f);

        boyPlayer.GetComponent<HealthController>().Sprite.GetComponent<Animator>().SetBool("Death", false);

        Egnevy.ForcePlayerRotation(4);
        yield return new WaitForSeconds(0.5f);
        Egnevy.ForcePlayerRotation(3);
        yield return new WaitForSeconds(1.5f);
        Egnevy.ForcePlayerRotation(1);
        yield return new WaitForSeconds(1.5f);
        Egnevy.ForcePlayerRotation(2);
        yield return new WaitForSeconds(1f);
        boyPlayer.GetComponent<NavMeshAgent>().SetDestination(new Vector3(ScriptedPosBoyIntro.transform.position.x, transform.position.y, ScriptedPosBoyIntro.transform.position.z));
        while(boyPlayer.transform.position != new Vector3(ScriptedPosBoyIntro.transform.position.x, boyPlayer.transform.position.y, ScriptedPosBoyIntro.transform.position.z))
        {
            yield return null;
        }

        //Debug.Log("here");
        Egnevy.ForcePlayerRotation(4);
        yield return new WaitForSeconds(0.5f);
        Egnevy.ForcePlayerRotation(3);
        yield return new WaitForSeconds(1.5f);
        boyPlayer.GetComponent<BetterPlayer_Movement>().IsCombat = false;
        yield return new WaitForSeconds(1f);
        girlPlayer.GetComponent<HealthController>().Sprite.GetComponent<Animator>().SetBool("Death", false);
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
        //GetComponent<TextFloor1Arrival>().PlayerEnableText(true);
        GetComponent<TextFloor1Arrival>().Activate(); //Why bother with PlayerEnableText? See if this is an acceptable substitute
        while (TextBoxManager.Instance.EventStart != true)
        {
            yield return null;
        }

        TextBoxManager.Instance.EventStart = false;

        girlPlayer.GetComponent<HealthController>().Sprite.GetComponent<Animator>().SetBool("Death", false);

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
