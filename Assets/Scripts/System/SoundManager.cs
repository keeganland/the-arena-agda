using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour {

    private static SoundManager soundManager;

    private static AudioClip backgroundMusic;
    private static AudioClip backgroundSFX;
    private static bool backgroundMusicChanged;
    private static bool backgroundSFXChanged;

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

    private float backgroundSFXTime = 0;
    private bool fadeBackgroundSFXOut = false;
    private bool fadeBackgroundSFXIn = false;

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
        if (backgroundSFXChanged)
        {
            BackgroundSFX();
        }

        if (BackgroundMusicSlider.isActiveAndEnabled)
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

        if (fadeBackgroundSFXOut)
        {
            AudioSource[] backgroundSFXSource = gameObject.GetComponentsInChildren<AudioSource>();

            if (backgroundSFXTime < 1)
            {
                backgroundSFXSource[1].volume = Mathf.Lerp(backgroundSFXSource[1].volume, 0, backgroundSFXTime);
                backgroundSFXTime += Time.deltaTime * exitSpeed * 0.1f;
            }
            if (backgroundSFXSource[1].volume <= Mathf.Pow(10, -6))
            {
                backgroundSFXSource[1].Stop();
                backgroundSFXTime = 0;
                fadeBackgroundSFXOut = false;
            }
        }
        if (fadeBackgroundSFXIn)
        {
            AudioSource[] backgroundSFXSource = gameObject.GetComponentsInChildren<AudioSource>();

            if (backgroundSFXTime < 1)
            {
                backgroundSFXSource[1].volume = Mathf.Lerp(0, 0.005f, backgroundSFXTime);
                backgroundSFXTime += Time.deltaTime * exitSpeed;
            }
            else
            {
                fadeBackgroundSFXIn = false;
                backgroundSFXTime = 0;
            }

            if (backgroundSFXSource[1].isPlaying == false)
            {
                backgroundSFXSource[1].Play();
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

    public static void Loop(bool value)
    {
        Instance.GetComponent<AudioSource>().loop = value;
    }

    public static void SetBackgroundSFX(AudioClip SFX)
    {
        backgroundSFX = SFX;
        backgroundSFXChanged = true;
    }

    void BackgroundSFX()
    {
        AudioSource[] backgroundSFXSource = gameObject.GetComponentsInChildren<AudioSource>();
        backgroundSFXSource[1].clip = backgroundSFX;
        backgroundSFXChanged = false;
    }

    public static void FadeOutBackgroundSFX()
    {
        AudioSource[] backgroundSFXSource = Instance.GetComponentsInChildren<AudioSource>();
        Instance.fadeBackgroundSFXIn = false;
        Instance.fadeBackgroundSFXOut = true;
        Instance.backgroundSFXTime = 0;
    }

    public static void FadeInBackgroundSFX()
    {
        AudioSource[] backgroundSFXSource = Instance.GetComponentsInChildren<AudioSource>();
        Instance.fadeBackgroundSFXIn = true;
        Instance.fadeBackgroundSFXOut = false;
        Instance.backgroundSFXTime = 0;
    }
}
