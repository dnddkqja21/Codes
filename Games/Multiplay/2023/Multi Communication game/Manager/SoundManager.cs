using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 사운드 매니저
/// </summary>

public class SoundManager : MonoBehaviour
{
    static SoundManager instance;

    public static SoundManager Instance { get { return instance; } }

    [Header("BGM")]
    [SerializeField]
    AudioClip bgmClip;
    [SerializeField]
    float bgmVolume = 0.5f;
    AudioSource bgmPlayer;
    [SerializeField]
    Slider bgmSlider;

    [Header("SFX")]
    [SerializeField]
    AudioClip[] sfxClips;
    [SerializeField]
    float sfxVolume = 0.5f;    
    int channels = 10;
    int channelIndex;
    AudioSource[] sfxPlayers;
    [SerializeField]
    Slider sfxSlider;

    const string BGMVolumeKey = "BGMVolume";
    const string SFXVolumeKey = "SFXVolume";

    void Awake()
    {
        if(instance == null)
            instance = this;

        InitSoundPlayer();
    }

    void Start()
    {
        // 저장된 사운드 값
        float savedBGMVolume = PlayerPrefs.GetFloat(BGMVolumeKey, bgmVolume);
        SetBGMVolume(savedBGMVolume);
        float savedSFXVolume = PlayerPrefs.GetFloat(SFXVolumeKey, sfxVolume);
        SetSFXVolume(savedSFXVolume);

        // 슬라이더 함수 연결
        bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);

        // 볼륨 세팅
        SetBGMVolume(bgmVolume); 
        SetSFXVolume(sfxVolume); 

        // 슬라이더 세팅
        InitSliderVolume(bgmSlider, bgmVolume);
        InitSliderVolume(sfxSlider, sfxVolume);

        PlayBGM();
    }

    void InitSoundPlayer()
    {
        GameObject bgm = new GameObject("BGM Player");
        bgm.transform.parent = transform;
        bgmPlayer = bgm.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClip;

        GameObject sfx = new GameObject("SFX Player");
        sfx.transform.parent = transform;
        sfxPlayers =  new AudioSource[channels];

        for (int i = 0; i < sfxPlayers.Length; i++)
        {
            sfxPlayers[i] = sfx.AddComponent<AudioSource>();
            sfxPlayers[i].playOnAwake = false;
            sfxPlayers[i].volume = sfxVolume;
        }
    }

    void PlayBGM()
    {
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
            if(sfx == SFX.BubbleIn)
            {
                random = Random.Range(0, 3);
            }
            else if (sfx == SFX.BubbleOut)
            {
                random = Random.Range(0, 2);
            }

            channelIndex = loopIndex;
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx + random];
            sfxPlayers[loopIndex].Play();
            break;
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
        PlayerPrefs.SetFloat(BGMVolumeKey, bgmVolume);
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
        for (int i = 0; i < sfxPlayers.Length; i++)
        {            
            sfxPlayers[i].volume = sfxVolume;
        }
        PlayerPrefs.SetFloat(SFXVolumeKey, sfxVolume);
    }
}
