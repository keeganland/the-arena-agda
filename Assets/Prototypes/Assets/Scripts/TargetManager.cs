using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{


    //TrackingSystem m_tracker;
    //ShootingSystem m_shooter;
    RangeChecker m_range;

    //set this up to get rid of error. would like to pass this into targetmanager instead
    public GameObject curTarget=null;

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
        //check to make sure there's something in range. Should probably switch to if(curtarget==null)
        if (/*!m_tracker || !m_shooter ||*/ !m_range)
            return;

        Target(curTarget);
    }
    public GameObject Target(GameObject curTarget)
    {
        //getting list of targets
        List<GameObject> validTargets = m_range.GetValidTargets();

        //return curTarget as target if within range
        //why did I do this? Is this to check if it's within range?
        for (int i = 0; i < validTargets.Count; i++)
        {
            if (curTarget == validTargets[i])
            {
                return curTarget;
            }
        }

        return null;
    }
}


        

        //m_tracker.SetTarget(curTarget);
        /*m_shooter.SetTarget(curTarget);*/
        