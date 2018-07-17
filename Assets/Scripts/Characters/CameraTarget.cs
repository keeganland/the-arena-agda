using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour {

    private void Awake()
    {
        EventManager.TriggerEvent("camTargetRefresh");
    }

    private void OnDisable()
    {
        EventManager.TriggerEvent("camTargetRefresh");
    }
    private void OnDestroy()
    {
        EventManager.TriggerEvent("camTargetRefresh");
    }
}
