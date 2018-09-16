using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class InitialSetUpDungeonFloor1 : InitialSceneSetup {

    private GameObject SpawnPosBoy;
    private GameObject SpawnPosGirl;

    new void Start()
    {
        base.Start();

        SpawnPosBoy = publicArenaEntrance.SpawnPosBoy;
        SpawnPosGirl = publicArenaEntrance.SpawnPosGirl;

        Boy.transform.position = new Vector3(SpawnPosBoy.transform.position.x, transform.position.y, SpawnPosBoy.transform.position.z);
        Girl.transform.position = new Vector3(SpawnPosGirl.transform.position.x, transform.position.y, SpawnPosGirl.transform.position.z);

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

        Boy.GetComponent<BetterPlayer_Movement>().isCombat = false; //To change when the scripted event will be there
        Girl.GetComponent<BetterPlayer_Movement>().isCombat = false;

        Boy.GetComponent<HealthController>().Sprite.GetComponent<Animator>().SetBool("Death", true);
        Boy.GetComponent<HealthController>().Sprite.GetComponent<Animator>().Play("Death");
        Girl.GetComponent<HealthController>().Sprite.GetComponent<Animator>().SetBool("Death", true);
        Girl.GetComponent<HealthController>().Sprite.GetComponent<Animator>().Play("Death");

        yield return new WaitForSeconds(.1f);

        StartCoroutine(ArrivalDialogue());
    }

    IEnumerator ArrivalDialogue()
    {
        yield return new WaitForSeconds(2f);

        GetComponent<TextFloor1Arrival>().PlayerEnableText(true);

        while(GameObject.Find("TextBoxManager").GetComponent<TextBoxManager>().eventStart != true)
        {
            yield return null;
        }

        GameObject.Find("TextBoxManager").GetComponent<TextBoxManager>().eventStart = false;

        Boy.GetComponent<HealthController>().Sprite.GetComponent<Animator>().SetBool("Death", false);
        Girl.GetComponent<HealthController>().Sprite.GetComponent<Animator>().SetBool("Death", false);


        //yield return new WaitUntil(() => Input.anyKeyDown == true);
    }
}
