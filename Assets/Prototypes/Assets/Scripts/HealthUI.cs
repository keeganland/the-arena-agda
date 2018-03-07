using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public Slider slider;

    void Start()
    {
        MessageHandler msgHandler = GetComponent<MessageHandler>();

        if (msgHandler)
        {
            msgHandler.RegisterDelegate(RecieveMessage);
        }

        UpdateUi(1, 1);
    }

 
    void RecieveMessage(MessageTypes msgType, GameObject go, MessageData msgData)
    {
        switch (msgType)
        {
            case MessageTypes.HEALTHCHANGED:
                HealthData hpData = msgData as HealthData;

                if (hpData != null)
                {
                    UpdateUi(hpData.maxHealth, hpData.curHealth);
                }
                break;
        }
    }

    public void UpdateUi(int maxHealth, int curHealth)
    {
        slider.value = (1.0f / maxHealth) * curHealth;
    }
}