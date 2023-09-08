using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using TMPro;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    readonly string version = "1.0";
    string userId = "Kang";

    public TMP_InputField userInput;
    public TMP_InputField roomInput;

    // 룸 관리
    Dictionary<string, GameObject> rooms = new Dictionary<string, GameObject>();
    GameObject roomPrefab;
    public Transform content;

    void Awake()
    {
        // 마스터 클라이언트의 씬 자동 동기화 옵션
        PhotonNetwork.AutomaticallySyncScene = true;
        // 게임의 버전 설정
        PhotonNetwork.GameVersion = version;    
        // 닉네임
        PhotonNetwork.NickName = userId;

        // 서버와의 초당 데이터 통신 전송 횟수
        Debug.Log("초당 데이터 전송 횟수 : " + PhotonNetwork.SendRate);

        // 프리펩 로드
        roomPrefab = Resources.Load<GameObject>("Room Info");

        // 포톤 서버 접속
        if(PhotonNetwork.IsConnected == false )
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    void Start()
    {
        userId = PlayerPrefs.GetString("USER_ID", $"USER_{Random.Range(1, 21):00}");
        userInput.text = userId;

        PhotonNetwork.NickName = userId;
    }

    // 유저명 설정
    public void SetUserID()
    {
        if(string.IsNullOrEmpty(userInput.text))
        {
            userId = $"USER_{Random.Range(1, 21):00}";
        }
        else
        {
            userId = userInput.text;
        }
        PlayerPrefs.SetString("USER_ID", userId);
        PhotonNetwork.NickName = userId;
    }

    // 룸이름 입력 여부
    string SetRoomName()
    {
        if (string.IsNullOrEmpty(roomInput.text))
        {
            roomInput.text = $"ROOM_{Random.Range(1, 101):000}";
        }
        return roomInput.text;
    }

    // 포톤 서버에 접속 후 호출되는 콜백 함수
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connect to Master, 포톤 서버 접속");
        Debug.Log($"PhotonNetwork.InLobby = {PhotonNetwork.InLobby}, 로비 접속 실패");
        PhotonNetwork.JoinLobby();
    }

    // 로비 접속 후 호출되는 콜백 함수
    public override void OnJoinedLobby()
    {
        Debug.Log($"PhotonNetwork.InLobby = {PhotonNetwork.InLobby}, 로비 접속");

        // 수동 입장 위해 주석
        //PhotonNetwork.JoinRandomRoom();
    }

    // 랜덤 룸에 실패, 룸 생성
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"JoinRandom Failed {returnCode} {message}, 랜덤 룸 입장 실패");

        // 룸 생성
        OnMakeRoomClick();

        // 아래 기능은 버튼으로 연결하기 위해 주석
        //// 룸 속성 정의
        //RoomOptions roomOptions = new RoomOptions();
        //roomOptions.MaxPlayers = 10;
        //roomOptions.IsOpen = true;
        //roomOptions.IsVisible = true;

        //// 룸 생성
        //PhotonNetwork.CreateRoom("MyRoom", roomOptions);
    }       

    // 룸 생성 후 콜백
    public override void OnCreatedRoom()
    {
        Debug.Log("Created Room, 방 생성");
        Debug.Log($"Room Name : {PhotonNetwork.CurrentRoom.Name}");
    }

    // 룸에 입장 후 콜백
    public override void OnJoinedRoom()
    {
        Debug.Log($"PhotonNetwork.InRoom : {PhotonNetwork.InRoom}, 룸 입장");
        Debug.Log($"Player Count : {PhotonNetwork.CurrentRoom.PlayerCount}");

        foreach(var player in PhotonNetwork.CurrentRoom.Players)
        {
            Debug.Log($"닉네임 : {player.Value.NickName}, 고유 넘버 : {player.Value.ActorNumber}");
        }

        // 마스터 클라이언트인 경우 메인 씬 로드
        if(PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("Angry Bot");

        }
        //// 입장 후 플레이어 생성
        //Transform[] points = GameObject.Find("Spawn").GetComponentsInChildren<Transform>();
        //int index = Random.Range(1, points.Length);

        //PhotonNetwork.Instantiate("Player", points[index].position, points[index].rotation, 0);
    }

    // 룸 목록을 수신
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        // 삭제된 룸 정보 프리팹 저장
        GameObject tempRoom = null; 

        foreach(var room in roomList)
        {
            // 룸이 삭제된 경우
            if(room.RemovedFromList == true)
            {
                // 딕셔너리에서 이름 검색하여 프리팹 추출
                rooms.TryGetValue(room.Name, out tempRoom);

                // 삭제
                Destroy(tempRoom);

                // 딕셔너리에서 데이터 삭제
                rooms.Remove(room.Name);
            }
            // 룸 정보 변경된 경우
            else
            {
                // 룸 이름이 딕셔너리에 없는 경우 추가
                if(rooms.ContainsKey(room.Name) == false) 
                {
                    GameObject createRoom = Instantiate(roomPrefab, content);
                    createRoom.GetComponent<RoomData>().RoomInfo = room;

                    // 추가
                    rooms.Add(room.Name, createRoom);
                }
                // 룸 이름이 있을 경우 갱신
                else
                {
                    rooms.TryGetValue(room.Name, out tempRoom);
                    tempRoom.GetComponent<RoomData>().RoomInfo = room;
                }
            }
            Debug.Log($"Room={room.Name} ({room.PlayerCount}/{room.MaxPlayers})");
        }
    }

    #region Button
    public void OnLoginClick()
    {
        SetUserID();
        PhotonNetwork.JoinRandomRoom();
    }

    public void OnMakeRoomClick()
    {
        SetUserID();

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 10;
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;

        PhotonNetwork.CreateRoom(SetRoomName(), roomOptions);
    }
    #endregion
}
