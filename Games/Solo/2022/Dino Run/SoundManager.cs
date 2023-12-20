using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SFX
{
    Jump,
    Land,
    Hit,
    Start
}

public class SoundManager : MonoBehaviour
{
    [Header("Å¬¸³")]
    public AudioClip[] clips;

    AudioSource audios;

    void Awake()
    {
        audios = GetComponent<AudioSource>(); 
    }

    public void PlaySound(SFX sfx)
    {
        audios.clip = clips[(int)sfx];
        audios.Play();
    }
}
