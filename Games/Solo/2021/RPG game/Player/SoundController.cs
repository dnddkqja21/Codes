using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public AudioSource bgmManager;

    public AudioSource mapSoundManager;

    [SerializeField]
    LayerMask layerMask;

    private void OnTriggerEnter(Collider other)
    {
        string tag = other.tag;

        switch(tag)
        {
            case "Sea":
                bgmManager.Stop();
                mapSoundManager.Stop();
                bgmManager.clip = bgmManager.GetComponent<SoundList>().clips[0];
                bgmManager.Play();
                mapSoundManager.PlayOneShot(mapSoundManager.GetComponent<SoundList>().clips[0]);
                break;

            case "BaseCamp":
                bgmManager.Stop();
                mapSoundManager.Stop();
                bgmManager.clip = bgmManager.GetComponent<SoundList>().clips[1];
                bgmManager.Play();
                mapSoundManager.PlayOneShot(mapSoundManager.GetComponent<SoundList>().clips[1]);
                break;

            case "Skel":
                bgmManager.Stop();
                mapSoundManager.Stop();
                bgmManager.clip = bgmManager.GetComponent<SoundList>().clips[2];
                bgmManager.Play();
                mapSoundManager.PlayOneShot(mapSoundManager.GetComponent<SoundList>().clips[2]);
                break;

            case "Orc":
                bgmManager.Stop();
                mapSoundManager.Stop();
                bgmManager.clip = bgmManager.GetComponent<SoundList>().clips[3];
                bgmManager.Play();
                mapSoundManager.PlayOneShot(mapSoundManager.GetComponent<SoundList>().clips[3]);
                break;

            case "Mage":
                bgmManager.Stop();
                mapSoundManager.Stop();
                bgmManager.clip = bgmManager.GetComponent<SoundList>().clips[4];
                bgmManager.Play();
                mapSoundManager.PlayOneShot(mapSoundManager.GetComponent<SoundList>().clips[4]);
                break;

            case "Dragon":
                bgmManager.Stop();
                mapSoundManager.Stop();
                bgmManager.clip = bgmManager.GetComponent<SoundList>().clips[5];
                bgmManager.Play();
                mapSoundManager.PlayOneShot(mapSoundManager.GetComponent<SoundList>().clips[5]);
                break;
        }        
    }
}
