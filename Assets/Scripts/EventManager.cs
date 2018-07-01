/*
 * Stealing from : https://unity3d.com/learn/tutorials/topics/scripting/events-creating-simple-messaging-system
 * 
 */


/* This script has the major advantage of decreasing dependencies but the major disadvantage of it becoming hard to keep track of
 * what "events" actually exist. I don't think visual studio has any feature that will make this easier - but look into it. 
 * Events correspond to keys. Therefore I'm going to keep here a record of what keys are used for the dictionary and where:
 * VictoryReferee.cs
 *  "victory"
 *  "bossDied"
 * HealthController.cs
 *  "bossDied"
 *  "checkVictory"
 *  
 * This list is far from finished, unforunately. 
 * 
 *  //Keegan note 2018/7/1: searching for alex's enterArena calls
 * StartFirstBattle.cs
 *  "enterArena"
 *  
 * 
 * */


using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class EventManager : MonoBehaviour
{
    //originally was: 
    //private Dictionary  eventDictionary;
    //Changed later constructor call accordingly
    private Dictionary<string, UnityEvent> eventDictionary;

    private static EventManager eventManager;

    //Note: original had lowercase instance throughout. Auto-suggest said capitalize as I all throughout
    /* Keegan NTS: I'm so not used to C# singleton patterns! But that's what's going on here
     * Refer to: https://msdn.microsoft.com/en-us/library/ff650316.aspx
     * 
     */
    public static EventManager Instance
    {
        get
        {
            if (!eventManager)
            {
                eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;

                if (!eventManager)
                {
                    Debug.LogError("There needs to be one active EventManger script on a GameObject in your scene.");
                }
                else
                {
                    eventManager.Init();
                }
            }
            return eventManager;
        }
    }

    void Init()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<string, UnityEvent>();
        }
    }

    public static void StartListening(string eventName, UnityAction listener)
    {
        UnityEvent thisEvent = null;
        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listener);
            Instance.eventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(string eventName, UnityAction listener)
    {
        if (eventManager == null) return;
        UnityEvent thisEvent = null;
        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void TriggerEvent(string eventName)
    {
        UnityEvent thisEvent = null;
        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke();
        }
    }
}
