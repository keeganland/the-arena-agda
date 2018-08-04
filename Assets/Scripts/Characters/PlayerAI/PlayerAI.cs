using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerAI : MonoBehaviour {
    
    public enum AiStates { NEAREST, FURTHEST, WEAKEST, STRONGEST }

    public AiStates aiState = AiStates.NEAREST;

    BetterPlayer_Movement m_player;

    public bool hasTarget;
    public bool AIavailable;
    // Use this for initialization
    void Start()
    {
        m_player = GetComponent<BetterPlayer_Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (AIavailable)
        {
            if (!hasTarget)
            {
                switch (aiState)
                {
                    case AiStates.NEAREST:
                        TargetNearest();
                        break;
                    case AiStates.FURTHEST:
                        TargetFurthest();
                        break;
                    case AiStates.WEAKEST:
                        TargetWeakest();
                        break;
                    case AiStates.STRONGEST:
                        TargetStrongest();
                        break;
                }
            }
        }
    }

    void TargetNearest()
    {
        GameObject[] validTargets = GameObject.FindGameObjectsWithTag("Enemy"); //KeeganNote 2018/8/1 - Replace with some sort of priority queue

        GameObject curTarget = null;

        float closesDist = 0.0f;

        for (int i = 0; i < validTargets.Length; i++)
        {
            float dist = Vector3.Distance(transform.position, validTargets[i].transform.position);

            if (!curTarget || dist < closesDist)
            {
                curTarget = validTargets[i];
                closesDist = dist;
            }
        }
        m_player.SetCurTarget(curTarget);
    }

    void TargetFurthest()
    {
        GameObject[] validTargets = GameObject.FindGameObjectsWithTag("Enemy");

        GameObject curTarget = null;
        float furthestDist = 0.0f;

        for (int i = 0; i < validTargets.Length; i++)
        {
            float dist = Vector3.Distance(transform.position, validTargets[i].transform.position);

            if (!curTarget || dist > furthestDist)
            {
                curTarget = validTargets[i];
                furthestDist = dist;
            }
        }
        m_player.SetCurTarget(curTarget);
    }

    void TargetWeakest()
    {
        GameObject[] validTargets = GameObject.FindGameObjectsWithTag("Enemy");

        GameObject curTarget = null;
        int lowestHealth = 0;

        for (int i = 0; i < validTargets.Length; i++)
        {
            int maxHp = validTargets[i].GetComponent<HealthController>().totalHealth;

            if (!curTarget || maxHp < lowestHealth)
            {
                lowestHealth = maxHp;
                curTarget = validTargets[i];
            }
        }
        m_player.SetCurTarget(curTarget);
    }

    void TargetStrongest()
    {
        GameObject[] validTargets = GameObject.FindGameObjectsWithTag("Enemy");

        GameObject curTarget = null;
        int highestHealth = 0;

        for (int i = 0; i < validTargets.Length; i++)
        {
            int maxHp = validTargets[i].GetComponent<HealthController>().totalHealth;

            if (!curTarget || maxHp > highestHealth)
            {
                highestHealth = maxHp;
                curTarget = validTargets[i];
            }
        }
        m_player.SetCurTarget(curTarget);
    }
}
