using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Video;

/// <summary>
/// 비디오 플레이어
/// 광고 영상 재생 관련
/// </summary>

public class PlayVideo : MonoBehaviour
{
    VideoPlayer video;

    void Awake()
    {
        video = GetComponent<VideoPlayer>();
    }

    void Start()
    {
        string videoFilePath = Application.persistentDataPath + "/tempVideo.mp4";
        if(File.Exists(videoFilePath))
        {
            video.url = videoFilePath;
            video.Play();
            video.SetDirectAudioMute(0, true);
        }
        else
        {
            Debug.Log("비디오 파일 존재하지 않음");
        }
    }
}
