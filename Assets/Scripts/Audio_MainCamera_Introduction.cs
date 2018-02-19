using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio_MainCamera_Introduction : MonoBehaviour {

    public AudioClip otherClip;


    IEnumerator Start()
    {
        AudioSource audio = GetComponent<AudioSource>();

        yield return new WaitForSeconds((otherClip.length));
        audio.Play();
    }

}
