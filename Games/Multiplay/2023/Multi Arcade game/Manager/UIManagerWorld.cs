using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using UnityEngine.SceneManagement;

/// <summary>
/// UI 매니저 (월드 씬)
/// </summary>

public class UIManagerWorld : MonoBehaviour
{
    static UIManagerWorld instance = null;
    public static UIManagerWorld Instance { get { return instance; } }

    [Header("컨트롤러")]
    public PlayerMoveOnMobile moveJoystick;
    public Transform cameraArm;
    public CameraZoomming cameraZoom;
    public Button buttonRun;
    public Button buttonShooting;

    [Header("목표 지점")]
    public GameObject destinationPrefab;
    public float destinationPosY;

    [Header("플레이어 이름")]
    public GameObject playerName;

    [Header("채팅")]
    [SerializeField]
    Animator chatWindow;
    [SerializeField]
    Button buttonChatWindow;
    bool isVisibleChatWindow = true;

    [Header("Settings")]
    [SerializeField]
    Animator settings;
    [SerializeField]
    Button buttonSettings;
    [SerializeField]
    Button buttonLogout;
    [SerializeField]
    Button buttonWithdraw;
    bool isVisibleSettings = false;

    [Header("포탈")]
    [SerializeField]
    Animator waypoint;
    [SerializeField]
    Button portal;
    [SerializeField]
    Button[] points;
    bool isVisiblePortal = false;

    [Header("랭킹")]
    [SerializeField]
    Animator ranking;
    [SerializeField]
    Toggle[] rankTypes;
    [SerializeField]
    Toggle[] games;
    [SerializeField]
    Button[] onOffRanking;
    [SerializeField]
    Image gameIcon;
    [SerializeField]
    Sprite[] gameIcons;
    public Transform contentRank;
    public GameObject rankItem;    
    public Sprite[] medals;

    [Header("로딩 관련 패널")]    
    public GameObject loadingPanel;    
    public GameObject fadeIn;    
    public GameObject fadeOut;

    [Header("Current App Ver")]
    [SerializeField]
    TextMeshProUGUI currentAppVer;

    [Header("터치 불가능")]    
    public GameObject untouchable;    

    [Header("기타 UI")]
    [SerializeField]
    Button buttonExit;
    public Animator colorPicker;
    public Button buttonManual;

    [Header("캔버스")]
    public RectTransform canvas;

    void Awake()
    {
        if(instance == null) { instance = this; }
    }

    void Start()
    {
        #region 버튼 연결
        buttonRun.onClick.AddListener(() =>
        {
            GameManager.Instance.ToggleRun();
        });
        buttonExit.onClick.AddListener(() =>
        {
            Config.ExitApp();
        });
        buttonChatWindow.onClick.AddListener(() =>
        {
            TogglePanel(chatWindow, ref isVisibleChatWindow);
        });
        buttonSettings.onClick.AddListener(() =>
        {
            TogglePanel(settings, ref isVisibleSettings);
        });
        buttonLogout.onClick.AddListener(() =>
        {
            string text = LocalizationManager.Instance.LocaleTable("로그아웃안내");
            PopupManager.Instance.ShowTwoButtnPopup(true, text, Logout);
        });
        buttonWithdraw.onClick.AddListener(() =>
        {
            string text = LocalizationManager.Instance.LocaleTable("탈퇴안내");
            PopupManager.Instance.ShowTwoButtnPopup(true, text, Withdraw);
        });

        portal.onClick.AddListener(() =>
        {
            TogglePanel(waypoint, ref isVisiblePortal);
        });
        waypoint.GetComponent<RectTransform>().sizeDelta = new Vector2(waypoint.GetComponent<RectTransform>().sizeDelta.x, 
                                                                        (waypoint.GetComponent<RectTransform>().GetChild(0).childCount * 85) + 35);
        points[0].onClick.AddListener(() =>
        {
            string text = LocalizationManager.Instance.WaypointTable("풋살장");
            PopupManager.Instance.ShowTwoButtnPopup(true, text, () => TeleportManager.Instance.Teleport(Config.Position_Futsal), ClosePortalMenu);
        });
        points[1].onClick.AddListener(() =>
        {
            string text = LocalizationManager.Instance.WaypointTable("농구장1");
            PopupManager.Instance.ShowTwoButtnPopup(true, text, () => TeleportManager.Instance.Teleport(Config.Position_Basket_One), ClosePortalMenu);
        });
        points[2].onClick.AddListener(() =>
        {
            string text = LocalizationManager.Instance.WaypointTable("농구장2");
            PopupManager.Instance.ShowTwoButtnPopup(true, text, () => TeleportManager.Instance.Teleport(Config.Position_Basket_Two), ClosePortalMenu);
        });
        points[3].onClick.AddListener(() =>
        {
            string text = LocalizationManager.Instance.WaypointTable("공원1");
            PopupManager.Instance.ShowTwoButtnPopup(true, text, () => TeleportManager.Instance.Teleport(Config.Position_Park_One), ClosePortalMenu);
        });
        points[4].onClick.AddListener(() =>
        {
            string text = LocalizationManager.Instance.WaypointTable("공원2");
            PopupManager.Instance.ShowTwoButtnPopup(true, text, () => TeleportManager.Instance.Teleport(Config.Position_Park_Two), ClosePortalMenu);
        });
        points[5].onClick.AddListener(() =>
        {
            string text = LocalizationManager.Instance.WaypointTable("미로게임");
            PopupManager.Instance.ShowTwoButtnPopup(true, text, () => TeleportManager.Instance.Teleport(Config.Position_Maze), ClosePortalMenu);
        });
        points[6].onClick.AddListener(() =>
        {
            string text = LocalizationManager.Instance.WaypointTable("컬러게임");
            PopupManager.Instance.ShowTwoButtnPopup(true, text, () => TeleportManager.Instance.Teleport(Config.Position_Color_Picker), ClosePortalMenu);
        }); 
        points[7].onClick.AddListener(() =>
        {
            string text = LocalizationManager.Instance.WaypointTable("슈팅게임");
            PopupManager.Instance.ShowTwoButtnPopup(true, text, () => TeleportManager.Instance.Teleport(Config.Position_Shooting), ClosePortalMenu);
        });
        points[8].onClick.AddListener(() =>
        {
            string text = LocalizationManager.Instance.WaypointTable("랜덤장소");
            PopupManager.Instance.ShowTwoButtnPopup(true, text, () => TeleportManager.Instance.Teleport(), ClosePortalMenu);
        });

        onOffRanking[0].onClick.AddListener(() =>
        {
            SoundManager.Instance.PlaySFX(SFX.Panel);
            ranking.SetBool("isShow", true);
            RankManager.Instance.GetGameRecord<RecordMaze>(Config.Rank_Uuid_Maze);
        });
        onOffRanking[1].onClick.AddListener(() =>
        {
            SoundManager.Instance.PlaySFX(SFX.Panel);
            ranking.SetBool("isShow", false);
            games[0].isOn = true;
            rankTypes[0].isOn = true;
        });

        // 명전 중 주간 누를 때, 주간 보는 도중 명전 눌렀을 때
        rankTypes[0].onValueChanged.AddListener((isOn) =>
        {
            for (int i = 0; i < games.Length; i++)
            {
                if(games[i].isOn)
                {
                    switch(i)
                    {
                        case 0:
                            RankManager.Instance.GetGameRecord<RecordMaze>(Config.Rank_Uuid_Maze);
                            break;
                        case 1:
                            RankManager.Instance.GetGameRecord<RecordColor>(Config.Rank_Uuid_Color);
                            break;
                        case 2:
                            RankManager.Instance.GetGameRecord<RecordShooting>(Config.Rank_Uuid_Shooting); 
                            break;
                    }
                }
            }
        });
        rankTypes[1].onValueChanged.AddListener((isOn) =>
        {
            for (int i = 0; i < games.Length; i++)
            {
                if (games[i].isOn)
                {
                    switch (i)
                    {
                        case 0:
                            RankManager.Instance.GetGameRecord<RecordMazeHOF>(Config.Rank_Uuid_Maze_HOF);
                            break;
                        case 1:
                            RankManager.Instance.GetGameRecord<RecordColorHOF>(Config.Rank_Uuid_Color_HOF);
                            break;
                        case 2:
                            RankManager.Instance.GetGameRecord<RecordShootingHOF>(Config.Rank_Uuid_Shooting_HOF);
                            break;
                    }
                }
            }
        });

        for (int i = 0; i < games.Length; i++)
        {
            int index = i; 

            games[i].onValueChanged.AddListener((isOn) =>
            {                
                if (isOn)
                {
                    SoundManager.Instance.PlaySFX(SFX.Click);
                    gameIcon.sprite = gameIcons[index];

                    switch (index)
                    {
                        case 0:
                            if (rankTypes[0].isOn)
                            {
                                RankManager.Instance.GetGameRecord<RecordMaze>(Config.Rank_Uuid_Maze);
                            }
                            else
                            {
                                RankManager.Instance.GetGameRecord<RecordMazeHOF>(Config.Rank_Uuid_Maze_HOF);
                            }
                            break;
                        case 1:
                            if (rankTypes[0].isOn)
                            {
                                RankManager.Instance.GetGameRecord<RecordColor>(Config.Rank_Uuid_Color);
                            }
                            else
                            {
                                RankManager.Instance.GetGameRecord<RecordColorHOF>(Config.Rank_Uuid_Color_HOF);
                            }
                            break;
                        case 2:
                            if (rankTypes[0].isOn)
                            {
                                RankManager.Instance.GetGameRecord<RecordShooting>(Config.Rank_Uuid_Shooting);
                            }
                            else
                            {
                                RankManager.Instance.GetGameRecord<RecordShootingHOF>(Config.Rank_Uuid_Shooting_HOF);
                            }
                            break;
                    }
                }
            });
        }

        #endregion

        currentAppVer.text = Application.version;
    }

    public void ClosePortalMenu()
    {
        SoundManager.Instance.PlaySFX(SFX.Panel);
        waypoint.SetBool("isShow", false);
        isVisiblePortal = false;
    }

    // ref 키워드 사용하여 변수 참조
    void TogglePanel(Animator ani, ref bool isVisible)
    {
        SoundManager.Instance.PlaySFX(SFX.Panel);
        isVisible = !isVisible;
        ani.SetBool("isShow", isVisible);
    }

    void Logout()
    {
        Registration.Instance.Logout();
        RemoveDataAndInitScene();
    }

    void Withdraw()
    {
        Registration.Instance.Withdraw();
        RemoveDataAndInitScene();
    }

    void RemoveDataAndInitScene()
    {
        loadingPanel.SetActive(true);
        AES.SaveData(AES.Auto_Login, string.Empty);
        AES.SaveData(AES.Save_ID, string.Empty);
        AES.SaveData(AES.USER_ID, string.Empty);
        AES.SaveData(AES.USER_PASS, string.Empty);
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene((int)SceneList.Init);        
    }

    // test
    //IEnumerator ChangeSprite(RectTransform image, bool boolean)
    //{
    //    yield return new WaitForSeconds(1);
    //    if (image != null)
    //    {
    //        float z = boolean ? 0 : 180;
    //        image.eulerAngles = new Vector3(0, 0, z);
    //    }
    //}

}
