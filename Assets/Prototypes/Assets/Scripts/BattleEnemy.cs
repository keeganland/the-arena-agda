using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleEnemy : MonoBehaviour {

    private float m_timer = 0;

    public List<GameObject> Attacks;
    public List<float> _Attacktimers;
    public float _CooldownTimer;

    private TargetRangeChecker m_range;

    public void Start()
    {
        m_range = GetComponentInChildren<TargetRangeChecker>();
        MessageHandler msgHandler = GetComponent<MessageHandler>();

        if (msgHandler)
        {
            msgHandler.RegisterDelegate(RecieveMessage);
        }

    }

    void RecieveMessage(MessageTypes msgType, GameObject go, MessageData msgData)
    {
        switch (msgType)
        {
            case MessageTypes.HEALTHCHANGED:
                HealthData hpData = msgData as HealthData;

                if (hpData != null)
                {
                    NormalAttackBoy(hpData.curHealth);
                    NormalAttackGirl(hpData.curHealth);
                    UltimateAttack(hpData.curHealth, hpData.maxHealth);
                    ChangeColor(hpData.curHealth);
                }
                break;
        }   
    }

    void NormalAttackGirl(int currhealth)
    {
        if(IsDivisble(currhealth, 10))
        {
            
        }
    }

    void NormalAttackBoy(int currhealth)
    {
        if(IsDivisble(currhealth, 20))
        {
            
        }
    }

    void UltimateAttack(int currhealth, int maxhealth)
    {
        if ((float)currhealth/(float)maxhealth > 0.4 && (float) currhealth / (float)maxhealth < 0.55
            ||  (float) currhealth / (float)maxhealth > 0.10 && (float)currhealth / (float)maxhealth < 0.15)
        {
            
        }
                                                                                                
    }

    void ChangeColor(int currhealth)
    {
        
    }

    public bool IsDivisble(int x, int n)
    {
        return (x % n) == 0;
    }

}
