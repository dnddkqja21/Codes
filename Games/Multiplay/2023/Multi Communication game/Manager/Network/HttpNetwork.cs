using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using UnityEngine;

/// <summary>
/// 공통 PostSendNetwork
/// </summary>

public class HttpNetwork : MonoBehaviour
{
    static HttpNetwork instance = null;
    public static HttpNetwork Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public IEnumerator PostSendNetwork(Dictionary<string, object> requestData, string tailUrl, Action<object> callback)
    {
        if (!NetworkInterface.GetIsNetworkAvailable())
        {
            Locale curLang = LocalizationSettings.SelectedLocale;
            string text = LocalizationSettings.StringDatabase.GetLocalizedString("Table 01", "인터넷상태", curLang);
            PopupManager.Instance.ShowOneButtnPopup(text);
            // 리턴을 할지 어떻게 할지 정책 결정.
            yield break;
        }
        string jsonMessage = JsonConvert.SerializeObject(requestData);
        string url = URL_CONFIG.MAIN_BACK + tailUrl;

        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, jsonMessage))
        {
            byte[] jsonBytes = System.Text.Encoding.UTF8.GetBytes(jsonMessage);

            // 유징 구문에서 뉴해주었기 때문에 중복이 일으키는 메모리릭 해결 
            webRequest.uploadHandler.Dispose();

            webRequest.uploadHandler = new UploadHandlerRaw(jsonBytes);
            webRequest.SetRequestHeader("Content-Type", "application/json;charset=UTF-8");
            if(UserData.Instance.avatarData.accessToken != null)
            {
                webRequest.SetRequestHeader("X-AUTH-TOKEN", UserData.Instance.avatarData.accessToken);
            }            

            yield return webRequest.SendWebRequest();

            string responseText = webRequest.downloadHandler.text;

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                // 네트워크 에러 (서버 다운, 점검 etc...)
                Dictionary<string, object> response = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseText);

                string msg = string.Empty;
                if (response.ContainsKey("message"))
                {
                    msg = (string) response["message"];
                }

                JObject jsonObj = JObject.Parse(responseText);
                bool authErrorExists = jsonObj["data"]?["authError"] != null;
                long code = (long)response["code"];

                if(code == 401 && authErrorExists)
                {   //갱신 api
                    Dictionary<string, object> refeshData = new Dictionary<string, object>();
                    refeshData.Add("accessToken", UserData.Instance.avatarData.accessToken);
                    StartCoroutine(PostSendNetwork(refeshData, URL_CONFIG.REFRESH_TOKEN, (data) => {
                        //재귀함수
                        Dictionary<string, object> obj = (Dictionary<string, object>)data;
                        UserData.Instance.avatarData.accessToken = (string)obj["newToken"];
                        Debug.Log("갱신된 토큰 : " + UserData.Instance.avatarData.accessToken);
                        StartCoroutine(PostSendNetwork(requestData, tailUrl, callback));
                    }));
                }                
                else
                {
                    PopupManager.Instance.ShowOneButtnPopup(msg);
                }
                
            }
            else
            {
                Dictionary<string, object> response = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseText);
                if (response.ContainsKey("data"))
                {
                    if (response["data"] != null)
                    {
                        string jsonStr = response["data"].ToString();
                        JToken tokenArray = JToken.Parse(jsonStr);

                        if (tokenArray is JArray)
                        {
                            List<Dictionary<string, object>> data = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(jsonStr);
                            callback(data);
                        }
                        else
                        {
                            Dictionary<string, object> data = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonStr);
                            callback(data);
                        }
                    }
                    else
                    {
                        callback(null);
                    }
                }
                else
                {
                    callback(null);
                }
            }
            webRequest.Dispose();
        }
    }
}
