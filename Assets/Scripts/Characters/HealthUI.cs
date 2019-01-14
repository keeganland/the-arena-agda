using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Alex : refactor 01/13
public class HealthUI : MonoBehaviour
{
    [SerializeField] private Slider slider;

    private int m_curHealth;
    private int m_maxHealth;

    void Awake()
    {
        m_maxHealth = this.gameObject.GetComponent<HealthController>().TotalHealth;
        m_curHealth = m_maxHealth;
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
                    m_curHealth = hpData.curHealth;
                    m_maxHealth = hpData.maxHealth;
                }
                break;
        }
    }

    public void UpdateUi(int maxHealth, int curHealth)
    {
        slider.value = (1.0f / maxHealth) * curHealth;
    }
}