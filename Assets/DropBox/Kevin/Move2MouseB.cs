using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move2MouseB : MonoBehaviour
{

    [SerializeField] Transform target;
    float speed = 1f;
    Vector2 targetPos;
    Animator anim;
    public bool charSwitch;

    // Use this for initialization
    void Start()
    {
        targetPos = transform.position;
        anim = GetComponent<Animator>();
        charSwitch = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (charSwitch == false)
        {
            if (Input.GetKeyDown("tab"))
            {
                charSwitch = true;
            }

            if (Input.GetMouseButtonDown(1))
            {
                targetPos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
                target.position = targetPos;
            }

            if ((Vector2)transform.position != targetPos)
            {
                anim.SetBool("iswalking", true);
                anim.SetFloat("input_x", (targetPos.x - transform.position.x));
                anim.SetFloat("input_y", (targetPos.y - transform.position.y));
                transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
            }
            else
            {
                anim.SetBool("iswalking", false);
            }
        }
    }
}
