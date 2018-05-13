﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstEnemyAttack : MonoBehaviour {

    [Header("Targeting and Attacks Data")]
    public Transform[] _Target;
    public int _BoyOrGirl;

    public float _AttackCD;
    public float _WarningtoAttackCD;

    private bool m_isAttacking;
    private float m_timer; 

    [Header("Linerenderers Variables")]

    private LineRenderer lineRenderer;
    private float counter;
    private float dist;
    private bool m_linedrawing;

    public float lineDrawSpeed = 6f;


    // Use this for initialization
    void Start ()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetWidth(.45f, .45f);
        lineRenderer.SetVertexCount(2);
        lineRenderer.sortingOrder = 10;
    }
	
	// Update is called once per frame
	void Update ()
    {
		if (!m_isAttacking && m_timer >= _AttackCD)
        {
            TrackingSystem();
            m_isAttacking = true;
            StartCoroutine("DashAttack");
        }

        if (m_linedrawing)
        {
                //get the unit vector in the desired direction, multiply by the desired length and add the starting point
                //Vector3 pointAlongLine = x * Vector3.Normalize(pointB - pointA) + pointA;

                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, _Target[_BoyOrGirl].transform.position);
                Debug.Log(_Target[_BoyOrGirl].transform.position);
        }
	}

    private void TrackingSystem ()
    {
           _BoyOrGirl = Random.Range(0, 2);
    }

    private IEnumerator DashAttack()
    {
        //Look toward target and draw "warning" line
        transform.LookAt(_Target[_BoyOrGirl]);
        m_linedrawing = true;

        yield return new WaitForSeconds(_WarningtoAttackCD); 
    }
}

