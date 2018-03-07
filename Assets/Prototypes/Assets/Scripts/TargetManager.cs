using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    public enum TargetOptions { WEAKEST, STRONGEST, NEAREST, FARTHEST, RANDOM }
    public TargetOptions targetOption = TargetOptions.NEAREST;

    RangeChecker m_range;

    //set this up to get rid of error. would like to pass this into targetmanager instead
    private GameObject curTarget = null;

    // Use this for initialization
    void Start()
    {
        m_range = GetComponent<RangeChecker>();
    }

    // Update is called once per frame
    void Update()
    {
        //check to make sure there's something in range. Should probably switch to if(curtarget==null)
        if (/*!m_tracker || !m_shooter ||*/ !m_range)
            return;

        switch (targetOption)
        {
            case TargetOptions.FARTHEST:
                curTarget = TargetFarthest();
                this.GetComponent<EnemyMovement>().TargetSet(curTarget);
                break;
            case TargetOptions.NEAREST:
                curTarget = TargetNearest();
                this.GetComponent<EnemyMovement>().TargetSet(curTarget);
                break;
            case TargetOptions.RANDOM:
                curTarget = TargetRandom(curTarget);
                this.GetComponent<EnemyMovement>().TargetSet(curTarget);
                break;
            case TargetOptions.STRONGEST:
                curTarget = TargetStrongest();
                this.GetComponent<EnemyMovement>().TargetSet(curTarget);
                break;
            case TargetOptions.WEAKEST:
                curTarget = TargetWeakest();
                this.GetComponent<EnemyMovement>().TargetSet(curTarget);
                break;
        }
    }
    private GameObject TargetFarthest()
    {
        List<GameObject> validTargets = m_range.GetValidTargets();

        GameObject curTarget = null;
        float farthestDistance = 0.0f;

        for(int i = 0; i < validTargets.Count; i++)
        {
            float dist = Vector3.Distance(transform.position, validTargets[i].transform.position);
            if(!curTarget || dist > farthestDistance)
            {
                curTarget = validTargets[i];
                farthestDistance = dist;
            }
        }
        return curTarget;
    }
    private GameObject TargetNearest()
    {
        List<GameObject> validTargets = m_range.GetValidTargets();

        GameObject curTarget = null;
        float closestDistance = 0.0f;

        for (int i = 0; i < validTargets.Count; i++)
        {
            float dist = Vector3.Distance(transform.position, validTargets[i].transform.position);
            if (!curTarget || dist < closestDistance)
            {
                curTarget = validTargets[i];
                closestDistance = dist;
            }
        }
        return curTarget;
    }
    private GameObject TargetRandom(GameObject go)
    {
        if (go = null)
        {
            List<GameObject> validTargets = m_range.GetValidTargets();
            int num = Random.Range(0, validTargets.Count);
            GameObject curTarget = validTargets[num];
        }
        return curTarget;
    }
    private GameObject TargetStrongest()
    {
        List<GameObject> validTargets = m_range.GetValidTargets();

        GameObject curTarget = null;
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
        return curTarget;
    }
    private GameObject TargetWeakest()
    {
        List<GameObject> validTargets = m_range.GetValidTargets();

        GameObject curTarget = null;
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
        return curTarget;
    }
}
        