using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 포톤 매니저
/// </summary>

public class PhotonManager : MonoBehaviourPunCallbacks
{
    static PhotonManager instance = null;
    public static PhotonManager Instance { get { return instance; } }

    readonly string version = Config.roomVersion;
    readonly string roomName = Config.roomName;

    public GameObject player;
    Vector3 createPos = new Vector3(27, 0, -27);
    Vector3 disconnectPos = Vector3.zero;
    bool isDisconnect;

    // 플레이어가 생성되었음을 알림
    public delegate void PlayerCreatedEventHandler();
    public event PlayerCreatedEventHandler PlayerCreated;

    bool isPopuped;

    void OnPlayerCreated()
    {
        PlayerCreated?.Invoke();
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

    public override void OnEnable()
    {
        base.OnEnable();
        // 씬 전환 이벤트에 이벤트 핸들러 추가
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().buildIndex == (int)SceneList.Init)
        {
            // 마스터 클라이언트의 씬 자동 동기화
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.GameVersion = version;
            DebugCustom.Log("포톤 서버 초당 데이터 통신 수 : " + PhotonNetwork.SendRate);
        }
        else if (SceneManager.GetActiveScene().buildIndex == (int)SceneList.World)
        {
            if(isDisconnect)
            {
                createPos = disconnectPos;
                string message = LocalizationManager.Instance.LocaleTable("재접속성공");
                PopupManager.Instance.ShowOneButtnPopup(false, message);
            }
            StartCoroutine(CreatePlayer(createPos));
        }
    }

    IEnumerator CreatePlayer(Vector3 createPos)
    {
        yield return new WaitForSeconds(1f);
        // 생성 시 닉네임과 아바타 번호를 플레이어데이터에 주입해야 한다.
        PhotonNetwork.NickName = PlayerData.nickName;
        int num = PlayerData.avatarNumber;
        Quaternion rot = Quaternion.Euler(0, -90, 0);
        player = PhotonNetwork.Instantiate(num.ToString(), createPos, rot, 0);
        isDisconnect = false;
        isPopuped = false;
        OnPlayerCreated();
    }

    public void ConnectToPhoton()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    // 포톤 마스터 서버에 접속
    public override void OnConnectedToMaster()
    {
        Debug.Log("포톤 접속 : " + PhotonNetwork.IsConnected);
        PhotonNetwork.JoinLobby();
    }

    // 로비 접속
    public override void OnJoinedLobby()
    {
        Debug.Log("로비 접속 : " + PhotonNetwork.InLobby);
        JoinRoom();
    }

    // 룸 생성 시
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
            //Debug.Log("닉네임 : " + player.Value.NickName + ", 고유 넘버 : " + player.Value.ActorNumber);
        }

        LoadMainScene();
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log($"JoinRoom Failed {returnCode} {message}, 방 없음 -> 방 생성 시도...");
        JoinRoom();
    }

    void LoadMainScene()
    {
        Debug.Log("메인 씬 로드 시도");
        // 마스터 클라이언트인 경우 메인 씬 로드
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel((int)SceneList.World);
        }
        else
        {
            Debug.Log("마스터 클라이언트가 아님");
        }
    }

    // 시작 버튼 (생성된 룸에 입장)
    public void JoinRoom()
    {        
        // 룸 속성 정의
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 0;
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;

        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
    }

    // 네트워크 연결 끊겼다가 재연결 시 재접속
    public override void OnDisconnected(DisconnectCause cause)
    {
        //Debug.Log("연결 끊김");
        // 클라이언트의 의도가 아닌 경우        
        if (cause != DisconnectCause.DisconnectByClientLogic)
        {
            isDisconnect = true;

            // 플레이어의 좌표 저장
            if(player != null)
            {
                disconnectPos = player.transform.position;
            }

            if(!isPopuped)
            {

                string message = LocalizationManager.Instance.LocaleTable("네트워크끊김");
                PopupManager.Instance.ShowOneButtnPopup(false, message, ClosePopup);
                isPopuped = true;
            }
            ConnectToPhoton();
        }
    }       

    void ClosePopup()
    {
        isPopuped = false;
    }

    // 포커싱이 바뀌었을 때 연결 끊길 시 재접속
    void OnApplicationFocus(bool hasFocus)
    {
        //Debug.Log("앱 포커싱 = " + hasFocus);
        if (hasFocus)
        {
            if (SceneManager.GetActiveScene().buildIndex == (int)SceneList.Init)
                return;

            // 앱이 포그라운드로 진입할 때 수행         
            if (!PhotonNetwork.IsConnected)
            {
                string message = LocalizationManager.Instance.LocaleTable("타임아웃");
                PopupManager.Instance.ShowTwoButtnPopup(false, message, ConnectToPhoton, null, Config.ExitApp);
                //ConnectToPhoton();
            }            
        }        
    }
}