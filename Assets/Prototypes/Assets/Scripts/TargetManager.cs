using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    public enum TargetOptions { WEAKEST, STRONGEST, NEAREST, FARTHEST, RANDOM }
    public TargetOptions targetOption /*= TargetOptions.NEAREST*/;//This was in the example script, don't think I need it

    RangeChecker m_range;
    EnemyMovement m_enemyM;
    float closestDistance = 0.0f;
    float farthestDistance = 0.0f;

    //set this up to get rid of error. would like to pass this into targetmanager instead
    //this should be private, making public to try to fix error in enemymovement
    public GameObject curTarget = null;

    // Use this for initialization
    void Start()
    {
        m_range = GetComponentInChildren<RangeChecker>();
        m_enemyM = GetComponent<EnemyMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        //check to make sure there's something in range.
        if (!m_range)
            return;

        switch (targetOption)
        {
            case TargetOptions.FARTHEST:
                TargetFarthest();
                break;
            case TargetOptions.NEAREST:
                TargetNearest();
                break;
            case TargetOptions.RANDOM:
                TargetRandom(curTarget);
                break;
            case TargetOptions.STRONGEST:
                TargetStrongest();
                break;
            case TargetOptions.WEAKEST:
                TargetWeakest();
                break;
        }
    }
    private void TargetFarthest()
    {
        List<GameObject> validTargets = m_range.GetValidTargets();
        bool curTargetinRange;

        for(int i =0; i<validTargets.Count; i++)
        {
            curTargetinRange = false;
            if(validTargets[i] == curTarget)
            {
                curTargetinRange = true;
                break;
            }
            if(curTargetinRange == false)
            {
                curTarget = null;
            }
        }
        if(validTargets.Count == 0)
        {
            curTarget = null;
        }
        for(int i = 0; i < validTargets.Count; i++)
        {
            float dist = Vector3.Distance(transform.position, validTargets[i].transform.position);
            if(!curTarget || dist > farthestDistance || curTarget == validTargets[i])
            {
                curTarget = validTargets[i];
                farthestDistance = dist;
            }
        }
        m_enemyM.TargetSet(curTarget);
        Debug.Log("TargetManager/FarthestTarget: " + curTarget.name);
    }
    private void TargetNearest()
    {
        List<GameObject> validTargets = m_range.GetValidTargets();

        for (int i = 0; i < validTargets.Count; i++)
        {
            float dist = Vector3.Distance(transform.position, validTargets[i].transform.position);
           // Debug.Log(dist);
            if (!curTarget || dist < closestDistance || curTarget == validTargets[i])
            {
                //Debug.Log("in loop");
                curTarget = validTargets[i];
                closestDistance = dist;
                Debug.Log(closestDistance);
            }
        }
        m_enemyM.TargetSet(curTarget);
        //Debug.Log("TargetManager/NearestTarget: " + curTarget.name);
        //Debug.Log(validTargets.Count);
    }
    private void TargetRandom(GameObject go)
    {
        if (!go)
        {
            List<GameObject> validTargets = m_range.GetValidTargets();//only want trigger stay on this one
            int num = Random.Range(0, validTargets.Count);
            curTarget = validTargets[num];
        }
        m_enemyM.TargetSet(curTarget);
        if (curTarget != null)
        {
            Debug.Log("TargetManager/RandomTarget: " + curTarget.name);
        }
    }
    private void TargetStrongest()
    {
        List<GameObject> validTargets = m_range.GetValidTargets();

        int highestHealth = 0;

        for(int i = 0; i < validTargets.Count; i++)
        {
            int hp = validTargets[i].GetComponent<HealthController>().totalHealth;
            if(!curTarget || hp > highestHealth)
            {
                highestHealth = hp;
                curTarget = validTargets[i];
            }
        }
        m_enemyM.TargetSet(curTarget);
        Debug.Log("TargetManager/StrongestTarget: " + curTarget.name);
    }
    private void TargetWeakest()
    {
        List<GameObject> validTargets = m_range.GetValidTargets();

        int lowestHealth = 0;

        for (int i = 0; i < validTargets.Count; i++)
        {
            int hp = validTargets[i].GetComponent<HealthController>().totalHealth;
            if (!curTarget || hp < lowestHealth)
            {
                lowestHealth = hp;
                curTarget = validTargets[i];
            }
        }
        m_enemyM.TargetSet(curTarget);
        Debug.Log("TargetManager/WeakestTarget: " + curTarget.name);
    }
}
        