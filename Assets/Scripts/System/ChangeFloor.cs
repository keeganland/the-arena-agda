using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChangeFloor : MonoBehaviour {

    private bool isBossCleared;

    private void OnEnable()
    {
      EventManager.StartListening("bossDied", bossDeathAction);
    }
    private void OnDisable()
    {

        EventManager.StopListening("bossDied", bossDeathAction);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && isBossCleared)
        {
            StartCoroutine("ChangeFloorCoroutine");  
        }
    }

    private IEnumerator ChangeFloorCoroutine()
    {
        ScreenFader fade = GameObject.FindWithTag("Fader").GetComponent<ScreenFader>();
        fade.StartCoroutine("FadeOut");

        GameObject PlayerUI = GameObject.Find("PlayerUI");
        PlayerUI.SetActive(false);

        yield return new WaitForSeconds(10);
    }
    private void bossDeathAction()
    {
        isBossCleared = true;
    }
    }

        
