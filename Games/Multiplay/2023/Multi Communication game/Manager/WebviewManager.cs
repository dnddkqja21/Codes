using UnityEngine;
using Vuplex.WebView;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections;
/// <summary>
/// 공통 웹뷰
/// </summary>

// 데이터 컨테이너
[System.Serializable]
public class DataContainer
{
    public string handlerId;
    public string command;
    public string message;
}

public class WebviewManager : MonoBehaviour
{
    static WebviewManager instance;
    public static WebviewManager Instance { get { return instance; } }

    public CanvasWebViewPrefab webview;

    public bool isNetworkChanged;

    // 웹뷰가 생성되었음을 알림
    public delegate void WebviewCreated();
    public event WebviewCreated isWebviewCreated;

    void OnWebviewCreated()
    {
        isWebviewCreated?.Invoke();
    }

    async void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        if(SceneManager.GetActiveScene().buildIndex == (int)SceneName.Init)
        {
#if UNITY_STANDALONE || UNITY_EDITOR
            await StandaloneWebView.TerminateBrowserProcess();
#endif
            Web.SetCameraAndMicrophoneEnabled(true);
        }
        LoadUrl(false, URL_CONFIG.MAIN_FRONT);
    }

    async void Start()
    {        
        await webview.WaitUntilInitialized();
        
        webview.WebView.MessageEmitted += (sender, eventArgs) =>
        {
            Debug.Log("Result received : " + eventArgs.Value);

            string jsonString = eventArgs.Value;
            DataContainer dataContainer = JsonUtility.FromJson<DataContainer>(jsonString);

            if (dataContainer.command == "getDeviceToken")
            {

            }
            else if (dataContainer.command == "loginData")
            {

            }
            else if (dataContainer.command == "userUpdate")
            {

            }
            else if(dataContainer.command == "langFromWebview")
            {
                // 로컬라이징
                // 웹뷰에서 받은 커맨드와 현재 언어가 같다면 return;
                if(dataContainer.message.Equals(LocalizationManager.Instance.CurrentLocale()))
                {
                    return;
                }
                else
                {
                    LocalizationManager.Instance.OnChangeLanguage();
                }
            }
            else if (dataContainer.command == "closeWebview")
            {
                // 로그인 씬에서는 로그인데이터 내려주면 클로즈웹뷰 유니티로 던짐
                // init -> 회원 관련 페이지 닫을 때
                // 실내 공간에서 -> 월드로 나가기,
                // 월드에서는 수강 회원가입, 번역기 

                switch (SceneManager.GetActiveScene().buildIndex)
                {
                    case (int)SceneName.Init:
                        webview.Visible = false;
                        break;
                    case (int)SceneName.World:
                        webview.Visible = false;
                        break;
                    case (int)SceneName.Interior:
                        isNetworkChanged = true;
                        PhotonManagerLobby.Instance.JoinRoom();
                        break;
                }
            }
            else if (dataContainer.command == "updatePosition")
            {

            }
            else
            {
                Debug.Log("로그인 데이터 아님");
            }

            SendMessege(dataContainer.handlerId);            
        };
        OnWebviewCreated();
    }

    async public void LoadUrl(bool isVisibleWebview,string url)
    {
        await webview.WaitUntilInitialized();
        webview.WebView.LoadUrl(url);

        await webview.WebView.WaitForNextPageLoadToFinish();
        webview.Visible = isVisibleWebview;
    }

    public void LoginMessage()
    {
        LoadUrl(false, URL_CONFIG.MAIN_FRONT);

        var message = new Dictionary<string, object>
        {
            { "handlerOrFn", "loginDataFromNative" },
            { "message", UserData.Instance.avatarData.GetLoginUser() },
            { "error", "" }
        };
        string jsonMessage = JsonConvert.SerializeObject(message);

        webview.WebView.PostMessage(jsonMessage);
        Debug.Log("로그인 주입 성공");
        Debug.Log(jsonMessage);
    }

    async public void ChangeLangMessage()
    {
        await webview.WaitUntilInitialized();
        var message = new Dictionary<string, object>
        {
            { "handlerOrFn", "langFromNative" },
            { "message", LocalizationManager.Instance.CurrentLocale() },
            { "error", "" }
        };
        string jsonMessage = JsonConvert.SerializeObject(message);  
        webview.WebView.PostMessage(jsonMessage);
    }

    public async void OnBackButton()
    {
        if (await webview.WebView.CanGoBack())
        {
            webview.WebView.GoBack();
        }
        else
        {
            Debug.Log("닫기");
        }
    }

    void SendMessege(string handlerId)
    {
        var data = new Dictionary<string, string>
        {
            { "msg", "TEST" }
        };
        var message = new Dictionary<string, object>
        {
            { "handlerId", handlerId },
            { "data", data },
            { "error", "null" }
        };
        string jsonMessage = JsonConvert.SerializeObject(message);

        webview.WebView.PostMessage("{\"handlerOrFn\": \"handler\", \"message\":" + jsonMessage + "}");
    }
}
