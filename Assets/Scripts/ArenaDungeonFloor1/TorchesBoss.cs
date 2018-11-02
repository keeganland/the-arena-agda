using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* This Script contain all the interactions for the Torches in the Floor 1 Boss;
 * 
 * The player can light the Torch by calling the "LightUp" function from the "InteractiveObjects.cs". 
 * The Torch is going to be lightned for _Timeup amount of time before going in a "cooldown" phase for _Timedown amount of time. 
 * 
 * The Torch will then stay idle and wait the player to interact with it again.
 */

public class TorchesBoss : MonoBehaviour {

    public Slider TorchSlider;
    public Light Light;
    public ParticleSystem LightInteractionParticle;
    public GameObject PointLight;

    float time;
    float lightAnimTime;
    bool lightdown;
    bool lightup;

    public float _Timeup;
    public float _Timedown;

    public int Intensity;

	void Update ()
    {
        if (lightup)
        {
            time += Time.deltaTime;  
            
            if (_Timeup - time >= 0)
            {
                TorchSlider.value = (1 - time/_Timeup);
            }
            else
            {
                LightDown();
            }

            if (lightAnimTime <= 1.0f)
            {
                lightAnimTime += Time.deltaTime;
                Light.intensity = Mathf.Lerp(0, Intensity, lightAnimTime);
            }

        }
        if (lightdown)
        {
            time += Time.deltaTime;

            if (_Timedown - time >= 0)
            {
                TorchSlider.value = (1 - time/_Timedown);
            }
            else
            {
                LightWaiting();
            }

            if (lightAnimTime <= 1.0f)
            {
                lightAnimTime += Time.deltaTime;
                Light.intensity = Mathf.Lerp(Intensity, 0, lightAnimTime);
            }
        }
	}

    public void LightUp()
    {
        time = 0;
        lightAnimTime=0;
        gameObject.GetComponent<Collider>().enabled = false;
        LightInteractionParticle.Stop();

        TorchSlider.gameObject.SetActive(true);
        TorchSlider.value = _Timeup;

        lightup = true;
        lightdown = false;
    }

    private void LightDown()
    {
        time = 0;
        lightAnimTime = 0;

        PointLight.SetActive(false);
        TorchSlider.gameObject.SetActive(true);
        TorchSlider.value = _Timedown;
    
        lightup = false;
        lightdown = true;
    }

    private void LightWaiting()
    {
        gameObject.GetComponent<Collider>().enabled = true;

        PointLight.SetActive(true);
        PointLight.GetComponent<Animator>().SetBool("FadeIn", true);
        PointLight.GetComponent<Animator>().SetBool("Lighten", true);
        TorchSlider.gameObject.SetActive(false);

        LightInteractionParticle.Play();

        lightup = false;
        lightdown = false;
    }
}
