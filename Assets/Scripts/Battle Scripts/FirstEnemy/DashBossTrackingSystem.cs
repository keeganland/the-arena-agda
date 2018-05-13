using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashBossTrackingSystem : MonoBehaviour {

    public Transform[] target;

	public int boyOrGirl = (int)Random.Range(0, 1);
	
	// Update is called once per frame
	void Update () {

		if (boyOrGirl == 0) {
			transform.LookAt (target[0]);
		} else {
			transform.LookAt (target[1]);
		}

		boyOrGirl = (int)Random.Range(0, 1);
	}


}
