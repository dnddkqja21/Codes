
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviourPunCallbacks, IOnEventCallback
{
    static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    public TextMeshProUGUI roomName;
    public TextMeshProUGUI connectInfo;
    public TextMeshProUGUI messageList;
    public Button exitButton;

    // �ѹ��� �׽�Ʈ
    public TextMeshProUGUI roomNumText;
    int roomNum = 1;
    string numberKey = "number";

    // �߰� �ڵ� (���ο� ���� ���� �� ������Ʈ)
    byte numberSyncEventCode = 1;

    public int GetNumber()
    {
        return roomNum;
    }

    public void SetNumber(int value)
    {
        roomNum = value;
        ExitGames.Client.Photon.Hashtable num = new ExitGames.Client.Photon.Hashtable();
        num.Add(numberKey, value);
        PhotonNetwork.LocalPlayer.SetCustomProperties(num);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if(targetPlayer != null && targetPlayer == PhotonNetwork.LocalPlayer && changedProps.ContainsKey(numberKey))
        {
            roomNum = (int)changedProps[numberKey];
        }
    }

    void Awake()
    {
        instance = this;

        CreatePlayer();
        SetRoomInfo();
        exitButton.onClick.AddListener(() => OnExitClick());
    }

    // �ѹ��� �׽�Ʈ
    void Start()
    {
        if(PhotonNetwork.InRoom)
        {
            InitNum();
            Debug.Log("�ѹ� �ʱ�ȭ ����");
        }
    }

    // �ѹ��� �׽�Ʈ, �ѹ� �ʱ�ȭ
    void InitNum()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(numberKey, out object value))
        {
            roomNum = (int)value;
        }
        else
        {
            roomNum = 0;
            SetNumber(roomNum);
        }

        MasterCall();
    }

    private void MasterCall()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // Notify the newly connected player (if any) about the current number value
            object[] data = new object[] { roomNum };
            RaiseEventOptions options = new RaiseEventOptions { CachingOption = EventCaching.AddToRoomCache, Receivers = ReceiverGroup.Others };
            PhotonNetwork.RaiseEvent(numberSyncEventCode, data, options, SendOptions.SendReliable);
            Debug.Log("������ ��");
        }
    }

    void SetRoomInfo()
    {
        Room room = PhotonNetwork.CurrentRoom;
        roomName.text = room.Name;
        connectInfo.text = $"({room.PlayerCount}/{room.MaxPlayers})";
    }

    void OnExitClick()
    {
        PhotonNetwork.LeaveRoom();
    }

    // �뿡�� �������� �� ȣ��
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Lobby");
    }

    // ������ �� ������ ���� �� ȣ��
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        SetRoomInfo();
        string msg = $"\n<color=#00ff00>{newPlayer.NickName}</color> is joined room";
        messageList.text += msg;
        MasterCall();
    }

    // �뿡�� ������ ���� �� ȣ��
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        SetRoomInfo();
        string msg = $"\n<color=#ff0000>{otherPlayer.NickName}</color> is left room";
        messageList.text += msg;
    }

    void CreatePlayer()
    {
        Transform[]points = GameObject.Find("Spawn").GetComponentsInChildren<Transform>();
        int index = Random.Range(1, points.Length);

        PhotonNetwork.Instantiate("Player", points[index].position, points[index].rotation, 0);
    }

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == numberSyncEventCode)
        {
            object[] data = (object[])photonEvent.CustomData;
            int remoteNumber = (int)data[0];
            SetNumber(remoteNumber);
        }
    }
}
