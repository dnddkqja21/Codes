using BackEnd;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization;
using System.Linq;
using System.Collections.Generic;
/// <summary>
/// 로그인, 회원가입
/// </summary>

public class Login 
{
    static Login instance = null;
    public static Login Instance { get { if (instance == null) { instance = new Login(); } return instance; } }

    public void SignUp(string id, string pw)
    {
        var bro = Backend.BMember.CustomSignUp(id, pw);

        if (bro.IsSuccess())
        {
            Debug.Log("회원가입에 성공했습니다. : " + bro);
            UnityMainThread.wkr.AddJob(() =>
            {
                //원래 돌려야하는 코드 작성 
                Locale curLang = LocalizationSettings.SelectedLocale;
                string text = LocalizationSettings.StringDatabase.GetLocalizedString("My Table", "가입성공", curLang);
                PopupManager.Instance.ShowOneButtnPopup(text, UIManagerInit.Instance.GoToLoginPanel);
                UIManagerInit.Instance.inputIDSignUp.text = string.Empty;
                UIManagerInit.Instance.inputPWSignUp.text = string.Empty;
            });
        }
        else
        {
            Debug.LogError("회원가입에 실패했습니다. : " + bro);
            UnityMainThread.wkr.AddJob(() =>
            {
                string errorMessage = ObjectToDictionary(bro);
                string text = EditMessage(errorMessage);
                PopupManager.Instance.ShowOneButtnPopup(text + ".");
            });            
        }
    }

    // 서버에서 오는 오브젝트를 딕셔너리에 저장
    string ObjectToDictionary(BackendReturnObject bro)
    {
        string serverMessage = bro.ToString();

        string[] lines = serverMessage.Split('\n');
        Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();

        foreach (string line in lines)
        {           
            string[] parts = line.Split(':');

            if (parts.Length == 2)
            {               
                string key = parts[0].Trim();
                string value = parts[1].Trim();
                
                keyValuePairs[key] = value;
            }
        }
        //string statusCode = keyValuePairs["statusCode"]; // "409"
        //string errorCode = keyValuePairs["errorCode"];   // "DuplicatedParameterException"
        string message = keyValuePairs["message"];       // "Duplicated customId, duplicate customId"

        //string[] codeAndMessage = new string[2];
        //codeAndMessage[0] = statusCode;
        //codeAndMessage[1] = message;

        return message;
    }

    // 영문, 한글로 오는 서버의 메시지를 편집
    string EditMessage(string serverMessage)
    {
        Locale locale = LocalizationSettings.SelectedLocale;
        string lang = locale.ToString();
        
        string input = serverMessage.ToString();
        string[] parts = input.Split(',').Select(part => part.Trim()).ToArray();        

        string warning = string.Empty;
        if (lang.Equals("Korean (ko)"))
        {
            warning = parts[1]; 
        }
        else
        {
            warning = parts[0];            
        }
        return warning;                
    }
}
