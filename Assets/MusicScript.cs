using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicScript : MonoBehaviour {

    public AudioClip _BackgroundMusicIntro;
    private AudioSource m_audioSource;

	// Use this for initialization
	void Start () {
        m_audioSource = GetComponent<AudioSource>();
        m_audioSource.PlayOneShot(_BackgroundMusicIntro);
	}
	
	// Update is called once per frame

}
