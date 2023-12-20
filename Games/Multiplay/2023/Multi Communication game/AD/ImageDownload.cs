using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class ImageDownload : MonoBehaviour
{
    string imageUrl = URL_CONFIG.MAIN_BACK + URL_CONFIG.IMAGE;

    void Start()
    {
        if(BuildConfig.isRelease)
        {
            StartCoroutine(UpdateFile());        
        }
    }

    IEnumerator UpdateFile()
    {
        string filePath = Application.persistentDataPath + "/tempImage.png";
        // 경로에 해당 파일이 존재하는지 체크        
        if (File.Exists(filePath))
        {
            using (UnityWebRequest headRequest = UnityWebRequest.Head(imageUrl))
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
                        yield return DownloadImage(imageUrl, filePath);
                    }
                    else
                    {
                        Debug.Log("파일 날짜 안 바뀜");                        
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
            yield return DownloadImage(imageUrl, filePath);
        }
    }

    IEnumerator DownloadImage(string url, string path)
    {
        // 이미지 다운로드
        using (UnityWebRequest webRequest = UnityWebRequest.Get(imageUrl))
        {
            // 요청
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error downloading image: " + webRequest.error);
                yield break;
            }
            else
            {
                byte[] imageData = webRequest.downloadHandler.data;

                // Check if the image data is valid
                if (imageData == null || imageData.Length == 0)
                {
                    Debug.LogError("Downloaded image data is empty or null.");
                    yield break;
                }

                // 파일 경로
                string tempPath = Application.persistentDataPath + "/tempImage.png";

                // 파일 쓰기
                File.WriteAllBytes(tempPath, imageData);
                Debug.Log("이미지 다운로드 완료");
            }
        }
    }
}
