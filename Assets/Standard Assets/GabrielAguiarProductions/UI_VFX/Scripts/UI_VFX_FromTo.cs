using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_VFX_FromTo : MonoBehaviour {

	public IncreaseMoneyScript moneyScript;
	public GameObject clickedVFX;
	public GameObject glowVFX;
	public GameObject UIvfx;
	public GameObject origin;
	public GameObject destination;
	public iTween.EaseType easeType;
	public float time;
	public float rate;
	public float amount;
	public Vector3 offset;

	public void SpawnClickedVFX (){
		if (clickedVFX != null) {
			var vfx = Instantiate (clickedVFX, origin.transform.position, Quaternion.identity) as GameObject;
			vfx.transform.SetParent (origin.transform);
			var ps = vfx.GetComponent<ParticleSystem> ();
			if(ps!= null)
				Destroy (vfx, ps.main.duration + ps.main.startLifetime.constantMax);
			if (vfx.transform.childCount > 0) {
				for(int i = 0; i< vfx.transform.childCount; i++){
					var psChild = vfx.transform.GetChild (i).GetComponent<ParticleSystem> ();
					if(psChild != null){
						Destroy (vfx, psChild.main.duration + psChild.main.startLifetime.constantMax);
						break;
					}
				}
			}
		}
		if (glowVFX != null) {
			var ps = glowVFX.GetComponent<ParticleSystem> ();
			if(ps != null){
				ps.Stop ();
			}
			if (glowVFX.transform.childCount > 0) {
				for(int i = 0; i< glowVFX.transform.childCount; i++){
					var psChild = glowVFX.transform.GetChild (i).GetComponent<ParticleSystem> ();
					if(psChild != null){
						psChild.Stop ();
					}
				}
			}
		}
	}

	public void FromTo (){
		if (UIvfx != null) {
			StartCoroutine (FromToCo ());
		}
	}

	IEnumerator FromToCo () {		
		for (int i = 0; i < amount; i++){
			moneyScript.NumberIncreaseCaller (10, 0.5f);
			var vfx = Instantiate (UIvfx, origin.transform.position, Quaternion.identity) as GameObject;
			vfx.transform.SetParent (origin.transform);
			iTween.MoveTo (vfx, iTween.Hash("position", destination.transform.position + offset, "easetype", easeType, "ignoretimescale", true, "time", time));
			Destroy (vfx, time + 1);
			yield return new WaitForSeconds (rate);
		}

		if (glowVFX != null) {
			var ps = glowVFX.GetComponent<ParticleSystem> ();
			if(ps != null){
				ps.Play ();
			}
			if (glowVFX.transform.childCount > 0) {
				for(int i = 0; i< glowVFX.transform.childCount; i++){
					var psChild = glowVFX.transform.GetChild (i).GetComponent<ParticleSystem> ();
					if(psChild != null){
						psChild.Play ();
					}
				}
			}
		}
	}
}
