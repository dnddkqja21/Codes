using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI 매니저 (이닛 씬)
/// </summary>

public class UIManagerInit : MonoBehaviour
{
    static UIManagerInit instance = null;
    public static UIManagerInit Instance { get { return instance; } }

    [Header("로그인 패널")]
    [SerializeField]
    Animator panelLogin; 
    public TMP_InputField inputIDLogin;    
    public TMP_InputField inputPWLogin;    
    public Button buttonLogin;
    [SerializeField]
    Toggle toggleSaveID;
    [SerializeField]
    Toggle toggleAutoLogin;    
    public bool saveID = false;
    public bool autoLogin = false;
    [SerializeField]
    Button buttonTrySignUp;

    [Header("회원 가입 패널")]
    [SerializeField]
    Animator panelSignUp;
    public TMP_InputField inputIDSignUp;    
    public TMP_InputField inputPWSignUp;
    public TMP_InputField inputCPWSignUp;
    [SerializeField]
    Button buttonCancel;    
    public Button buttonSignUp;


    [Header("플레이어 설정 패널")]
    [SerializeField]
    Animator panelSetPlayer;
    public TMP_InputField inputNickName;
    [SerializeField]
    Button buttonToRight;
    [SerializeField]
    Button buttonToLeft;
    [SerializeField]
    Camera avatarCamera;
    [SerializeField]
    GameObject[] avatars;
    [SerializeField]
    RawImage avatarImage;
    RenderTexture renderTexture;
    public int currentIndex = 0;    
    public Button buttonConfirmPlayer;

    [Header("로딩 패널")]    
    public GameObject panelLoading;
    public int panelNumber = 0;

    [Header("종료 버튼")]
    [SerializeField]
    Button buttonExitApp;

    [Header("앱 버전")]
    [SerializeField]
    TextMeshProUGUI curAppVer;

    [Header("구글 해시 키")]
    public Button getGoogleHashKey;
    public TMP_InputField getHashKey;

    [Header("캔버스")]
    public RectTransform canvas;

    void Awake()
    {
        if(instance == null) { instance = this; }
    }

    void Start()
    {
        #region 버튼 및 토글 이벤트 연결
        buttonTrySignUp.onClick.AddListener(() =>
        {
            TrySignUp();
        });

        buttonCancel.onClick.AddListener(() =>
        {
            CancelSignUp();
        });        

        buttonToRight.onClick.AddListener(() =>
        {
            currentIndex++;
            if(currentIndex >= avatars.Length)
                currentIndex = 0;
            StartCoroutine(ToggleAvatars());
        });

        buttonToLeft.onClick.AddListener(() =>
        {
            currentIndex--;
            if (currentIndex < 0)
                currentIndex = avatars.Length -1;
            StartCoroutine(ToggleAvatars());
        });

        buttonConfirmPlayer.onClick.AddListener(() =>
        {
            PlayerData.avatarNumber = currentIndex;
        });

        buttonExitApp.onClick.AddListener(() =>
        {

            Config.ExitApp();
        });

        SetToggleButton();
        toggleSaveID.onValueChanged.AddListener(ToggleSaveID);
        toggleAutoLogin.onValueChanged.AddListener(ToggleAutoLogin);
        #endregion

        curAppVer.text = "Current App Ver   " + Application.version;

        // 아바타 텍스쳐 & 카메라 세팅
        InitAvatarCamera();

        // 자동 로그인
        if (autoLogin)
        {
            AutoLogin();
        }
        // 아이디 저장
        else if(saveID)
        {
            string id = AES.LoadData(AES.USER_ID);
            if(!id.Equals(string.Empty))
            {
                inputIDLogin.text = id;
            }
        }

#if UNITY_EDITOR_WIN
        inputIDLogin.text = "test01";
        inputPWLogin.text = "eoqkr!@34";
#endif
    }    

    void TrySignUp()
    {
        inputIDLogin.text = string.Empty;
        inputPWLogin.text = string.Empty;
        string text = LocalizationManager.Instance.LocaleTable("약관");
        PopupManager.Instance.ShowTwoButtnPopup(true, text, GoToSignUpPanel);
    }

    void CancelSignUp()
    {
        inputIDSignUp.text = string.Empty;
        inputPWSignUp.text = string.Empty;
        panelNumber = (int)PanelNumber.Login;
        StartCoroutine(ShowPanel(panelSignUp, panelLogin));
    }

    void NavigateTo(TMP_InputField targetField)
    {
        targetField.Select();
        targetField.ActivateInputField();
    }

    void AutoLogin()
    {
        if(AES.LoadData(AES.Auto_Login).Equals("1"))
        {
            string id = AES.LoadData(AES.USER_ID);
            string pw = AES.LoadData(AES.USER_PASS);

            Registration.Instance.Login(id, pw);
        }
    }

#region 인풋창 셀렉트 키
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            switch(panelNumber)
            {
                case (int)PanelNumber.Login:
                    if (inputIDLogin.isFocused)
                    {
                        NavigateTo(inputPWLogin);
                    }
                    else if (inputPWLogin.isFocused)
                    {
                        NavigateTo(inputIDLogin);
                    }
                    break; 

                case (int)PanelNumber.SignUp:
                    if (inputIDSignUp.isFocused)
                    {
                        NavigateTo(inputPWSignUp);
                    }
                    else if (inputPWSignUp.isFocused)
                    {
                        NavigateTo(inputCPWSignUp);
                    }
                    else if (inputCPWSignUp.isFocused)
                    {
                        NavigateTo(inputIDSignUp);
                    }
                    break; 
            }            
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            switch (panelNumber)
            {
                case (int)PanelNumber.Login:
                    Registration.Instance.Login(inputIDLogin.text, inputPWLogin.text);
                    break;
                case (int)PanelNumber.SignUp:
                    Registration.Instance.SignUp(inputIDSignUp.text, inputPWSignUp.text, inputCPWSignUp.text);
                    break;
                case (int)PanelNumber.SetPlayer:
                    Registration.Instance.SetPlayer(inputNickName.text);
                    break;
            }            
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            switch (panelNumber)
            {
                case (int)PanelNumber.Login:
                    Config.ExitApp();
                    break;
                case (int)PanelNumber.SignUp:
                    CancelSignUp();
                    break;                
            }            
        }
    }
    #endregion

#region 아바타 관련

    void InitAvatarCamera()
    {
        renderTexture = new RenderTexture(256, 256, 24);
        avatarImage.texture = renderTexture;
        avatarCamera.targetTexture = null;
        avatarCamera.clearFlags = CameraClearFlags.SolidColor;
        avatarCamera.backgroundColor = Color.clear;
        avatarCamera.gameObject.SetActive(true);
        avatarCamera.targetTexture = renderTexture;
    }

    IEnumerator ToggleAvatars()
    {
        avatarCamera.targetTexture = null;

        // 모든 오브젝트 비활성화 한 뒤 현재 인덱스의 오브젝트만 활성화
        foreach (GameObject obj in avatars)
        {
            obj.SetActive(false);
        }
        if (currentIndex >= 0 && currentIndex < avatars.Length)
        {
            avatars[currentIndex].SetActive(true);
        }

        // 이전 렌더링의 잔상이 남는 현상을 해결하기 위함.
        avatarCamera.clearFlags = CameraClearFlags.SolidColor;
        avatarCamera.backgroundColor = Color.clear;

        avatarCamera.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        avatarCamera.gameObject.SetActive(true);
        avatarCamera.targetTexture = renderTexture;
    }
#endregion

#region 패널 관련
    // 파라미터 : 사라질 패널, 보여줄 패널
    IEnumerator ShowPanel(Animator firstPanel, Animator secondPanel)
    {
        SoundManager.Instance.PlaySFX(SFX.Panel);
        firstPanel.SetBool("isShow", false);
        yield return new WaitForSeconds(0.5f);
        secondPanel.SetBool("isShow", true);
    }

    public void GoToLoginPanel()
    {
        panelNumber = (int)PanelNumber.Login;
        StartCoroutine(ShowPanel(panelSetPlayer, panelLogin));
    }

    public void GoToSignUpPanel()
    {
        panelNumber = (int)PanelNumber.SignUp;
        StartCoroutine(ShowPanel(panelLogin, panelSignUp));
    }

    public void GoToSetPlayerPanel()
    {
        panelNumber = (int)PanelNumber.SetPlayer;
        StartCoroutine(ShowPanel(panelSignUp, panelSetPlayer));
    }

    public void GoToReRegistration()
    {
        panelNumber = (int)PanelNumber.SetPlayer;
        StartCoroutine(ShowPanel(panelLogin, panelSetPlayer));
    }
#endregion

#region 토글 관련
    void SetToggleButton()
    {
        SetToggleIsOn(AES.Auto_Login, toggleAutoLogin);
        SetToggleIsOn(AES.Save_ID, toggleSaveID);
    }

    void SetToggleIsOn(string key, Toggle toggle)
    {
        string value = "0";
        int savedIntValue = 0;
        if (!AES.LoadData(key).Equals(""))
        {
            value = AES.LoadData(key);
        }

        savedIntValue = int.Parse(value);
        if (toggle == toggleAutoLogin)
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
        SoundManager.Instance.PlaySFX(SFX.Click);
        autoLogin = value;
        SaveBoolean(autoLogin, AES.Auto_Login);
    }

    void ToggleSaveID(bool value)
    {
        SoundManager.Instance.PlaySFX(SFX.Click);
        saveID = value;
        SaveBoolean(saveID, AES.Save_ID);
    }

    void SaveBoolean(bool boolean, string key)
    {
        int intValueToboolean = boolean ? 1 : 0;
        AES.SaveData(key, intValueToboolean.ToString());
    }
#endregion    
}
