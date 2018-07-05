using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DestroyableObjects : MonoBehaviour {

    private int totalHealth;
    private int currentHealth;
    public GameObject _DestroyEffect;
    public GameObject Door;

    private MessageHandler m_messageHandler;

	void Start () {

        totalHealth = GetComponent<HealthController>().totalHealth;
        currentHealth = GetComponent<HealthController>().currentHealth;
        m_messageHandler = GetComponent<MessageHandler>();

        if (m_messageHandler)
        {
            m_messageHandler.RegisterDelegate(RecieveMessage);
        }
	}

    void RecieveMessage(MessageTypes msgType, GameObject go, MessageData msgData)
    {
        switch (msgType)
        {
            case MessageTypes.DAMAGED:
                DamageData dmgData = msgData as DamageData;

             
                        ApplyDamage(dmgData.damage, go);
               
                break;
        }
    }

    public void ApplyDamage(int damage, GameObject go)
    {
        currentHealth -= damage;
        if (currentHealth <= 0f)
        {
            StartCoroutine("Death");
        }

        if (m_messageHandler)
        {
            HealthData hpData = new HealthData();
            hpData.maxHealth = totalHealth;
            hpData.curHealth = currentHealth;

            m_messageHandler.GiveMessage(MessageTypes.HEALTHCHANGED, gameObject, hpData);
        }
    }

    private IEnumerator Death()
    {
        currentHealth = 0;

        if (_DestroyEffect)
        {
            GameObject.Find("ScreenFader").GetComponent<Animator>().Play("LaserAttackDevastation");
            yield return new WaitForSeconds(0.5f);
            GameObject sfx = Instantiate(_DestroyEffect, transform.position, Quaternion.identity);
            Destroy(sfx, 5f);
        }
        Destroy(Door, 0.5f);
    }
}
