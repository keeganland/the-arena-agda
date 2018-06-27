using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleFollowPath : MonoBehaviour {

	public string pathName;
	public float time;

	void Start () {
		iTween.MoveTo (gameObject, iTween.Hash ("path", iTweenPath.GetPath (pathName), "easetype", iTween.EaseType.easeInOutSine, "time", time));
	}
}
