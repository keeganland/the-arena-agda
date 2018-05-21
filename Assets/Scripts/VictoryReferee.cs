using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VictoryReferee : MonoBehaviour {


    private UnityAction victoryAction;
    public GameObject victoryUI;

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

    public void Update()
    {
        Debug.Log("victoryUI is active? == " + victoryUI.activeSelf);
    }

    public void Victory()
    {
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
