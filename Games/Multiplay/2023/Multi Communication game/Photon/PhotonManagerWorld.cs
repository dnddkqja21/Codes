using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

/// <summary>
/// 포톤 매니저 (월드)
/// Create Player & RPC
/// </summary>

public class PhotonManagerWorld : MonoBehaviourPunCallbacks, IOnEventCallback
{
    static PhotonManagerWorld instance = null;
    public static PhotonManagerWorld Instance { get { return instance; } }

    public GameObject player;

    int channelName;
    public string numberKey = "number";
    byte numberSyncEventCode = 1;

    // 미니맵 관련
    List<int> miniMapIcons = new List<int>();

    // 테스트 뷰어
    public TextMeshProUGUI channelNameText;

    // 생성 좌표 리스트
    //public List<Vector3> createPos = new List<Vector3>(); 
    public Vector3 createPos;

    public string bubbleKey = "bubbleList";

    // 이벤트
    public event Action<Vector3 , string, string> OnIsMeetChanged;

    public void collisionUser(Vector3 pos ,string bubbleMaster, string joinUser)
    {
        OnIsMeetChanged?.Invoke(pos , bubbleMaster, joinUser);
    }

    // 플레이어가 생성되었음을 알림
    public delegate void PlayerCreatedEventHandler();
    public event PlayerCreatedEventHandler PlayerCreated;
    bool isCreated = false;

    void OnPlayerCreated()
    {
        PlayerCreated?.Invoke();
    }

    void Awake()
    {
        if (instance == null)
            instance = this;

        if (PhotonManagerLobby.Instance.disconnect == true)
        {
            CreatePlayer(PhotonManagerLobby.Instance.reconnectPos);
        }
        else
        {
            CreatePlayer(new Vector3(0, 0, -20f));
        }        
    }

    void Start()
    {
        SetRoomInfo();
        InitChannelNumber();
        //pv = GetComponent<PhotonView>();

        // 이벤트 핸들러 연결
        OnIsMeetChanged += HandleIsMeetChanged;

        Debug.Log("마스터입니까?" + PhotonNetwork.IsMasterClient);

        //Resources.Load("Bubble");

        Hashtable table = PhotonNetwork.CurrentRoom.CustomProperties;

        if (table[bubbleKey] == null)
        {
            Hashtable customProperties = new Hashtable();
            Dictionary<int, List<string>> list = new Dictionary<int, List<string>>();
            customProperties[bubbleKey] = Util.dictionaryToJson(list);
            PhotonNetwork.CurrentRoom.SetCustomProperties(customProperties);
        }
    }

    void CreatePlayer(Vector3 userStart)
    {
        sameUserOut();

        // 월드 올 때마다 로그인 데이터 주입
        // 아이디 비번 가져옴
        string id = UserData.Instance.avatarData.userEmail;
        string pass = Util.LoadData(AESUtil.USER_PASS);

        Dictionary<string, object> requestData = new Dictionary<string, object>();
        requestData.Add("userEmail", id);
        requestData.Add("userPassword", pass);

        StartCoroutine(HttpNetwork.Instance.PostSendNetwork(requestData, URL_CONFIG.LOGIN, (data) =>
        {
            if (data != null)
            {
                // 웹에서 내려온 데이터를 유저데이터에 주입
                UserData.Instance.avatarData.SetFromDictionary((Dictionary<string, object>)data);
            }
            
            PhotonNetwork.NickName = UserData.Instance.avatarData.userNcnm;

            player = PhotonNetwork.Instantiate("Player", userStart, Quaternion.identity, 0);

            PhotonView photonView = player.GetComponent<PhotonView>();
            Hashtable playerProperties = new Hashtable();
            playerProperties["userSeq"] = UserData.Instance.avatarData.userSeq;
            playerProperties["userNcnm"] = UserData.Instance.avatarData.userNcnm;
            playerProperties["lang"] = UserData.Instance.avatarData.lang;
            playerProperties["userAuthor"] = UserData.Instance.avatarData.userAuthor;
            playerProperties["gradeNm"] = UserData.Instance.avatarData.gradeNm;
            playerProperties["bubbleAccept"] = UserData.Instance.avatarData.bubbleAccept;

            photonView.Owner.SetCustomProperties(playerProperties);
            PhotonManagerLobby.Instance.disconnect = false;            
            isCreated = true;
            OnPlayerCreated();
        }));
    }

    private void sameUserOut()
    {
        Player[] players = PhotonNetwork.PlayerList;
        foreach (var player in players)
        {
            Hashtable customProperties = player.CustomProperties;

            string userSeq = Util.GetStr(customProperties, "userSeq");

            if (userSeq.Equals(UserData.Instance.avatarData.userSeq))
            {
                object[] userData = new object[] { userSeq };
                byte sameUserCode = 33;
                RaiseEventOptions options = new RaiseEventOptions
                {
                    Receivers = ReceiverGroup.Others
                };
                PhotonNetwork.RaiseEvent(sameUserCode, userData, options, SendOptions.SendReliable);
                break;
            }
        }
    }

    bool isCreatebubbleCoroutine = false;
    private Queue<IEnumerator> coroutineQueue = new Queue<IEnumerator>();
    bool IsAdjacent(Vector3 currentPosition, Vector3 targetPosition)
    {
        float deltaX = Mathf.Abs(currentPosition.x - targetPosition.x);
        float deltaZ = Mathf.Abs(currentPosition.z - targetPosition.z);

        return deltaX == 1 && deltaZ == 1;
    }
    public IEnumerator CreateBubble(Vector3 bubblePos , string bubbleMaster , string joinUser)
    {
     
        if (!PhotonNetwork.IsMasterClient) { yield break; }

        if (PhotonNetwork.IsConnectedAndReady)
        {

            bool isCreate = true;
            BubbleBox[] allBubbleBox = FindObjectsOfType<BubbleBox>();
            Debug.Log("gusanaglkyo bubbleCount=" + allBubbleBox.Length);
            if (PhotonNetwork.IsMasterClient && allBubbleBox.Length >= 1)
            {
                foreach (BubbleBox box in allBubbleBox)
                {
                    //bool isAdjacent = IsAdjacent(box.transform.position, bubblePos);

                    if (box.bubbleMaster.Equals(bubbleMaster) || box.bubbleMaster.Equals(joinUser) ||
                        box.subMaster.Equals(bubbleMaster) || box.subMaster.Equals(joinUser) )
                    {
                        isCreate = false;
                        break;
                    }
                }
            }

            if(isCreate)
            {
                // 포톤의 소유로 생성한다.
                GameObject tempBubble = PhotonNetwork.InstantiateRoomObject("Bubble", Vector3.zero, Quaternion.identity);
                tempBubble.GetComponent<BubbleBox>().RoomCreater(bubbleMaster, joinUser);
                tempBubble.SetActive(false);
                yield return new WaitForSeconds(0.1f);

                tempBubble.transform.position = bubblePos + new Vector3(0, 0.5f, 0);

                yield return new WaitForSeconds(0.3f);
                tempBubble.SetActive(true);
            }
        }
    }
    private void Update()
    {
        // 큐가 비어있지 않고 작업을 처리 중이 아닌 경우, 다음 작업을 실행합니다.
        if (coroutineQueue.Count > 0 && !isCreatebubbleCoroutine)
        {
            StartCoroutine(ExecuteNextTask());
        }
    }
    [PunRPC]
    public void masterBubbleMessage(object[] parameters)
    {
        // 버블이 동시에 2개가 들어오면 코루틴이 병렬로 처리한다.
        // 이때 문제가 발생하여 큐에 쌓아 직렬로 작업하는 로직 추가.
        coroutineQueue.Enqueue(CreateBubble((Vector3)parameters[0], (string)parameters[1], (string)parameters[2]));
    }
    private IEnumerator ExecuteNextTask()
    {
        isCreatebubbleCoroutine = true;
        yield return StartCoroutine(coroutineQueue.Dequeue());
        isCreatebubbleCoroutine = false;
    }
    void HandleIsMeetChanged(Vector3 bubblePos ,string bubbleMaster , string joinUser)
    {
        object[] parameters = new object[] { bubblePos, bubbleMaster,joinUser};
        photonView.RPC("masterBubbleMessage", RpcTarget.MasterClient, parameters);
    }

    void SetRoomInfo()
    {
        Room room = PhotonNetwork.CurrentRoom;

        Debug.Log("룸 이름 : " + room.Name + ", 접속 인원 수 : " + room.PlayerCount + ", 최대 인원 수 : " + room.MaxPlayers); 
    }

    #region 포톤 콜백
    public override void OnDisconnected(DisconnectCause cause)
    {
        // 클라이언트의 의도가 아닌 경우        
        if (cause != DisconnectCause.DisconnectByClientLogic)
        {
            PhotonManagerLobby.Instance.disconnect = true;
            PhotonManagerLobby.Instance.reconnectPos = player.transform.position;
            // 커넥팅 상황
            Debug.Log("gusanaglkyo  == == == 재연결 시도");

            // 팝업
            //PopupManager.Instance.ShowNoButtnPopup("메타버스에 재접속 중입니다.");

            //재연결 시도중입니다.
            PhotonManagerLobby.Instance.ConnectToPhoton();
        }
    }

    // 룸으로 새 유저가 접속 시 호출
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        SetRoomInfo();
        string msg = $"\n<color=#00ff00>{newPlayer.NickName}</color> is joined room";
        Debug.Log(newPlayer.NickName + " is Joined");
        MasterCall();
        
        if (PhotonNetwork.IsMasterClient)
        {
            SpawnMiniMapIcon(newPlayer);
        }

    }

    // 룸에서 유저가 퇴장 시 호출
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        SetRoomInfo();
        string msg = $"\n<color=#ff0000>{otherPlayer.NickName}</color> is left room";
        Debug.Log(otherPlayer.NickName + " is Left");

        if (miniMapIcons.Contains(otherPlayer.ActorNumber))
        {            
            miniMapIcons.Remove(otherPlayer.ActorNumber);
        }

        // 내가 마스터면 모든 버블을 살피고 리스트 1 미만은 닫는다.
        BubbleBox[] allBubbleBox = FindObjectsOfType<BubbleBox>();
        if (PhotonNetwork.IsMasterClient && allBubbleBox.Length >= 1)
        {
            Hashtable customProperties = otherPlayer.CustomProperties;
            string outUserSeq = (string)customProperties["userSeq"];

            foreach (BubbleBox box in allBubbleBox)
            {
                foreach(string inUserSeq in box.joinUserList)
                {
                    if (inUserSeq.Equals(outUserSeq))
                    {
                        //나간 처리 한다.
                        box.ConnectOut(outUserSeq);
                        break;
                    }
                }
            }
        }

    }

    // 로컬 플레이어 속성 업데이트 시 채널 넘버 업데이트
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (targetPlayer != null &&
            targetPlayer == PhotonNetwork.LocalPlayer &&
            changedProps.ContainsKey(numberKey))
        {
            channelName = (int)changedProps[numberKey];
        }
    }

    // 방에서 퇴장했을 때 호출
    public override void OnLeftRoom()
    {

    }

    #endregion

    public int GetChannelName()
    {
        return channelName;
    }

    // 로컬 채널 넘버 설정, 업데이트 된 값을 저장, 채널 명 동기화
    public void SetNextChannelName(int value)
    {
        channelName = value;
        // 해시 테이블 
        ExitGames.Client.Photon.Hashtable hashNumber = new ExitGames.Client.Photon.Hashtable();
        hashNumber.Add(numberKey, value);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hashNumber);

        //channelNameText.text = "nextChannel : " + channelName.ToString();
    }

    // 채널 번호 초기화
    void InitChannelNumber()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(numberKey, out object value))
        {
            channelName = (int)value;
        }
        // 최초 접속 시 채널 넘버 0으로 초기화
        else
        {
            channelName = 0;
            SetNextChannelName(channelName);

            //channelNameText.text = "nextChannel : " + channelName.ToString();
        }
        Debug.Log("채널 명 초기화 성공 : " + channelName);
        MasterCall();
    }     

    // 채널 명 수정    
    public void UpdateNextChannelName()
    {
        int nextChannelName = GetChannelName() + 1;

        photonView.RPC("UpdateNextChannelNameRPC", RpcTarget.All, nextChannelName);
    }

    // 채널 명 업데이트 
    [PunRPC]
    void UpdateNextChannelNameRPC(int next)
    {
        SetNextChannelName(next);
        Debug.Log("채널명 업데이트 RPC 호출됨");
    }

    // 마스터콜
    void MasterCall()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // 새로운 유저 접속 시 업데이트
            object[] data = new object[] { channelName };
            RaiseEventOptions options = new RaiseEventOptions { CachingOption = EventCaching.AddToRoomCache, Receivers = ReceiverGroup.Others };
            PhotonNetwork.RaiseEvent(numberSyncEventCode, data, options, SendOptions.SendReliable);
            Debug.Log("채널명 마스터 콜");
        }
    }

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == numberSyncEventCode)
        {
            object[] data = (object[])photonEvent.CustomData;
            int remoteNumber = (int)data[0];
            SetNextChannelName(remoteNumber);
        }
    }

    void SpawnMiniMapIcon(Player player)
    {
        if (!miniMapIcons.Contains(player.ActorNumber))
        {
            //GameObject icon = Instantiate(miniMapIconPrefab, miniMapTransform);
            miniMapIcons.Add(player.ActorNumber);
        }
    }    
}
