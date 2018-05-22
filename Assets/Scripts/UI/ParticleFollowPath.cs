using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleFollowPath : MonoBehaviour {

    public string _PathName;
    public float _Time;

	// Use this for initialization
	void Start () {
        iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPath(_PathName), "easetype", iTween.EaseType.easeInOutSine, "time", _Time));
	}
	
	
}
