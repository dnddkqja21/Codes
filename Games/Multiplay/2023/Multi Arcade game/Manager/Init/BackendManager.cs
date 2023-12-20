using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using System.Threading.Tasks;

/// <summary>
/// 뒤끝 서버 매니저
/// </summary>

public class BackendManager : MonoBehaviour
{    
    void Start()
    {
        var backend = Backend.Initialize(true);
        if (backend.IsSuccess())
        {
            DebugCustom.Log("뒤끝 초기화 성공 " + backend);
#if UNITY_ANDROID
            CheckApp();
#endif
        }
        else
        {
            DebugCustom.Log("뒤끝 초기화 실패 " + backend);
        }

        UIManagerInit.Instance.buttonSignUp.onClick.AddListener(() =>
        {
            TrySignUp();
        });

        UIManagerInit.Instance.buttonConfirmPlayer.onClick.AddListener(() =>
        {
            TrySetPlayer();
        });

        UIManagerInit.Instance.buttonLogin.onClick.AddListener(() =>
        {
            TryLogin();
        });

        // 구글 해시키 얻기
        //UIManagerInit.Instance.getGoogleHashKey.onClick.AddListener(() =>
        //{
        //    string googlehash = Backend.Utils.GetGoogleHash();
        //    UIManagerInit.Instance.getHashKey.text = googlehash;
        //});
    } 

    async void TrySignUp()
    {
        await Task.Run(() =>
        {
            Registration.Instance.SignUp(UIManagerInit.Instance.inputIDSignUp.text, UIManagerInit.Instance.inputPWSignUp.text, UIManagerInit.Instance.inputCPWSignUp.text);
        });
    }

    async void TrySetPlayer()
    {
        await Task.Run(() =>
        {
            Registration.Instance.SetPlayer(UIManagerInit.Instance.inputNickName.text);
        });
    }

    async void TryLogin()
    {
        await Task.Run(() =>
        {
            Registration.Instance.Login(UIManagerInit.Instance.inputIDLogin.text, UIManagerInit.Instance.inputPWLogin.text);            
        });
    }

    void CheckApp()
    {
        var bro = Backend.Utils.GetLatestVersion();
        //"statusCode : 400\nerrorCode : NotFoundException\nmessage : Not Available OS\n"
        if (bro.ToString().Equals("statusCode : 400\nerrorCode : NotFoundException\nmessage : Not Available OS\n"))
        {
            DebugCustom.Log("모바일 플랫폼이 아닙니다.");
            return;
        }

        string version = bro.GetReturnValuetoJSON()["version"].ToString();        
        if (version == Application.version)
        {
            DebugCustom.Log("버전 일치 : " + version);
            return;
        }
        
        string forceUpdate = bro.GetReturnValuetoJSON()["type"].ToString();
        if (forceUpdate == "1")
        {
            DebugCustom.Log("업데이트를 하시겠습니까? y/n");
        }
        else if (forceUpdate == "2")
        {
            string text = LocalizationManager.Instance.LocaleTable("업데이트");
#if UNITY_ANDROID
            PopupManager.Instance.ShowOneButtnPopup(true, text, () => Application.OpenURL(Config.Store_URL), Application.Quit);            
#elif UNITY_IOS
        //Application.OpenURL("url"); 차후 ios 개발 시 추가
#endif
        }
    }
}
