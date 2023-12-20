using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    static SoundManager instance;

    public AudioClip startClip;
    public AudioClip overClip;
    public AudioClip failClip;
    public AudioClip[] hitClips;

    public AudioSource bgmPlayer;
    public AudioSource[] sfxPlayers;
    public int nextPlayer;

    void Start()
    {
        instance = this;
    }

    public static void BGMStart()
    {
        instance.bgmPlayer.Play();
    }

    public static void BGMStop()
    {
        instance.bgmPlayer.Stop();
    }

    public static void PlaySound(string name)
    {
        switch(name)
        {
            case "Start":
                instance.sfxPlayers[instance.nextPlayer].clip = instance.startClip;
                break;
            case "Over":
                instance.sfxPlayers[instance.nextPlayer].clip = instance.overClip;
                break;
            case "Hit":
                int random = Random.Range(0, instance.hitClips.Length);
                instance.sfxPlayers[instance.nextPlayer].clip = instance.hitClips[random];
                break;
            case "Fail":
                instance.sfxPlayers[instance.nextPlayer].clip = instance.failClip;
                break;

        }
        instance.sfxPlayers[instance.nextPlayer].Play();
        // 돌아가며 플레이
        instance.nextPlayer = (instance.nextPlayer + 1) % instance.sfxPlayers.Length;
    }
}
