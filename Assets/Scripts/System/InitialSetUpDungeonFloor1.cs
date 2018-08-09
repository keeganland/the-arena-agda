using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        BoyNav.enabled = true;
        GirlNav.enabled = true;

        Boy.GetComponent<BetterPlayer_Movement>().isCombat = false;
        Girl.GetComponent<BetterPlayer_Movement>().isCombat = false;

        MainCamera = publicArenaEntrance.MainCamera;

        MainCamera.GetComponent<BetterCameraFollow>()._FieldOfViewMin = MainCameraFieldOfViewMin;
        MainCamera.GetComponent<BetterCameraFollow>()._FieldOfViewMax = MainCameraFieldOfViewMax;

        publicArenaEntrance.publicVariableHolderNeverUnload.PlayerUI.SetActive(false);

        publicArenaEntrance.publicVariableHolderNeverUnload.fader.StartCoroutine("FadeIn");   
	}
}
