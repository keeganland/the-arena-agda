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

    public PublicVariableHolderArenaEntrance publicVariableHolderArenaEntrance;

    public Slider TorchSlider;
    public Light Light;
    public ParticleSystem LightInteractionParticle;
    public GameObject PointLight;
    public Text Text;

    float time;
    float lightAnimTime;
    bool lightdown;
    bool lightup;

    private GameObject LightCollider;
    private GameObject LightCollidergo;

    public float _Timeup;
    public float _Timedown;

    public int Intensity;

    [SerializeField] int LightingTime = 2;

    void Update ()
    {
        if (lightup)
        {
            time += Time.deltaTime;  
            Text.text = ((int)(_Timeup - time)).ToString();

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
            Text.text = ((int)(_Timedown - time)).ToString();
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
        LightInteractionParticle.Stop();

        TorchSlider.gameObject.SetActive(true);
        TorchSlider.value = _Timeup;

        lightup = true;
        lightdown = false;
        gameObject.GetComponentInChildren<CapsuleCollider>().enabled = true; //turns on collider for dark damam
        Debug.Log(gameObject.GetComponentInChildren<CapsuleCollider>().gameObject.name);
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
        gameObject.GetComponentInChildren<CapsuleCollider>().enabled = false; //Turns off collider for dark damage
    }

    private void LightWaiting()
    {
        gameObject.GetComponent<BoxCollider>().enabled = true;
        Text.text = "";
        PointLight.SetActive(true);
        PointLight.GetComponent<Animator>().SetBool("FadeIn", true);
        PointLight.GetComponent<Animator>().SetBool("Lighten", true);
        TorchSlider.gameObject.SetActive(false);

        LightInteractionParticle.Play();

        lightup = false;
        lightdown = false;
    }

    public IEnumerator LightUpCoroutine(GameObject sender)
    {
        gameObject.GetComponent<BoxCollider>().enabled = false;

        sender.GetComponent<SpriteInteractionSlider>().SetUpInteractionSlider("Lighting", (float)LightingTime);

        yield return new WaitForSeconds(LightingTime);
        LightUp();
    }
}
