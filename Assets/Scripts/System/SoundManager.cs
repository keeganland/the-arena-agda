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
    private bool exitScene;
    private bool enterScene;

    [SerializeField] private float scaleFactor = 0.65f;
    [SerializeField] private float exitSpeed = 0.1f;
    private float time = 0;

    #region Getters and Setters

    public float ScaleFactor
    {
        set { scaleFactor = value; }
    }
    public float ExitSpeed
    {
        set { exitSpeed = value; }
    }
    #endregion

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
            backgroundMusicSource.volume = (BackgroundMusicVolume * scaleFactor) / 100;

            if (onSoundChangedCallback != null)
            {
                onSoundChangedCallback.Invoke();
            }
        }

        //Debug.Log(SFXVolumeSlider.isActiveAndEnabled);

        if (SFXVolumeSlider.isActiveAndEnabled)
        {
            SFXVolume = (int)SFXVolumeSlider.value;
            sfxVolumeText.text = SFXVolumeSlider.value.ToString();

            if (onSoundChangedCallback != null)
            {
                onSoundChangedCallback.Invoke();
            }
        }

        if(exitScene)
        {
            AudioSource backgroundMusicSource = gameObject.GetComponent<AudioSource>();

            if (time < 1)
            {             
                backgroundMusicSource.volume = Mathf.Lerp(backgroundMusicSource.volume, 0, time);
                time += Time.deltaTime * exitSpeed;
            }
            if(backgroundMusicSource.volume <= Mathf.Pow(10, -6))
            {
                backgroundMusicSource.Stop();
                time = 0;
                exitScene = false;
            }
        }
        if (enterScene)
        {
            AudioSource backgroundMusicSource = gameObject.GetComponent<AudioSource>();

            if (time < 1)
            {
                backgroundMusicSource.volume = Mathf.Lerp(0, scaleFactor, time);           
                time += Time.deltaTime * exitSpeed;
            }
            else
            {
                enterScene = false;
                time = 0;
            }

            if (backgroundMusicSource.isPlaying == false)
            {
                backgroundMusicSource.Play();
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
        backgroundMusicChanged = false;
    }

    public static void ExitScene()
    {
        Instance.exitScene = true;
        Instance.enterScene = false;
        Instance.time = 0;
    }

    public static void EnterScene()
    {
        Instance.exitScene = false;
        Instance.enterScene = true;
        Instance.time = 0;
    }
}
