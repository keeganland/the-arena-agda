using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopCamera : MonoBehaviour {

    public PublicVariableHolderneverUnload _PublicVariableHolder;

    private void OnEnable()
    {
        EventManager.StartListening("StopCamera", StopCameraMovement);
        EventManager.StartListening("StartCamera", StartCameraMovement);
    }

    private void OnDisable()
    {
        EventManager.StopListening("StopCamera", StopCameraMovement);
        EventManager.StopListening("StartCamera", StartCameraMovement);
    }

    void StopCameraMovement()
    {
        _PublicVariableHolder.StopCamera = true;
    }
    void StartCameraMovement()
    {
        _PublicVariableHolder.StopCamera = false;
    }
}
