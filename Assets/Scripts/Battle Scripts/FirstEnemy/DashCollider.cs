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
    public GameObject target;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (target.activeSelf == false)
        {
            Destroy(gameObject, 1);
        }
    }
    private void FixedUpdate()
    {
        if (!isCollided)
        {
            Vector3 direction = target.transform.position - transform.position;
            transform.position = Vector3.Lerp(this.transform.position, target.transform.position, speed * Time.fixedDeltaTime);
       
        }
    }

    public void SetTarget(GameObject passedTarget)
    {
        target = passedTarget;
    }
}
