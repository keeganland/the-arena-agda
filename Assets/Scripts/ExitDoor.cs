using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : MonoBehaviour {

    public PublicVariableHolderArenaEntrance publicVariableHolderArenaEntrance;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            StartCoroutine("ExitDoorC");
         }
    }

    private IEnumerator ExitDoorC()
    {
        ScreenFader.fadeOut();
        publicVariableHolderArenaEntrance.publicVariableHolderNeverUnload.PlayerUI.SetActive(false);
        yield return new WaitForSeconds(2);
        publicVariableHolderArenaEntrance.publicVariableHolderNeverUnload.VictoryCanvas.SetActive(true);
    }
}
