using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyFlare : MonoBehaviour {

    public int _Time;

	// Use this for initialization
	void Start () {
        Destroy(gameObject, _Time);
	}
}
