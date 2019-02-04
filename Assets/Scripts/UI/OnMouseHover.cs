using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnMouseHover : MonoBehaviour {

    Image image;
    AudioSource m_audioSource;

    [SerializeField] private AudioClip MouseHover;

    public float ScaleFactor = 0.05f;
	// Use this for initialization
	void Start () 
    {
        image = GetComponent<Image>();
        MouseHover = Resources.Load("SFX/ui_button_simple_click_01") as AudioClip;
        m_audioSource = GetComponent<AudioSource>();
        SoundManager.onSoundChangedCallback += UpdateSound;
    }

    public void MouseOver()
    {
        image.enabled = true;
        m_audioSource.PlayOneShot(MouseHover);
    }

    public void MouseExit()
    {
        image.enabled = false;
    }

    void UpdateSound()
    {
        if(m_audioSource)
       m_audioSource.volume = (SoundManager.SFXVolume * ScaleFactor) / 100;
    }
}
