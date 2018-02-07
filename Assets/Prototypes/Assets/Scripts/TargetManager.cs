using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{


    //TrackingSystem m_tracker;
    //ShootingSystem m_shooter;
    RangeChecker m_range;

    // Use this for initialization
    void Start()
    {
        //m_tracker = GetComponent<TrackingSystem>();
        //m_shooter = GetComponent<ShootingSystem>();
        m_range = GetComponent<RangeChecker>();
    }

    // Update is called once per frame
    void Update()
    {
        if (/*!m_tracker || !m_shooter ||*/ !m_range)
            return;

        Target(curTarget);
    }
    public GameObject Target(GameObject curTarget)
    {
        List<GameObject> validTargets = m_range.GetValidTargets();

        for(int i = 0; i < m_range.Count; i++)
        {
            if (curTarget == validTargets[i])
            {
                return curTarget;
            }
        }

        

        //m_tracker.SetTarget(curTarget);
        //m_shooter.SetTarget(curTarget);
        }