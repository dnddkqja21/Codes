using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoChatArea : MonoBehaviour
{
    [SerializeField]
    VideoChannelName videoChannelName;

    string channelName;
    float originVolume;

    void Start()
    {
        channelName = videoChannelName.ToString();
        //Debug.Log("채널 명 : " + channelName);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (other.GetComponent<PhotonView>().IsMine)
        {
            originVolume = SoundManager.Instance.GetBGMVolume();
            SoundManager.Instance.SetBGMVolume(0);
            VideoChat.Instance.Join(channelName);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (other.GetComponent<PhotonView>().IsMine)
        {
            SoundManager.Instance.SetBGMVolume(originVolume);
            VideoChat.Instance.Leave();        
        }                
    }
}
