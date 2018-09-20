using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : MonoBehaviour {
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            ScreenFader.fadeOut();
        }
    }

    private IEnumerator ExitDoorC()
    {
        ScreenFader.fadeOut();
        yield return new WaitForSeconds(1);
        FindObjectOfType<PublicVariableHolderneverUnload>().GetComponent<PublicVariableHolderneverUnload>().VictoryCanvas.SetActive(true);
    }
}
