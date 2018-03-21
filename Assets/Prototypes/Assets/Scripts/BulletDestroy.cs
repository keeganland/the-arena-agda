using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDestroy : MonoBehaviour {

    void OnCollisionEnter (Collision collision)
    {
        Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("wall"))
        {
            Destroy(gameObject, 2.0f);
        }
    }
}
