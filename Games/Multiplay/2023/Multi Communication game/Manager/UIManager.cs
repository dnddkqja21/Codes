using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization;
using UnityEngine.UI;

/// <summary>
/// UI 매니저
/// 각종 패널 관리
/// </summary>

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance { get { return instance; } }

    public GameObject uiPopup;   
    public GameObject chatWindow;
    public GameObject minimap;
    public GameObject emoticon;
    public GameObject bCUpAndDown;
    public GameObject bCMuteGroup;
    public GameObject settingWindow;
    public TMP_InputField inputChat;
    public TextMeshProUGUI noticeArea;
    public GameObject loadingPanel;
    public Button buttonLogout;
    public Button buttonSecession;
    public Button buttonTutorial;
    public GameObject fadeOut;
    [SerializeField]
    TextMeshProUGUI currentAppVer;

    [Header("씬체인지, 오픈 웹 팝업")]
    public GameObject changeScenePopup;
    public GameObject openWebPopup;

    // 유저 인포 관련
    [Header("유저 정보")]
    [SerializeField]
    GameObject userInfoPanel;
    [SerializeField]
    TextMeshProUGUI nickName;
    [SerializeField]
    Image nation;
    [SerializeField]
    Sprite[] nations;
    [SerializeField]
    Button like;
    [SerializeField]
    TextMeshProUGUI likeCount;
    [SerializeField]
    Image grade;
    [SerializeField]
    Sprite[] grades;
    [SerializeField]
    TextMeshProUGUI belongTo;
    string userNcnm;
    string lang;
    string userAuthor;
    string gradeNm;
    [SerializeField]
    Button reportButton;

    [Header("웹뷰 버튼")]
    [SerializeField]
    Button scheduleButton;
    [SerializeField]
    Button translatorButton;
    public Button courseButton;

    [Header("알람 관련")]    
    public Button alarmButton;    
    public Sprite[] alarmSprites;
    public GameObject alarmPrefab;
    public Transform contentAlarm;
    public Transform bgDetails;
    public TextMeshProUGUI detailsTitle;
    public TextMeshProUGUI detailsDesc;
    public GameObject emptyAlarmImage;
    public Sprite[] alarmCheckSprites;

    string clickSeq;

    void Awake()
    {
        if(instance == null) instance = this;
    }

    void Start()
    {
        scheduleButton.onClick.AddListener(() =>
            {
                if (PhotonManagerWorld.Instance.player.GetComponent<PlayerAttributes>().isGuest)
                {
                    Locale curLang = LocalizationSettings.SelectedLocale;
                    string text = LocalizationSettings.StringDatabase.GetLocalizedString("Table 01", "게스트 제한", curLang);
                    PopupManager.Instance.ShowOneButtnPopup(text);
                    return;
                }
                changeScenePopup.SetActive(true);
                //WebviewManager.Instance.LoadUrl(true, URL_CONFIG.MAIN_FRONT + URL_CONFIG.SCHEDULE);
                //Debug.Log(URL_CONFIG.MAIN_FRONT + URL_CONFIG.SCHEDULE);
            }
        );

        translatorButton.onClick.AddListener(() =>
            WebviewManager.Instance.LoadUrl(true, URL_CONFIG.MAIN_FRONT + URL_CONFIG.TRANSLATOR)
        );

        alarmButton.onClick.AddListener(() =>
        AlarmManager.Instance.AlarmListApi());

        buttonLogout.onClick.AddListener(() => {
            Locale curLang = LocalizationSettings.SelectedLocale;
            string text = LocalizationSettings.StringDatabase.GetLocalizedString("Table 01", "로그아웃 메시지", curLang);
            PopupManager.Instance.ShowTwoButtnPopup(text, null, PhotonManagerLobby.Instance.LogoutToInit);
        });

        buttonSecession.onClick.AddListener(() => {
            Locale curLang = LocalizationSettings.SelectedLocale;
            string text = LocalizationSettings.StringDatabase.GetLocalizedString("Table 01", "탈퇴 안내", curLang);
            PopupManager.Instance.ShowTwoButtnPopup(text, null, InputPassPopup);
        });

        currentAppVer.text = Application.version;
    }

    void Update()
    {
        // 유저 정보
        if (Input.GetMouseButtonDown(0))
        {
            // UI위에 있을 때 리턴
#if UNITY_EDITOR_WIN || UNITY_STANDALONE || UNITY_EDITOR_OSX
            if (EventSystem.current.IsPointerOverGameObject())
                return;

#elif UNITY_ANDROID || UNITY_IOS
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                    return;
            }
#endif
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                bool hitPlayer = hit.transform.CompareTag("Player");

                if (hitPlayer)
                {
                    PhotonView photonView = hit.transform.GetComponent<PhotonView>();

                    if (photonView == null)
                    {
                        Debug.Log("포톤뷰 널");
                        return;
                    }

                    ExitGames.Client.Photon.Hashtable customProperties = photonView.Owner.CustomProperties;
                    
                    clickSeq = Util.GetStr(customProperties, "userSeq");

                    userNcnm = Util.GetStr(customProperties, "userNcnm");
                    lang = Util.GetStr(customProperties, "lang");
                    userAuthor = Util.GetStr(customProperties, "userAuthor");
                    gradeNm = Util.GetStr(customProperties, "gradeNm");

                    Debug.Log("click clickSeq = " + clickSeq);

                    if (!clickSeq.Equals(UserData.Instance.avatarData.userSeq))
                    {
                        // 내가 아니면 로직 처리 
                        ShowUserInfoPopup();
                    }
                }
            }
        }
    }

    void InputPassPopup()
    {
        Locale curLang = LocalizationSettings.SelectedLocale;
        string text = LocalizationSettings.StringDatabase.GetLocalizedString("Table 01", "패스입력", curLang);
        PopupManager.Instance.ShowInputPopup(text, null, DoSecession);
    }

    // 탈퇴 호출 액션
    void DoSecession()
    {
        string pass = PopupManager.Instance.pass;
        SecessionApi(pass);
    }

    // 탈퇴 팝업
    void SecessionApi(string pass)
    {        
        Dictionary<string, object> requestData = new Dictionary<string, object>();
        requestData.Add("userPassword", pass);
        StartCoroutine(HttpNetwork.Instance.PostSendNetwork(requestData, URL_CONFIG.SECESSION, (data) =>
        {
            Locale curLang = LocalizationSettings.SelectedLocale;
            string text = LocalizationSettings.StringDatabase.GetLocalizedString("Table 01", "탈퇴 완료", curLang);
            PopupManager.Instance.ShowOneButtnPopup(text, LogoutAndPassInit);            
        }));
    }

    void LogoutAndPassInit()
    {
        // 메인 화면으로 가기 전에 값들 비우기
        PhotonManagerLobby.Instance.LogoutToInit();
        PopupManager.Instance.pass = "";
    }

    // 전체 메뉴 UI에서 사용
    public void OnchangeScenePopup()
    {
        if(PhotonManagerWorld.Instance.player.GetComponent<PlayerAttributes>().isGuest)
        {
            string objName = GameManager.Instance.buildingName;
            if (objName.Equals(EnumToData.Instance.BuildingNameToString(BuildingName.ClassRoom)) ||
                objName.Equals(EnumToData.Instance.BuildingNameToString(BuildingName.StudyRoom)) ||
                objName.Equals(EnumToData.Instance.BuildingNameToString(BuildingName.VODCenter)) ||
                objName.Equals(EnumToData.Instance.BuildingNameToString(BuildingName.SeminarRoom)) ||
                objName.Equals(EnumToData.Instance.BuildingNameToString(BuildingName.ProfessorOffice)) ||
                objName.Equals(EnumToData.Instance.BuildingNameToString(BuildingName.ExperienceCenter)))
            {
                Locale curLang = LocalizationSettings.SelectedLocale;
                string text = LocalizationSettings.StringDatabase.GetLocalizedString("Table 01", "게스트 제한", curLang);
                PopupManager.Instance.ShowOneButtnPopup(text);
                return;
            }
        }

        changeScenePopup.SetActive(true);
    }

    void ShowUserInfoPopup()
    {
        if(clickSeq.Equals(""))
        {
            return;
        }
        // 우선 상대 검사 
        Dictionary<string, object> requestData = new Dictionary<string, object>();
        requestData.Add("fromUserSeq", clickSeq);
        StartCoroutine(HttpNetwork.Instance.PostSendNetwork(requestData, URL_CONFIG.CHECK_LIKE, (data) =>
        {
            if (data != null)
            {
                // 보여주기
                userInfoPanel.SetActive(true);

                // 닉네임
                nickName.text = userNcnm;   
                
                // 국적 스프라이트
                nation.sprite = lang.Equals("K") ? nations[0] : nations[1];

                // 등급 추가
                int index = -1; 
                switch(gradeNm)
                {
                    case "브론즈":
                        index = 0;
                        break;
                    case "실버":
                        index = 1;
                        break;
                    case "골드":
                        index = 2;
                        break;
                    case "플래티넘":
                        index = 3;
                        break;
                    case "다이아몬드":
                        index = 4;
                        break;
                }
                grade.sprite = grades[index];

                // 유저 종류
                string belong = "";
                Locale curLang = LocalizationSettings.SelectedLocale;                
                switch (userAuthor)
                {
                    case "G":
                        belong = LocalizationSettings.StringDatabase.GetLocalizedString("Table 01", "게스트", curLang);
                        break;
                    case "SA":
                        belong = LocalizationSettings.StringDatabase.GetLocalizedString("Table 01", "관리자", curLang);                        
                        break;
                    case "PR":
                        belong = LocalizationSettings.StringDatabase.GetLocalizedString("Table 01", "교수", curLang);
                        break;
                    case "ST":
                        belong = LocalizationSettings.StringDatabase.GetLocalizedString("Table 01", "재학생", curLang);
                        break;
                    case "TJ":
                        belong = LocalizationSettings.StringDatabase.GetLocalizedString("Table 01", "텐진", curLang);
                        break;
                    case "FF":
                        belong = LocalizationSettings.StringDatabase.GetLocalizedString("Table 01", "교직원", curLang);
                        break;
                    case "ETA":
                        belong = LocalizationSettings.StringDatabase.GetLocalizedString("Table 01", "중고생", curLang);
                        break;
                    case "ETB":
                        belong = LocalizationSettings.StringDatabase.GetLocalizedString("Table 01", "초등학생", curLang);
                        break;
                    case "ETC":
                        belong = LocalizationSettings.StringDatabase.GetLocalizedString("Table 01", "지역 주민", curLang);
                        break;
                    case "AP":
                        belong = LocalizationSettings.StringDatabase.GetLocalizedString("Table 01", "알파", curLang);
                        break;
                }  
                belongTo.text = belong;  

                // 좋아요
                string count = "0";
                try
                {
                    Dictionary<string, object> d = (Dictionary<string, object>)data;
                    object count2 = d["likeCount"];
                    count = Convert.ToInt32(count2).ToString();
                    
                    Debug.Log("");
                }
                catch(Exception e)
                {
                    Debug.Log(e);
                }

               
                likeCount.text = count;
                //LikeUp();

                // 신고하기 버튼에 userSeq
                reportButton.onClick.AddListener(SendUserSeq);
            }
        }));
    }
    
    void SendUserSeq()
    {
        // 개발 중
        //Locale curLang = LocalizationSettings.SelectedLocale;
        //string text = LocalizationSettings.StringDatabase.GetLocalizedString("Table 01", "개발", curLang);
        //PopupManager.Instance.ShowOneButtnPopup(text);

        string sendData = "userSeq=" + UserData.Instance.avatarData.userSeq + "&trgetUserSeq=" + clickSeq;
        WebviewManager.Instance.LoadUrl(true, URL_CONFIG.MAIN_FRONT + URL_CONFIG.MAIN_REPORT + sendData);
    }

    public void LikeUp()
    {
        if (clickSeq.Equals(""))
        {
            return;
        }
        Dictionary<string, object> requestData = new Dictionary<string, object>();
        requestData.Add("fromUserSeq", clickSeq);
        StartCoroutine(HttpNetwork.Instance.PostSendNetwork(requestData, URL_CONFIG.LIKE_UP, (data) =>
        {
            if (data != null)
            {
                // 좋아요를 눌렀습니다.
                // 좋아요를 취소했습니다.
                Dictionary<string, object> d = (Dictionary<string, object>)data;

                string message = (string)d["msg"];
                PopupManager.Instance.ShowOneButtnPopup(message);

                // 정보 갱신                
                ShowUserInfoPopup();
            }
        }));
    }
}
