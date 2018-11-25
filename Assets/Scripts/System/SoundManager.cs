using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour {

    private static SoundManager soundManager;

    private static AudioClip backgroundMusic;
    private static bool backgroundMusicChanged;

    public static int BackgroundMusicVolume = 70;
    public static int SFXVolume = 70;

    public Slider BackgroundMusicSlider;
    public Slider SFXVolumeSlider;

    public Text backgroundMusicSliderText;
    public Text sfxVolumeText;

    public delegate void OnSoundChanged();
    public static OnSoundChanged onSoundChangedCallback;
    public static bool ExitScene;
    public static bool EnterScene;

    public float ScaleFactor = 0.65f;
    public float ExitSpeed = 0.1f;
    private float time = 0;

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

    private void Start()
    {
        if (onSoundChangedCallback != null)
        {
            onSoundChangedCallback.Invoke();
        }
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

            AudioSource backgroundMusicSource = gameObject.GetComponent<AudioSource>();
            backgroundMusicSource.volume = (BackgroundMusicVolume * ScaleFactor) / 100;
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


        if(ExitScene)
        {
            AudioSource backgroundMusicSource = gameObject.GetComponent<AudioSource>();

            if (time < 1)
            {             
                backgroundMusicSource.volume = Mathf.Lerp(backgroundMusicSource.volume, 0, time);
                Debug.Log(time);
                time += Time.deltaTime * ExitSpeed;
            }
            if(backgroundMusicSource.volume <= Mathf.Pow(10, -4))
            {
                backgroundMusicSource.Stop();
                backgroundMusicSource.volume = ScaleFactor;
                time = 0;
                ExitScene = false;
            }
        }
        if (EnterScene)
        {
            AudioSource backgroundMusicSource = gameObject.GetComponent<AudioSource>();

            if (time < 1)
            {
                backgroundMusicSource.volume = Mathf.Lerp(0, ScaleFactor, time);           
                time += Time.deltaTime * ExitSpeed;
            }
            else
            {
                EnterScene = false;
                time = 0;
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
