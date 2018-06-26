using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightIntensity : MonoBehaviour {

    private Light _Light;
    public float _MaximumIntensity;
    public float _Range;
    public float _SpeedIntensity;

	void Start () {
        _Light = GetComponent<Light>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        _Light.intensity = Mathf.Lerp(0, _MaximumIntensity, _SpeedIntensity * Time.deltaTime);
	}
}
