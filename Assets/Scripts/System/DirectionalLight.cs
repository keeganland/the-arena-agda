using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalLight : MonoBehaviour {

    public int intensity;

    private Light m_light;
    private float time;

    private bool lightUp;
    private bool lightDown;
	// Use this for initialization
	void Start () 
    {
        m_light = GetComponent<Light>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if(lightUp || lightDown)
        {
            time += Time.deltaTime;
        }
        if(lightUp)
        {
            m_light.intensity = Mathf.Lerp(0, intensity, time); //Lerp the intensity toward the wanted intensity
        }
        else if(lightDown)
        {
            m_light.intensity = Mathf.Lerp(intensity, 0, time); //Lerp the intensity toward 0
        }
        if(time > 1)
        {
            lightUp = false;
            lightDown = false;
        }

	}

    public void LightUp() //Set up bools and conditions for the intensity
    {
        time = 0;
        lightUp = true;
    }

    public void LightDown()
    {
        time = 0;
        lightDown = true;
    }
}
