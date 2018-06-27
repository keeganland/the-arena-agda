using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour {

	public int hits;
	public AudioClip charging;
	public GameObject leftHand;
	public GameObject rightHand;
	public GameObject path1StartPos;
	public GameObject path2StartPos;
	public GameObject firePoint;
	public GameObject handFX;
	public GameObject pathFX;
	public GameObject gatheringFX;
	public GameObject attackFX;
	public float delay;

	private Animator anim;

	void Start (){
		anim = GetComponent<Animator> ();
	}

	void Update (){
		if (hits <= 0) {
			anim.SetBool ("isIdle", false);
			anim.SetBool ("isDead", true);
		}
		if (Input.GetMouseButtonDown (0)) {
			anim.SetBool ("isIdle", false);
			anim.SetBool ("isAttacking", true);
			StartCoroutine (ChangeState(anim.GetCurrentAnimatorClipInfo(0).Length, "isIdle", true));
			StartCoroutine (ChangeState(anim.GetCurrentAnimatorClipInfo(0).Length, "isAttacking", false));

			GameObject leftHandFx = Instantiate (handFX, leftHand.transform.position, Quaternion.identity);
			leftHandFx.transform.SetParent (leftHand.transform);
			leftHandFx.transform.localScale = handFX.transform.localScale;
			Destroy (leftHandFx, leftHandFx.GetComponent<ParticleSystem> ().main.duration + leftHandFx.GetComponent<ParticleSystem> ().main.startLifetime.constantMax);

			GameObject rightHandFx = Instantiate (handFX, rightHand.transform.position, Quaternion.identity);
			rightHandFx.transform.SetParent (rightHand.transform);
			rightHandFx.transform.localScale = handFX.transform.localScale;
			Destroy (rightHandFx, rightHandFx.GetComponent<ParticleSystem> ().main.duration + rightHandFx.GetComponent<ParticleSystem> ().main.startLifetime.constantMax);

			GameObject gatherEffect = Instantiate (gatheringFX, firePoint.transform.position, Quaternion.identity);
			gatherEffect.transform.SetParent (firePoint.transform);
			gatherEffect.transform.localScale = gatheringFX.transform.localScale;
			gatherEffect.GetComponent<AudioSource> ().PlayOneShot (charging);
			Destroy (gatherEffect, gatherEffect.GetComponent<ParticleSystem> ().main.duration + gatherEffect.GetComponent<ParticleSystem> ().main.startLifetime.constantMax);

			GameObject pathEffect = Instantiate (pathFX, path1StartPos.transform.position, Quaternion.identity);
			Destroy (pathEffect, pathEffect.GetComponent<ParticleSystem> ().main.duration + pathEffect.GetComponent<ParticleSystem> ().main.startLifetime.constantMax);

			GameObject pathEffect2 = Instantiate (pathFX, path2StartPos.transform.position, Quaternion.identity);
			pathEffect2.GetComponent<ParticlePathScript> ().pathName = "Path02";
			Destroy (pathEffect2, pathEffect2.GetComponent<ParticleSystem> ().main.duration + pathEffect2.GetComponent<ParticleSystem> ().main.startLifetime.constantMax);

			StartCoroutine (NextEffect (delay));
		}
	}

	IEnumerator NextEffect (float waitTime) {
		
		yield return new WaitForSeconds (waitTime);

		Vector3 midPoint = (leftHand.transform.position + rightHand.transform.position) / 2;

		GameObject attackEffect = Instantiate (attackFX, midPoint, attackFX.transform.rotation);
		Destroy (attackEffect, attackEffect.GetComponent<ParticleSystem> ().main.duration + attackEffect.GetComponent<ParticleSystem> ().main.startLifetime.constantMax);
	}

	void OnCollisionEnter (Collision co) {
		
		if (co.gameObject.tag == "Bullet") {
			if (hits > 0) {
				anim.SetBool ("isIdle", false);
				anim.SetBool ("isHit", true);
				hits--;
				StartCoroutine (ChangeState(anim.GetCurrentAnimatorClipInfo(0).Length, "isIdle", true));
				StartCoroutine (ChangeState(anim.GetCurrentAnimatorClipInfo(0).Length, "isHit", false));
			}
		}
	}

	IEnumerator ChangeState(float waitTime, string stateToChange, bool cond){

		yield return new WaitForSeconds (waitTime);
		anim.SetBool (stateToChange, cond);
	}
}
