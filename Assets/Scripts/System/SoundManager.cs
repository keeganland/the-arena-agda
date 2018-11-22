using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour {

    private static SoundManager soundManager;

    private static AudioClip backgroundMusic;
    private static bool backgroundMusicChanged;

    public static int BackgroundMusicVolume;
    public static int SFXVolume;

    public Slider BackgroundMusicSlider;
    public Slider SFXVolumeSlider;

    public Text backgroundMusicSliderText;
    public Text sfxVolumeText;

    public delegate void OnSoundChanged();
    public static OnSoundChanged onSoundChangedCallback;

    public static SoundManager Instance
    {
        get
        {
            if (!soundManager)
            {
                soundManager = FindObjectOfType(typeof(SoundManager)) as SoundManager;

                if (!soundManager)
                {
                    Debug.LogError("There needs to be one active EventManger script on a GameObject in your scene.");
                }
                else
                {
                    soundManager.Init();
                }
            }
            return soundManager;
        }
    }

    void Init()
    {
    }

    private void Update()
    {
        if(backgroundMusicChanged)
        {
            BackgroundMusic();
        }

        if(BackgroundMusicSlider.isActiveAndEnabled)
        {
            BackgroundMusicVolume = (int) BackgroundMusicSlider.value;
            backgroundMusicSliderText.text = BackgroundMusicSlider.value.ToString();
            if (onSoundChangedCallback != null)
            {
                onSoundChangedCallback.Invoke();
            }
        }
        if (SFXVolumeSlider.isActiveAndEnabled)
        {
            SFXVolume = (int)SFXVolumeSlider.value;
            sfxVolumeText.text = SFXVolumeSlider.value.ToString();

            if (onSoundChangedCallback != null)
            {
                onSoundChangedCallback.Invoke();
            }
        }
    }

    public static void SetBackgroundMusic(AudioClip music)
    {
        backgroundMusic = music;
        backgroundMusicChanged = true;
    }

    void BackgroundMusic()
    {
        AudioSource backgroundMusicSource = gameObject.GetComponent<AudioSource>(); 
        backgroundMusicSource.clip = backgroundMusic;
        backgroundMusicSource.Play();
        backgroundMusicChanged = false;
    }
}
