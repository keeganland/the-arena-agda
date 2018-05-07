using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashBossTrackingSystem : MonoBehaviour {

	public float speed = 100.0f;

	public GameObject targetPlayer = null; 

	Vector3 lastKnownPosition = Vector3.zero;

	Quaternion lookAtRotation;

//	public Transform target2;
	
	// Update is called once per frame
	void Update () {

		if (targetPlayer) {
			
			//if target gets lost, enemy will rotate to last position that target was
			if (lastKnownPosition != targetPlayer.transform.position) {
				lastKnownPosition = targetPlayer.transform.position;
				lookAtRotation = Quaternion.LookRotation (lastKnownPosition - transform.position);
			}

			if (transform.rotation != lookAtRotation) {
				transform.rotation = Quaternion.RotateTowards (transform.rotation, lookAtRotation, speed*Time.deltaTime);
			}
		}

//		if (targetPlayer) {
//
//			transform.LookAt (targetPlayer);
//		}
	}

	bool setTarget(GameObject target) {
		
		if (!target) {
			return false;

		} else {
			targetPlayer = target;
//			target2 = target;
			return true;
		}
	}
}
