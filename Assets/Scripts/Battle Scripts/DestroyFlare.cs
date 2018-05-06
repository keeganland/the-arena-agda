using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyFlare : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Destroy(gameObject, 4f);
	}
}
