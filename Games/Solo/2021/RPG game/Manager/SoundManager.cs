using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource Bgm;

    public AudioSource Effect;

    public void SetBGMVolume(float _volume)
    {
        Bgm.volume = _volume;
    }

    public void SetEffectVolume(float _volume)
    {
        Effect.volume = _volume;
    }
}
