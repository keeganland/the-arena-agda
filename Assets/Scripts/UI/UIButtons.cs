using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButtons : MonoBehaviour
{
    Button[] buttons;

    private void Awake()
    {
        buttons = GetComponentsInChildren<Button>();
    }

    private void OnEnable()
    {
        EventManager.StartListening("HiddeUIButtons", HiddeUIButtons);
        EventManager.StartListening("ShowUIButtons", ShowUIButtons);
    }

    private void OnDisable()
    {
        EventManager.StopListening("HiddeUIButtons", HiddeUIButtons);
        EventManager.StopListening("ShowUIButtons", ShowUIButtons);

    }

    void HiddeUIButtons()
    {
        foreach (Button but in buttons)
        {
            but.gameObject.SetActive(false);
        }
    }

    void ShowUIButtons()
    {
        foreach (Button but in buttons)
        {
            but.gameObject.SetActive(true);
        }

    }
}
