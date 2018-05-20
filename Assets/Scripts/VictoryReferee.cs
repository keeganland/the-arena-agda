﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VictoryReferee : MonoBehaviour {


    private UnityAction victoryAction;
    public GameObject victoryUI;
    public VictoryCondition vc;

    void Awake()
    {
        victoryAction = new UnityAction(Victory);
    }
    void OnEnable()
    {
        EventManager.StartListening("victory", victoryAction);
    }
    void OnDisable()
    {
        EventManager.StopListening("victory", victoryAction);
    }

    public void Victory()
    {
        Debug.Log("You won!");
        victoryUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResetGame()
    {
        victoryUI.SetActive(false);
        Time.timeScale = 1f;

        AnyManager.anyManager.ResetGame();
    }
}
