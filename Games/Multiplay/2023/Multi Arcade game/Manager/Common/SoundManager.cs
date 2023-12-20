using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 사운드 매니저
/// </summary>

public class SoundManager : MonoBehaviour
{
    static SoundManager instance = null;

    public static SoundManager Instance { get { return instance; } }

    [Header("BGM")]
    [SerializeField]
    AudioClip[] bgmClips;
    [SerializeField]
    float bgmVolume = 0.3f;
    AudioSource bgmPlayer;    
    Slider bgmSlider;

    [Header("SFX")]
    [SerializeField]
    AudioClip[] sfxClips;
    [SerializeField]
    float sfxVolume = 0.7f;
    int channels = 10;
    int channelIndex;
    AudioSource[] sfxPlayers;    
    Slider sfxSlider;

    const string BGM_Volume_Key = "BGM_Volume";
    const string SFX_Volume_Key = "SFX_Volume";

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        InitSoundPlayer();
    }

    void OnEnable()
    {        
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartSoundSystem();
    }

    void InitSoundPlayer()
    {
        GameObject bgm = new GameObject("BGM Player");
    
        bgm.transform.parent = transform;
        bgmPlayer = bgm.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;        

        GameObject sfx = new GameObject("SFX Player");
        sfx.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];

        for (int i = 0; i < sfxPlayers.Length; i++)
        {
            sfxPlayers[i] = sfx.AddComponent<AudioSource>();
            sfxPlayers[i].playOnAwake = false;
            sfxPlayers[i].volume = sfxVolume;
        }
    }

    void StartSoundSystem()
    {
        // 저장된 사운드 값
        float savedBGMVolume = PlayerPrefs.GetFloat(BGM_Volume_Key, bgmVolume);
        SetBGMVolume(savedBGMVolume);
        float savedSFXVolume = PlayerPrefs.GetFloat(SFX_Volume_Key, sfxVolume);
        SetSFXVolume(savedSFXVolume);

        // 월드 씬일 경우 슬라이더 함수 연결
        if (SceneManager.GetActiveScene().buildIndex == (int)SceneList.World)
        {
            bgmSlider = Config.FindChild(UIManagerWorld.Instance.canvas, "Slider BGM").GetComponent<Slider>();
            sfxSlider = Config.FindChild(UIManagerWorld.Instance.canvas, "Slider SFX").GetComponent<Slider>();

            bgmSlider.onValueChanged.AddListener(SetBGMVolume);
            sfxSlider.onValueChanged.AddListener(SetSFXVolume);

            // 슬라이더 세팅
            InitSliderVolume(bgmSlider, bgmVolume);
            InitSliderVolume(sfxSlider, sfxVolume);
        }

        // 볼륨 세팅
        SetBGMVolume(bgmVolume);
        SetSFXVolume(sfxVolume);

        PlayBGM();
    }

    void PlayBGM()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        bgmPlayer.clip = bgmClips[sceneIndex];
        bgmPlayer.Play();
    }

    public void PlaySFX(SFX sfx)
    {
        for (int i = 0; i < sfxPlayers.Length; i++)
        {
            int loopIndex = (i + channelIndex) % sfxPlayers.Length;

            if (sfxPlayers[loopIndex].isPlaying)
            {
                continue;
            }

            int random = 0;
            if (sfx == SFX.Click)
            {
                random = Random.Range(0, 5);
            }
            else if (sfx == SFX.Hover)
            {
                random = Random.Range(0, 2);
            }
            else if (sfx == SFX.Panel)
            {
                random = Random.Range(0, 3);
            }
            else if(sfx == SFX.Shooting)
            {
                random = Random.Range(0, 2);
            }
            else if (sfx == SFX.Hit)
            {
                random = Random.Range(0, 3);
            }
            else if (sfx == SFX.Death)
            {
                random = Random.Range(0, 2);
            }

            channelIndex = loopIndex;
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx + random];

            if (sfx == SFX.Walking)
            {
                sfxPlayers[loopIndex].loop = true; 
                sfxPlayers[loopIndex].pitch = 1.3f; 
            }
            else if (sfx == SFX.Running)
            {
                sfxPlayers[loopIndex].loop = true;
                sfxPlayers[loopIndex].pitch = 1.75f;
            }
            else
            {
                sfxPlayers[loopIndex].loop = false; 
                sfxPlayers[loopIndex].pitch = 1.0f; 
            }

            sfxPlayers[loopIndex].Play();
            break;
        }
    }

    public void StopSFX(SFX sfx)
    {
        for (int i = 0; i < sfxPlayers.Length; i++)
        {
            if (sfxPlayers[i].isPlaying && sfxPlayers[i].clip == sfxClips[(int)sfx])
            {
                sfxPlayers[i].Stop();
                break;
            }
        }
    }

    void InitSliderVolume(Slider slider, float volume)
    {
        slider.value = volume;
    }

    public void SetBGMVolume(float volume)
    {
        bgmVolume = volume;
        bgmPlayer.volume = bgmVolume;
        PlayerPrefs.SetFloat(BGM_Volume_Key, bgmVolume);
    }

    public float GetBGMVolume()
    {
        return bgmVolume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
        for (int i = 0; i < sfxPlayers.Length; i++)
        {
            sfxPlayers[i].volume = sfxVolume;
        }
        PlayerPrefs.SetFloat(SFX_Volume_Key, sfxVolume);
    }
}
