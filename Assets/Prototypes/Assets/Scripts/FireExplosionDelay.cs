using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireExplosionDelay : MonoBehaviour {

    public float _Speed;

    SphereCollider _Collider;
    float timer = 0;

	// Use this for initialization
	void Start () {
        _Collider = GetComponent<SphereCollider>();
	}
	
	// Update is called once per frame
	void Update () {
        if (timer >= 2.6f)
        {
            _Collider.radius = Mathf.Lerp(_Collider.radius, 100f, _Speed * Time.deltaTime);
        }

        timer += Time.deltaTime;

        if (_Collider.radius >= 99f)
        {
            _Collider.enabled = false;
            Destroy(gameObject, 1.5f);
        }
	}
}
