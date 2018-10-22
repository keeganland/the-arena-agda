using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TorchesBoss : MonoBehaviour {

    public Slider TorchSlider;
    public Animator LightAnim;
    public ParticleSystem LightInteractionParticle;

    float time;
    bool lightdown;
    bool lightup;

    public float _Timeup;
    public float _Timedown;

	void Update ()
    {
        if (lightup)
        {
            time += Time.deltaTime;

            if (_Timeup - time >= 0)
            {
                TorchSlider.value = (_Timeup - time);
            }
            else
            {
                LightDown();
            }

        }
        if (lightdown)
        {
            time += Time.deltaTime;

            if (_Timedown - time >= 0)
            {
                TorchSlider.value = (_Timeup - time);
            }
            else
            {
                LightWaiting();
            }
        }
	}

    public void LightUp()
    {
        time = 0;

        LightInteractionParticle.Stop();

        TorchSlider.gameObject.SetActive(true);
        TorchSlider.value = _Timeup;

        lightup = true;
        lightdown = false;
    
        LightAnim.SetBool("TurnOff", false);
        LightAnim.SetBool("TurnOn", true);
    }

    private void LightDown()
    {
        time = 0;

        TorchSlider.gameObject.SetActive(true);
        TorchSlider.value = _Timedown;
    
        lightup = false;
        lightdown = true;

        LightAnim.SetBool("TurnOff", true);
        LightAnim.SetBool("TurnOn", false);
    }

    private void LightWaiting()
    {
        lightup = false;
        lightdown = false;

        TorchSlider.gameObject.SetActive(false);

        LightInteractionParticle.Play();
    }
}
