using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractiveObjectAbstract : MonoBehaviour {
    
    public bool isAttackable;

    protected PublicVariableHolderneverUnload publicVariableHolderNeverUnload;

    protected int totalHealth;
    protected int currentHealth;

    protected MessageHandler m_messageHandler;

    protected GameObject ActionSender;

    public bool canBeCancelled;
    public bool isCoroutineStarted;

	// Use this for initialization
	protected void Start () 
    {
        publicVariableHolderNeverUnload = GameObject.Find("PublicVariableHolderNeverUnload").GetComponent<PublicVariableHolderneverUnload>();

        if (GetComponent<HealthController>() == true)
        {
            totalHealth = GetComponent<HealthController>().totalHealth;
            currentHealth = GetComponent<HealthController>().currentHealth;
        }


        m_messageHandler = GetComponent<MessageHandler>();

        if (m_messageHandler)
        {
            m_messageHandler.RegisterDelegate(RecieveMessage);
        }
	}

    public abstract void DoAction(GameObject sender);
    public abstract IEnumerator Action(GameObject sender);
    public abstract void ActionFunction(GameObject sender);
    public abstract void CancelAction(GameObject sender);

    protected void RecieveMessage(MessageTypes msgType, GameObject go, MessageData msgData)
    {
        switch (msgType)
        {
            case MessageTypes.DAMAGED:
                DamageData dmgData = msgData as DamageData;

                if (gameObject.tag == "Objects")
                {
                    if (dmgData != null)
                    {
                        ApplyDamage(dmgData.damage, go);
                    }
                }
                break;
        }
    }

    public void ApplyDamage(int damage, GameObject go)
    {
        currentHealth -= damage;
    }

    protected void OnDestroy()
    {
        ChangeRoom[] cghroom = this.GetComponentsInParent<ChangeRoom>();
        for (int i = 0; i < cghroom.Length; i++)
        {
            cghroom[i].ResetChangeRoom();
        }
        EventManager.TriggerEvent("StartMoving");
    }
}

