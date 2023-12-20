using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

/// <summary>
/// 영상 다운로드
/// 앱 최초 실행 시 광고 영상 관련 다운로드 프로세스
/// </summary>

public class VideoDownload : MonoBehaviour
{
    // 여러 개의 영상일 때는 리스트나 배열로 관리    
    string videoUrl = URL_CONFIG.MAIN_BACK + URL_CONFIG.VIDEO;

    [SerializeField]
    Slider progressBar;
    [SerializeField]
    TextMeshProUGUI progressText;
    [SerializeField]
    Button loginButton;

    void Start()
    {
        if (BuildConfig.isRelease)
        {
            StartCoroutine(UpdateFile());   
        }
        else
        {
            ActiveLoginButton();
        }
    }

    IEnumerator UpdateFile()
    {
        string filePath = Application.persistentDataPath + "/tempVideo.mp4";
        // 경로에 해당 파일이 존재하는지 체크        
        if (File.Exists(filePath))
        {
            using (UnityWebRequest headRequest = UnityWebRequest.Head(videoUrl))
            {
                yield return headRequest.SendWebRequest();

                if (headRequest.result != UnityWebRequest.Result.ConnectionError && headRequest.result != UnityWebRequest.Result.ProtocolError)
                {
                    // 서버의 파일 날짜
                    string lastModifiedHeader = headRequest.GetResponseHeader("Last-Modified");
                    DateTime remoteFileModifiedDate = DateTime.Parse(lastModifiedHeader);

                    // 로컬 파일의 날짜를 가져온다.
                    DateTime localFileModifiedDate = File.GetLastWriteTime(filePath);

                    // 날짜 바뀌었으면 재다운로드.
                    if (remoteFileModifiedDate > localFileModifiedDate)
                    {
                        Debug.Log("파일 날짜 바뀜 재다운로드");
                        // Download the updated video
                        yield return DownloadVideo(videoUrl, filePath);
                    }
                    else
                    {
                        Debug.Log("파일 날짜 안 바뀜");
                        ActiveLoginButton();
                        yield break;
                    }
                }
                else
                {
                    Debug.LogError("HTTP request error: " + headRequest.error);
                }
            }
        }
        else
        {
            Debug.Log("파일 없음, 다운로드");
            yield return DownloadVideo(videoUrl, filePath);
        }
    }

    IEnumerator DownloadVideo(string url, string path)
    {
        // 영상 다운로드
        using (UnityWebRequest webRequest = UnityWebRequest.Get(videoUrl))
        {
            // 요청
            var asyncOperation = webRequest.SendWebRequest();

            // 다운로드 진행 바
            while (!asyncOperation.isDone)
            {
                float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
                progressBar.value = progress;
                progressText.text = "file download progress... " + string.Format("{0}%", Mathf.RoundToInt(progress * 100));
                yield return null;
            }

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error downloading video: " + webRequest.error);
                yield break;
            }
            else
            {
                byte[] videoData = webRequest.downloadHandler.data;

                // 파일 경로
                string tempPath = Application.persistentDataPath + "/tempVideo.mp4";

                // 파일 쓰기
                File.WriteAllBytes(tempPath, videoData);

                ActiveLoginButton();
            }
        }
    }

    void ActiveLoginButton()
    {
        // 시작 버튼 활성화 및 프로그레스 바 비활성화
        loginButton.interactable = true;
        progressBar.gameObject.SetActive(false);
        progressText.gameObject.SetActive(false);
    }
}
