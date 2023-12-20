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

    // �� ����
    Dictionary<string, GameObject> rooms = new Dictionary<string, GameObject>();
    GameObject roomPrefab;
    public Transform content;

    void Awake()
    {
        // ������ Ŭ���̾�Ʈ�� �� �ڵ� ����ȭ �ɼ�
        PhotonNetwork.AutomaticallySyncScene = true;
        // ������ ���� ����
        PhotonNetwork.GameVersion = version;    
        // �г���
        PhotonNetwork.NickName = userId;

        // �������� �ʴ� ������ ��� ���� Ƚ��
        Debug.Log("�ʴ� ������ ���� Ƚ�� : " + PhotonNetwork.SendRate);

        // ������ �ε�
        roomPrefab = Resources.Load<GameObject>("Room Info");

        // ���� ���� ����
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

    // ������ ����
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

    // ���̸� �Է� ����
    string SetRoomName()
    {
        if (string.IsNullOrEmpty(roomInput.text))
        {
            roomInput.text = $"ROOM_{Random.Range(1, 101):000}";
        }
        return roomInput.text;
    }

    // ���� ������ ���� �� ȣ��Ǵ� �ݹ� �Լ�
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connect to Master, ���� ���� ����");
        Debug.Log($"PhotonNetwork.InLobby = {PhotonNetwork.InLobby}, �κ� ���� ����");
        PhotonNetwork.JoinLobby();
    }

    // �κ� ���� �� ȣ��Ǵ� �ݹ� �Լ�
    public override void OnJoinedLobby()
    {
        Debug.Log($"PhotonNetwork.InLobby = {PhotonNetwork.InLobby}, �κ� ����");

        // ���� ���� ���� �ּ�
        //PhotonNetwork.JoinRandomRoom();
    }

    // ���� �뿡 ����, �� ����
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"JoinRandom Failed {returnCode} {message}, ���� �� ���� ����");

        // �� ����
        OnMakeRoomClick();

        // �Ʒ� ����� ��ư���� �����ϱ� ���� �ּ�
        //// �� �Ӽ� ����
        //RoomOptions roomOptions = new RoomOptions();
        //roomOptions.MaxPlayers = 10;
        //roomOptions.IsOpen = true;
        //roomOptions.IsVisible = true;

        //// �� ����
        //PhotonNetwork.CreateRoom("MyRoom", roomOptions);
    }       

    // �� ���� �� �ݹ�
    public override void OnCreatedRoom()
    {
        Debug.Log("Created Room, �� ����");
        Debug.Log($"Room Name : {PhotonNetwork.CurrentRoom.Name}");
    }

    // �뿡 ���� �� �ݹ�
    public override void OnJoinedRoom()
    {
        Debug.Log($"PhotonNetwork.InRoom : {PhotonNetwork.InRoom}, �� ����");
        Debug.Log($"Player Count : {PhotonNetwork.CurrentRoom.PlayerCount}");

        foreach(var player in PhotonNetwork.CurrentRoom.Players)
        {
            Debug.Log($"�г��� : {player.Value.NickName}, ���� �ѹ� : {player.Value.ActorNumber}");
        }

        // ������ Ŭ���̾�Ʈ�� ��� ���� �� �ε�
        if(PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("Angry Bot");

        }
        //// ���� �� �÷��̾� ����
        //Transform[] points = GameObject.Find("Spawn").GetComponentsInChildren<Transform>();
        //int index = Random.Range(1, points.Length);

        //PhotonNetwork.Instantiate("Player", points[index].position, points[index].rotation, 0);
    }

    // �� ����� ����
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        // ������ �� ���� ������ ����
        GameObject tempRoom = null; 

        foreach(var room in roomList)
        {
            // ���� ������ ���
            if(room.RemovedFromList == true)
            {
                // ��ųʸ����� �̸� �˻��Ͽ� ������ ����
                rooms.TryGetValue(room.Name, out tempRoom);

                // ����
                Destroy(tempRoom);

                // ��ųʸ����� ������ ����
                rooms.Remove(room.Name);
            }
            // �� ���� ����� ���
            else
            {
                // �� �̸��� ��ųʸ��� ���� ��� �߰�
                if(rooms.ContainsKey(room.Name) == false) 
                {
                    GameObject createRoom = Instantiate(roomPrefab, content);
                    createRoom.GetComponent<RoomData>().RoomInfo = room;

                    // �߰�
                    rooms.Add(room.Name, createRoom);
                }
                // �� �̸��� ���� ��� ����
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
