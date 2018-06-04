/*
 * Keegan Todo:
 * 
 * We might want this to be an abstract class for different types of spells. 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour {

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
        if (target)
        {
            if (target.activeSelf == false)
            {
                Destroy(gameObject, 1);
            }
        }
        else
            Destroy(gameObject);
    }
    private void FixedUpdate()
    {
        if (!isCollided)
        {
            if (target)
            {
                Vector3 direction = target.transform.position - transform.position;

                transform.position = Vector3.Lerp(this.transform.position, target.transform.position, speed * Time.fixedDeltaTime);

                float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
                transform.eulerAngles = new Vector3(0, -angle, 0);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == target)
        {
            isCollided = true;
            transform.SetParent(collision.transform);
        }
    }

    public void SetTarget(GameObject passedTarget)
    {
        target = passedTarget;
    }
}


