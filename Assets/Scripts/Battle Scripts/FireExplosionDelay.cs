using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireExplosionDelay : MonoBehaviour {

    public float _Speed;
    public int _Radius = 110;
    public float _Delay;

    SphereCollider _Collider;
    float timer = 0;
    bool iscollider;

	// Use this for initialization
	void Start () {
        _Collider = GetComponent<SphereCollider>();
	}
	
	// Update is called once per frame
	void Update () {
        if (timer >= _Delay)
        {
            if (!_Collider.enabled && !iscollider)
            {
                _Collider.enabled = true;
                iscollider = true;
            }

            _Collider.radius = Mathf.Lerp(_Collider.radius, _Radius, _Speed * Time.deltaTime);
        }

        timer += Time.deltaTime;

        if (_Collider.radius >= _Radius - 1)
        {
           _Collider.enabled = false;
           Destroy(gameObject, 1.5f);
        }
	}
}
