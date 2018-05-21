using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashCollider : MonoBehaviour {

    private Rigidbody rb;
    private GameObject spellCaster;
    private bool isCollided;
    private int AggroValue;

    [SerializeField]
    private float speed;
    public Vector3 target;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (!isCollided)
        {
            Vector3 direction = target - transform.position;
            transform.position = Vector3.Lerp(this.transform.position, target, speed * Time.fixedDeltaTime);
       
        }
    }

    public void SetTarget(Vector3 passedTarget)
    {
        target = passedTarget;
    }
}
