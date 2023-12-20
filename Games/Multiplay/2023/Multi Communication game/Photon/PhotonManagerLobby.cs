using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 포톤 매니저
/// </summary>

public class PhotonManagerLobby : MonoBehaviourPunCallbacks
{
    static PhotonManagerLobby instance = null;
    public static PhotonManagerLobby Instance { get { return instance; } }
    
    [Header("Fade in Effect")]
    [SerializeField]
    Image background;    
    [SerializeField]
    float waitTime = 0.5f;    
    [SerializeField]
    float fadeInSpeed = 0.65f;

    readonly string version = BuildConfig.roomVersion;
    readonly string roomName = BuildConfig.roomName;

    public bool isLobby;

    // 재접속 관련
    public bool disconnect = false;
    public Vector3 reconnectPos = Vector3.zero;

    // 씬 체인지
    int sceneName = -1;
    public string URL;

    public bool isLogout;

    bool isBackground;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }

        // 마스터 클라이언트의 씬 자동 동기화
        PhotonNetwork.AutomaticallySyncScene = true;
        // 동일한 버전끼리만 만날 수 있도록
        PhotonNetwork.GameVersion = version;
        PhotonNetwork.NickName = "USER";

        Debug.Log("초당 데이터 통신 수 : " + PhotonNetwork.SendRate);

        DontDestroyOnLoad(gameObject);
    }

    public void ConnectToPhoton()
    {
        // 포톤 서버 접속
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    // 포톤 마스터 서버에 접속
    public override void OnConnectedToMaster()
    {
        Debug.Log("포톤 마스터에 접속");
        PhotonNetwork.JoinLobby();
    }

    // 로비 접속
    public override void OnJoinedLobby()
    {
        Debug.Log("로비 접속 : " + PhotonNetwork.InLobby);
        isLobby = true;
        
        // interior에서 네트워크 재연결 후 접속하기 위함
        if(WebviewManager.Instance.isNetworkChanged)
        {
            JoinRoom();
            WebviewManager.Instance.isNetworkChanged = false;
        }

        // Interior 씬이 아닐 때만 조인 룸
        if(SceneManager.GetActiveScene().buildIndex != (int)SceneName.Interior)
        {            
            JoinRoom();
        }
    }

    // 룸 생성
    public override void OnCreatedRoom()
    {
        Debug.Log("방 생성 완료 : " + PhotonNetwork.CurrentRoom.Name);
    }

    // 룸에 입장
    public override void OnJoinedRoom()
    {
        Debug.Log("룸 입장 : " + PhotonNetwork.InRoom + ", 인원 수: " + PhotonNetwork.CurrentRoom.PlayerCount);

        foreach (var player in PhotonNetwork.CurrentRoom.Players)
        {
            Debug.Log("닉네임 : " + player.Value.NickName + ", 고유 넘버 : " + player.Value.ActorNumber);
        }

        // 페이드인 효과 후 씬 로드
        //LoadSceneAndFadeIn();
        //Debug.Log("페이드 효과 호출");


        //SetCustomPropertie();

        LoadMainScene();
    }
    
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log($"JoinRoom Failed {returnCode} {message}, 방 없음 -> 방 생성 시도...");
        JoinRoom();
    }


    // 2D 화면 진입
    public void ChangeSceneToInterior()
    {
        LeaveRoom((int)SceneName.Interior);
        SetURL();
    }

    public void LogoutToInit()
    {
        isLogout = true;
        UserData.Instance.avatarData.ClearUserData();
        LeaveRoom((int)SceneName.Init);
    }

    void LeaveRoom(int scene)
    {     
        if (!PhotonManagerWorld.Instance.player.GetComponent<PhotonView>().IsMine)
            return;

        sceneName = scene;
        SoundManager.Instance.PlaySFX(SFX.OpenDoor);
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        UIManager.Instance.loadingPanel.SetActive(true);

        int scene = sceneName;
        switch(scene)
        {
            // case -1일 경우를 추가 (test 필요)
            case -1:
                //ConnectToPhoton();
                break;
            case 0:
                LoadToInit();
                break;
            case 1:
                break;
            case 2:
                LoadToInterior();
                break;
        }
        sceneName = -1;
    }

    void LoadToInit()
    {
        Util.SaveData(AESUtil.AutoLogin, "");
        Util.SaveData(AESUtil.SaveID, "");
        Util.SaveData(AESUtil.USER_ID,"");
        Util.SaveData(AESUtil.USER_PASS, "");

        PhotonNetwork.Disconnect();
        SceneManager.LoadScene((int)SceneName.Init);
        //PhotonNetwork.LeaveLobby();
        //Debug.Log("로비 나가기 시도");
    }

    void LoadToInterior()
    {        
        SceneManager.LoadScene((int)SceneName.Interior);
    }

    public override void OnLeftLobby()
    {
        Debug.Log("로비 나가기 성공");

        //SceneManager.LoadScene((int)SceneName.Init);
    }

    void LoadMainScene()
    {
        Debug.Log("메인 씬 로드 시도");
        // 마스터 클라이언트인 경우 메인 씬 로드
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(EnumToData.Instance.SceneNameToString(SceneName.World));
        }
        else
        {
            Debug.Log("마스터 클라이언트가 아님");
        }
    }

    // 차후 수정필요하면 구현
    //public override void OnRoomListUpdate(List<RoomInfo> roomList)
    //{
    //    // 여유가 있는 룸이 있는지 여부
    //    bool hasAvailableRoom = false;

    //    foreach (RoomInfo room in roomList)
    //    {
    //        Debug.Log("방의 정보 : " + room.Name + " 맥스 인원 : " + room.MaxPlayers + " 현재 인원 : " + room.PlayerCount);

    //        // 여유가 있는 룸이 있으면 플래그를 설정하고 입장
    //        if (room.PlayerCount < room.MaxPlayers)
    //        {
    //            hasAvailableRoom = true;
    //            PhotonNetwork.JoinRoom(room.Name);
    //            break;
    //        }            
    //    }
    //    // 여유가 있는 룸이 없으면 새로운 룸을 생성
    //    if (!hasAvailableRoom)
    //    {
    //        JoinRoom();
    //    }
    //}

    // 시작 버튼 (생성된 룸에 입장)
    public void JoinRoom()
    {        
        if (!PhotonNetwork.IsConnected)
        {
            ConnectToPhoton();
        }
        // 룸 속성 정의
        //string roomName = "Room" + Random.Range(1, 1000).ToString();
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 0;
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;

        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
    }

    public void SetURL()
    {
        URL = EnumToData.Instance.BuildingNameToURL(GameManager.Instance.buildingName);
    }

#if UNITY_IOS
    // ios
    void OnApplicationPause(bool pauseStatus)
    {
        Debug.Log("ios type = " + pauseStatus);
        if (pauseStatus)
        {
            // 앱이 백그라운드로 진입할 때 수행되는 코드
            isBackground = true;
        }
        else
        {
            // 앱이 포그라운드로 돌아올 때 수행되는 코드
            if (SceneManager.GetActiveScene().buildIndex == (int)SceneName.Init)
                return;
            // 연결 상태인지 확인 후 재연결
            if (!PhotonNetwork.IsConnected)
            {
                ConnectToPhoton(); 
            }
            isBackground = false;
        }
    }

#elif UNITY_ANDROID
    // aos
    void OnApplicationFocus(bool hasFocus)
    {
        Debug.Log("aos type = " + hasFocus);
        if (hasFocus)
        {
            if (SceneManager.GetActiveScene().buildIndex == (int)SceneName.Init)
                return;

            // 앱이 포그라운드로 진입할 때 수행되는 코드            
            if (!PhotonNetwork.IsConnected)
            {
                ConnectToPhoton(); 
            }
            isBackground = false;
        }
        else
        {
            // 앱이 백그라운드로 이동할 때 수행되는 코드
            isBackground = true;
        }
    }
#endif
}
