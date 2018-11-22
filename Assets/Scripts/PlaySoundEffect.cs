using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundEffect : MonoBehaviour {

    public AudioClip _SFXOnInstentiate;
    public AudioClip _SFXOnLoop;
    public AudioClip _SFXOnHitEnemy;
    public AudioClip _SFXOnHitObject;

    public bool PlayLoop = false;
    public bool isSFX;
    public bool isBackgroundMusic;

    private AudioSource m_audioSource;

    public float ScaleFactor; //Alex : It helps scaling the sound. That footsteps sfx are not as high as magic spells, etc...

	// Use this for initialization
	void Awake () 
    {
        m_audioSource = GetComponent<AudioSource>();

        SoundManager.onSoundChangedCallback += UpdateSound;
    }
    private void Start()
    {
        if (isSFX)
        {
            m_audioSource.volume = (SoundManager.SFXVolume * ScaleFactor) / 100;
        }
        if (isBackgroundMusic)
        {
            m_audioSource.volume = (SoundManager.BackgroundMusicVolume * ScaleFactor) / 100;
        }
    }

    private void OnEnable()
    {
        if(_SFXOnInstentiate)
        {
            m_audioSource.PlayOneShot(_SFXOnInstentiate);
        }
    }

    public void OnHitSFXEnemy()
    {
        if(_SFXOnHitEnemy)
        {
            m_audioSource.PlayOneShot(_SFXOnHitEnemy);
        }
    }

    public void OnHitObject()
    {
        if(_SFXOnHitObject)
        {
            m_audioSource.PlayOneShot(_SFXOnHitObject);
        }
    }

    // Update is called once per frame
    void Update () 
    {
        if (_SFXOnLoop)
        {
            if (PlayLoop)
            {
                if (!m_audioSource.isPlaying)
                {
                    m_audioSource.PlayOneShot(_SFXOnLoop);
                    if (!m_audioSource.loop)
                        m_audioSource.loop = true;
                }
            }
            else
            {
                m_audioSource.loop = false;
            }
        }
	}

    void UpdateSound()
    {
        if(isSFX)
        {            
            m_audioSource.volume = (SoundManager.SFXVolume*ScaleFactor)/100;
        }
        if(isBackgroundMusic)
        {
            m_audioSource.volume = (SoundManager.BackgroundMusicVolume*ScaleFactor)/100;
        }
    }
}
