using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

/// <summary>
/// 오디오 뮤트
/// 버블챗
/// </summary>

public class MuteAudio : MonoBehaviour
{
    [SerializeField]
    VideoPlayer video;


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {    
            if(other.GetComponent<PhotonView>().IsMine)
                video.SetDirectAudioMute(0, false);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<PhotonView>().IsMine)
                video.SetDirectAudioMute(0, true);
        }
    }
}
