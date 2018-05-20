using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanMe : MonoBehaviour {


    public void Awake()
    {
    }
    public void OnEnable()
    {
        EventManager.StartListening("cleanup", CleanUp);
    }

    public void OnDisable()
    {
        EventManager.StopListening("cleanup", CleanUp);
    }
    public void CleanUp()
    {
        EventManager.StopListening("cleanup", CleanUp);
        Destroy(gameObject);
    }
}
