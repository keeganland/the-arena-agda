using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

        StartCoroutine(ArrivalDialogue());
	}

    IEnumerator ArrivalDialogue()
    {
        yield return new WaitForSeconds(2f);

        GetComponent<TextFloor1Arrival>().PlayerEnableText(true);

        //yield return new WaitUntil(() => Input.anyKeyDown == true);
    }
}
