using System.Collections.Generic;
using System.Collections;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization;

public class Login : MonoBehaviour
{
    [Header("인풋필드, 로그인")]
    [SerializeField]
    TMP_InputField inputID;
    [SerializeField]
    TMP_InputField inputPW;
    [SerializeField]
    Button LoginButton;

    [Header("계정 관련 버튼")]
    [SerializeField]
    Button findID;
    [SerializeField]
    Button findPW;
    [SerializeField]
    Button signUp;
    [SerializeField]
    Button secession;

    [Header("버튼 - 웹 연결")]
    [SerializeField]
    LoadURLButton loadURLButton;

    [Header("로딩 패널")]
    [SerializeField]
    GameObject loadingPanel;

    [Header("로그인 관련 토글")]
    [SerializeField]
    Toggle toggleAutoLogin;
    [SerializeField]
    Toggle toggleSaveID;

    bool autoLogin = false;
    bool saveID = false;

    [Header("앱 종료")]
    [SerializeField]
    Button exit;

    [SerializeField]
    FirebaseManager firebaseManager;    

    void Start()
    {
        // 토클 버튼 관련 세팅 & 연결
        SetToggleButton();
        toggleAutoLogin.onValueChanged.AddListener(ToggleAutoLogin);
        toggleSaveID.onValueChanged.AddListener(ToggleSaveID);               

        // 회원 관련 버튼 연결
        findID.onClick.AddListener(() => {
            WebviewManager.Instance.LoadUrl(true, URL_CONFIG.MAIN_FRONT + URL_CONFIG.FIND_ID);
        });
        findPW.onClick.AddListener(() => {
            WebviewManager.Instance.LoadUrl(true, URL_CONFIG.MAIN_FRONT + URL_CONFIG.FIND_PASS);
        });
        signUp.onClick.AddListener(() => {
            WebviewManager.Instance.LoadUrl(true, URL_CONFIG.MAIN_FRONT + URL_CONFIG.SIGN_UP);
        });        

        // 앱 종료 버튼
        exit.onClick.AddListener(() => {
            Locale curLang = LocalizationSettings.SelectedLocale;
            string text = LocalizationSettings.StringDatabase.GetLocalizedString("Table 01", "종료", curLang);
            PopupManager.Instance.ShowTwoButtnPopup(text, null, ExitApp);
        });

        // 로그인 버튼
        LoginButton.onClick.AddListener(() => {
            CheckLogin();           
        });

        if (autoLogin)
        {
            // 웹뷰 이닛 기다림
            WebviewManager.Instance.isWebviewCreated += AutoLogin;
            //Invoke("AutoLogin", 3f);
            //AutoLogin();
        }
        else if (Util.LoadData(AESUtil.SaveID).Equals("1"))
        {
            string id = Util.LoadData(AESUtil.USER_ID);
            if (!id.Equals(""))
            {
                inputID.text = id;
            }
        }
#if UNITY_EDITOR_WIN
        // test        
        inputID.text = BuildConfig.isRelease ? "dnddkqja@musicen.com" : "test01@musicen.com";
        inputPW.text = BuildConfig.isRelease ? "fkrxhvlt[]" : "1234";
#endif
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (inputID.isFocused)
            {
                NavigateTo(inputPW);
            }
            else if (inputPW.isFocused)
            {
                NavigateTo(inputID);
            }
        }  
        if(Input.GetKeyDown(KeyCode.Return) && !WebviewManager.Instance.webview.Visible)
        {
            CheckLogin();
        }
    }       

    void AutoLogin()
    {
        // 자동 로그인 
        if (Util.LoadData(AESUtil.AutoLogin).Equals("1"))
        {
            // 로그인 시키기
            string id = Util.LoadData(AESUtil.USER_ID);
            string pass = Util.LoadData(AESUtil.USER_PASS);
            ApiLogin(id, pass);
        }
    }

    void NavigateTo(TMP_InputField targetField)
    {
        targetField.Select();
        targetField.ActivateInputField();
    }

    void SetToggleButton()
    {
        SetToggleIsOn(AESUtil.AutoLogin, toggleAutoLogin);
        SetToggleIsOn(AESUtil.SaveID, toggleSaveID);
    }

    void SetToggleIsOn(string key, Toggle toggle)
    {
        string value = "0";
        int savedIntValue = 0;
        if (!Util.LoadData(key).Equals(""))
        {
            value = Util.LoadData(key);
        }
        
        savedIntValue = int.Parse(value);
        if(toggle == toggleAutoLogin)
        {
            autoLogin = savedIntValue == 1 ? true : false; 
        }
        else if (toggle == toggleSaveID)
        {
            saveID = savedIntValue == 1 ? true : false;
        }

        bool boolValue = savedIntValue != 0;
        toggle.isOn = boolValue;
    }

    void ToggleAutoLogin(bool value)
    {
        autoLogin = value;
        SaveBoolean(autoLogin, AESUtil.AutoLogin);
    }

    void ToggleSaveID(bool value)
    {
        saveID = value;
        SaveBoolean(saveID, AESUtil.SaveID);
    }

    void SaveBoolean(bool boolean, string key)
    {
        int intValueToboolean = boolean ? 1 : 0;
        Util.SaveData(key, intValueToboolean.ToString());
    }

    void CheckLogin()
    {        
        string msg = "";
        if (inputID.text.Equals(""))
        {
            Locale curLang = LocalizationSettings.SelectedLocale;
            string text = LocalizationSettings.StringDatabase.GetLocalizedString("Table 01", "아이디입력", curLang);
            msg = text;
        }
        else if(!CheckEmail(inputID.text))
        {
            Locale curLang = LocalizationSettings.SelectedLocale;
            string text = LocalizationSettings.StringDatabase.GetLocalizedString("Table 01", "아이디입력", curLang);
            msg = text;
        }
        else if (inputPW.text.Equals(""))
        {
            Locale curLang = LocalizationSettings.SelectedLocale;
            string text = LocalizationSettings.StringDatabase.GetLocalizedString("Table 01", "비밀번호입력", curLang);
            msg = text;
        }        
        //else if (!CheckPass(inputPW.text))
        //{
        //    msg = "비밀번호 형식이 아닙니다.";
        //}
        if (!msg.Equals(""))
        {
            PopupManager.Instance.ShowOneButtnPopup(msg);
            return;
        }

        ApiLogin(inputID.text , inputPW.text);
    }

    void ApiLogin(string id , string pass)
    {        
        Dictionary<string, object> requestData = new Dictionary<string, object>();
        requestData.Add("userEmail", id);
        requestData.Add("userPassword", pass);
        string appOs = "";
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_EDITOR_OSX
        appOs = "PC";        
#elif UNITY_ANDROID
        appOs = "AOS";
#elif UNITY_IOS
        appOs = "IOS";
#endif
        requestData.Add("appOs", appOs);
        requestData.Add("deviceToken", firebaseManager.deviceToken);
        
        StartCoroutine(HttpNetwork.Instance.PostSendNetwork(requestData, URL_CONFIG.LOGIN, (data) =>
        {
            if (data != null)
            {
                if(autoLogin == true)
                {
                    Util.SaveData(AESUtil.USER_ID, id);
                    Util.SaveData(AESUtil.USER_PASS, pass);
                }
                else
                {
                    if (saveID == true)
                    {
                        Util.SaveData(AESUtil.USER_ID, id);
                    }
                    Util.SaveData(AESUtil.USER_PASS, pass);
                }
                // 웹에서 내려온 데이터를 유저데이터에 주입
                UserData.Instance.avatarData.SetFromDictionary((Dictionary<string, object>)data);

                // 로그인 데이터 전달
                WebviewManager.Instance.LoginMessage();
                PhotonManagerLobby.Instance.ConnectToPhoton();
                PhotonManagerLobby.Instance.isLogout = false;
                loadingPanel.SetActive(true);
            }
        }));
    }   

    bool CheckEmail(string email)
    {
        string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        return Regex.IsMatch(email, pattern);
    }
    bool CheckPass(string password)
    {
        string passwordPattern = @"^(?=.*[A-Z].*[A-Z])(?=.*[a-z].*[a-z])(?=.*\d.*\d)(?=.*[^a-zA-Z\d].*[^a-zA-Z\d]).{10,}$";
        return Regex.IsMatch(password, passwordPattern);


        //// Check for a minimum length of 10 characters
        //if (password.Length < 10)
        //    return false;

        //// Count the occurrences of uppercase, lowercase, numbers, and special characters
        //int uppercaseCount = 0;
        //int lowercaseCount = 0;
        //int numberCount = 0;
        //int specialCharCount = 0;

        //foreach (char c in password)
        //{
        //    if (char.IsUpper(c))
        //        uppercaseCount++;
        //    else if (char.IsLower(c))
        //        lowercaseCount++;
        //    else if (char.IsDigit(c))
        //        numberCount++;
        //    else
        //    {
        //        // Add more special characters if needed
        //        if ("!@#$%^&*()-_=+[]{}|;:,.<>?".Contains(c))
        //            specialCharCount++;
        //    }
        //}

        //// Check if each requirement is met
        //return uppercaseCount >= 2 && lowercaseCount >= 2 && numberCount >= 2 && specialCharCount >= 2;
    }

    void ExitApp()
    {
        Application.Quit();
    }
}
