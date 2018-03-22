using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour {

    private Rigidbody rb;
    private GameObject spellCaster;

    [SerializeField]
    private float speed;
    public GameObject target;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 direction = target.transform.position - transform.position;

         transform.position = Vector3.Lerp(this.transform.position, target.transform.position ,speed * Time.fixedDeltaTime);

        float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
        Debug.Log(angle);

        transform.eulerAngles = new Vector3(0, -angle, 0);
    }

    public void SetTarget(GameObject passedTarget)
    {
        target = passedTarget;
    }
}


