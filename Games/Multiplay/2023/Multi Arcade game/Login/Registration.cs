using BackEnd;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

/// <summary>
/// 로그인, 회원가입
/// </summary>

public class Registration 
{
    static Registration instance = null;
    public static Registration Instance { get { if (instance == null) { instance = new Registration(); } return instance; } }

    #region 회원가입
    public void SignUp(string id, string pw, string confirm)
    {       
        // 조건 확인 후 가입
        if (pw.Equals(confirm) && IsPasswordValid(pw) && IsAlphanumeric(id))
        {
            var bro = Backend.BMember.CustomSignUp(id, pw);

            if (bro.IsSuccess())
            {
                Debug.Log("회원가입에 성공했습니다. : " + bro);
                UnityMainThread.wkr.AddJob(() =>
                {
                    //원래 돌려야하는 코드 작성              
                    UIManagerInit.Instance.GoToSetPlayerPanel();
                    UIManagerInit.Instance.inputIDSignUp.text = string.Empty;
                    UIManagerInit.Instance.inputPWSignUp.text = string.Empty;
                    UIManagerInit.Instance.inputCPWSignUp.text = string.Empty;
                });
            }
            else
            {
                Debug.LogError("회원가입에 실패했습니다. : " + bro);
                UnityMainThread.wkr.AddJob(() =>
                {
                    string errorMessage = ObjectToDictionary(bro);
                    string text = EditMessage(errorMessage);
                    PopupManager.Instance.ShowOneButtnPopup(true, text + ".");
                });            
            }
        }        
        #region 조건 및 정규식
        if (id.Equals(string.Empty))
        {
            UnityMainThread.wkr.AddJob(() =>
            {
                string text = LocalizationManager.Instance.LocaleTable("아이디입력");
                PopupManager.Instance.ShowOneButtnPopup(false, text);
            });
            return;
        }
        if (pw.Equals(string.Empty))
        {
            UnityMainThread.wkr.AddJob(() =>
            {
                string text = LocalizationManager.Instance.LocaleTable("패스입력");
                PopupManager.Instance.ShowOneButtnPopup(false, text);
            });
            return;
        }
        if (confirm.Equals(string.Empty))
        {
            UnityMainThread.wkr.AddJob(() =>
            {
                string text = LocalizationManager.Instance.LocaleTable("패스확인입력");
                PopupManager.Instance.ShowOneButtnPopup(true, text);
            });
            return;
        }
        if (!IsAlphanumeric(id))
        {
            UnityMainThread.wkr.AddJob(() =>
            {
                string text = LocalizationManager.Instance.LocaleTable("아이디정규식");
                PopupManager.Instance.ShowOneButtnPopup(true, text);
            });
            return;
        }
        if(!pw.Equals(confirm))
        {
            UnityMainThread.wkr.AddJob(() =>
            {
                string text = LocalizationManager.Instance.LocaleTable("비번불일치");
                PopupManager.Instance.ShowOneButtnPopup(false, text);
            });
            return;
        }        
        if (!IsPasswordValid(pw))
        {
            UnityMainThread.wkr.AddJob(() =>
            {
                string text = LocalizationManager.Instance.LocaleTable("비번정규식");
                PopupManager.Instance.ShowOneButtnPopup(true, text);
            });
            return;
        }
        #endregion
    }
    #endregion

    #region 플레이어 세팅 (닉네임, 아바타)
    public void SetPlayer(string nickName)
    {
        // 닉네임 글자 수 제한
        if(!IsNickNameLength(nickName))
        {
            UnityMainThread.wkr.AddJob(() =>
            {
                string text = LocalizationManager.Instance.LocaleTable("닉네임조건");
                PopupManager.Instance.ShowOneButtnPopup(true, text);                
            });
            return;
        }

        var bro = Backend.BMember.UpdateNickname(nickName);

        if (bro.IsSuccess())
        {
            Debug.Log("닉네임 변경에 성공했습니다 : " + bro);
            UnityMainThread.wkr.AddJob(() =>
            {
                //원래 돌려야하는 코드 작성 
                string text = LocalizationManager.Instance.LocaleTable("가입성공");
                PopupManager.Instance.ShowOneButtnPopup(false, text, UIManagerInit.Instance.GoToLoginPanel);

                PlayerData.nickName = nickName;
                PlayerData.avatarNumber = UIManagerInit.Instance.currentIndex;
                PlayerData.SetGameData();

                Debug.Log("닉네임 : " + PlayerData.nickName + " 아바타 번호 : " + PlayerData.avatarNumber);
                UIManagerInit.Instance.inputNickName.text = string.Empty;
            });
        }
        else
        {
            Debug.LogError("닉네임 변경에 실패했습니다 : " + bro);
            UnityMainThread.wkr.AddJob(() =>
            {
                string errorMessage = ObjectToDictionary(bro);
                string text = EditMessage(errorMessage);
                PopupManager.Instance.ShowOneButtnPopup(true, text + ".");
            });
        }
    }
    #endregion

    #region 로그인
    public void Login(string id, string pw)
    {
        Debug.Log("로그인을 요청합니다.");

        var bro = Backend.BMember.CustomLogin(id, pw);

        if (bro.IsSuccess())
        {
            Debug.Log("로그인이 성공했습니다. : " + bro);
            UnityMainThread.wkr.AddJob(() =>
            {                
                AES.SaveData(AES.USER_ID, id);
                AES.SaveData(AES.USER_PASS, pw);

                if(PlayerData.GetGameData())
                {                    
                    UIManagerInit.Instance.panelLoading.SetActive(true);
                    PhotonManager.Instance.ConnectToPhoton();
                };
            });
        }
        else
        {
            Debug.LogError("로그인이 실패했습니다. : " + bro);
            UnityMainThread.wkr.AddJob(() =>
            {               
                string errorMessage = ObjectToDictionary(bro);
                string text = EditMessage(errorMessage);
                PopupManager.Instance.ShowOneButtnPopup(true, text + ".");
            });
        }
    }
    #endregion

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
        string message = keyValuePairs["message"];       // "Duplicated customId, 중복 아이디"

        // 가장 긴 서버 메시지 잘못된 "beginning or end of the nickname must not be blank 입니다"

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

    #region 회원 가입 관련 체크사항

    // 아이디 정규식
    bool IsAlphanumeric(string id)
    {
        // 정규식 패턴: 영문 소문자, 대문자, 숫자로 이루어진 문자열
        string pattern = @"^(?=.*[a-zA-Z])(?=.*[0-9])[a-zA-Z0-9]+$";

        // 정규식 검사 결과를 리턴
        return Regex.IsMatch(id, pattern);
    }

    // 비번 정규식
    bool IsPasswordValid(string password)
    {
        // 정규식 패턴 (8글자 이상, 특수 문자 중 하나 포함, 영문과 숫자의 조합)                        
        string pattern = @"^(?=.*[a-zA-Z])(?=.*\d)(?=.*[!@#$%^&*()_+{}\[\]:;<>,.?~\\-]).{8,}$";                
        return Regex.IsMatch(password, pattern);
    }

    // 닉네임 글자 수 제한
    bool IsNickNameLength(string name)
    {
        if(name.Length > 8)
        {
            return false;
        }
        return true;
    }
    #endregion

    public void Logout()
    {
        Backend.BMember.Logout();
    }

    public void Withdraw()
    {
        Backend.BMember.WithdrawAccount();
    }
}
