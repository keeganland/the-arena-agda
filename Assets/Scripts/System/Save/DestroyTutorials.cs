using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTutorials : MonoBehaviour {

    SaveManager saveManager;

    private void Awake()
    {
        saveManager = FindObjectOfType<SaveManager>();
    }
    // Use this for initialization

    private void OnEnable()
    {
        if (saveManager.tutorials.Contains(name))
        {
            Destroy(gameObject);
        }

        saveManager.tutorials.Add(name);
    }
	
	void Update () 
    {
        if(Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0))
        {
            Destroy(gameObject);
        }
	}
}
