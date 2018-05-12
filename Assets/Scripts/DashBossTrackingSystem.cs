using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Take2 : MonoBehaviour {

	public Transform[] target = {0, 1};

	public int boyOrGirl = (int)Random.Range(0, 1);
	
	// Update is called once per frame
	void Update () {

		if (boyOrGirl == 0) {
			transform.LookAt (target[1]);
		} else {
			transform.LookAt (target[2]);
		}

		boyOrGirl = (int)Random.Range(0, 1);
	}
}
