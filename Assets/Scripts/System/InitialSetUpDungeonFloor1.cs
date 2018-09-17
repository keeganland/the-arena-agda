using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class InitialSetUpDungeonFloor1 : InitialSceneSetup {

    private GameObject SpawnPosBoy;
    private GameObject SpawnPosGirl;

    private GameObject ScriptedPosBoyIntro;
    private SpriteScript2 Egnevy;
    private SpriteScript2 Eva;

   

    new void Start()
    {
        base.Start();

        SpawnPosBoy = publicArenaEntrance.SpawnPosBoy;
        SpawnPosGirl = publicArenaEntrance.SpawnPosGirl;
        ScriptedPosBoyIntro = publicArenaEntrance.ScriptedPosBoyIntro;

        Boy.transform.position = new Vector3(SpawnPosBoy.transform.position.x, transform.position.y, SpawnPosBoy.transform.position.z);
        Girl.transform.position = new Vector3(SpawnPosGirl.transform.position.x, transform.position.y, SpawnPosGirl.transform.position.z);

        Egnevy = Boy.GetComponent<HealthController>().Sprite.GetComponent<SpriteScript2>();
        Eva = Girl.GetComponent<HealthController>().Sprite.GetComponent<SpriteScript2>();

        MainCamera = publicArenaEntrance.MainCamera;

        MainCamera.GetComponent<BetterCameraFollow>()._FieldOfViewMin = MainCameraFieldOfViewMin;
        MainCamera.GetComponent<BetterCameraFollow>()._FieldOfViewMax = MainCameraFieldOfViewMax;

        publicArenaEntrance.publicVariableHolderNeverUnload.PlayerUI.SetActive(false);

        publicArenaEntrance.publicVariableHolderNeverUnload.fader.StartCoroutine("FadeIn");

        StartCoroutine(ArrivalScriptedEvent());
	}

    IEnumerator ArrivalScriptedEvent()
    {
        Boy.GetComponent<NavMeshAgent>().enabled = true;
        Girl.GetComponent<NavMeshAgent>().enabled = true;

        Girl.GetComponent<BetterPlayer_Movement>().isCombat = false;

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
        Debug.Log("here");
        Egnevy.ForcePlayerRotation(4);
        yield return new WaitForSeconds(0.5f);
        Egnevy.ForcePlayerRotation(3);
        yield return new WaitForSeconds(1.5f);
        Boy.GetComponent<BetterPlayer_Movement>().isCombat = false;
        yield return new WaitForSeconds(1f);
        Girl.GetComponent<HealthController>().Sprite.GetComponent<Animator>().SetBool("Death", false);
        Eva.ForcePlayerRotation(4);
        yield return new WaitForSeconds(0.5f);
        Eva.ForcePlayerRotation(3);
        yield return new WaitForSeconds(1.5f);
        Eva.ForcePlayerRotation(1);
        yield return new WaitForSeconds(0.5f);
        Eva.ForcePlayerRotation(3);

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
}
