using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePathScript : MonoBehaviour {

	public string pathName;
	public float interval;
	public AudioClip soundFX;

	[HideInInspector]
	public List<Transform> destinationPoints;

	void Start () {
		//gameObject.transform.position = destinationPoints [0].position;
		FollowTweenPath (interval);
		if (soundFX != null && GetComponent<AudioSource>()) {
			GetComponent<AudioSource> ().PlayOneShot (soundFX);
		}
	}

	//itween is fantastic, makes simple animations in only a line of code. Here i simply "fade in" the movement from the first destination point to the second,
	//and then "fade out" to the last destination point
	IEnumerator FollowPath (float waitTime) {
		for (int i = 0; i < destinationPoints.Count; i++) {
			if(i==0)
				iTween.MoveTo (gameObject, iTween.Hash ("easetype", "easeInCubic", "lookahead", 1, "position", destinationPoints[i].position, "time", waitTime) );
			else if(i==destinationPoints.Count)
				iTween.MoveTo (gameObject, iTween.Hash ("easetype", "easeOutCubic", "lookahead", 1, "position", destinationPoints[i].position, "time", waitTime) );
			else
				iTween.MoveTo (gameObject, iTween.Hash ("easetype", "linear", "lookahead", 1, "position", destinationPoints[i].position, "time", waitTime) );
				
			yield return new WaitForSeconds (waitTime);
		}
	}

	public void FollowTweenPath (float interval) {

		iTween.MoveTo (gameObject, iTween.Hash ("path", iTweenPath.GetPath(pathName), "easetype", iTween.EaseType.easeInOutSine, "time", interval));
	}

}
