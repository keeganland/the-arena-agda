using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MessageData { };
public enum MessageTypes { DAMAGED, HEALTHCHANGED, DIED, AGGROCHANGED};
public delegate void MessageDelegate(MessageTypes msgType, GameObject go, MessageData data);

public class MessageHandler : MonoBehaviour
{

    public List<MessageTypes> messages;

    List<MessageDelegate> m_messageDelegates = new List<MessageDelegate>();

    public void RegisterDelegate(MessageDelegate msgDele)
    {
        m_messageDelegates.Add(msgDele);
    }

    public bool GiveMessage(MessageTypes msgType, GameObject go, MessageData msgData)
    {
        bool approuved = false;

        for (int i = 0; i < messages.Count; i++)
        {
            if (messages[i] == msgType)
            {
                approuved = true;
                break;
            }
        }

        if (!approuved)
            return false;

        for (int i = 0; i < m_messageDelegates.Count; i++)
        {
            m_messageDelegates[i](msgType, go, msgData);
        }

        return true;
    }

}
public class DamageData : MessageData
{
    public int damage;
}
public class DeathData : MessageData
{
    public GameObject attacker;
    public GameObject attacked;
}

public class HealthData : MessageData
{
    public int maxHealth;
    public int curHealth;
}
public class AggroData : MessageData
{
    public GameObject attacker;
    public GameObject attacked;
    public int aggro;
}