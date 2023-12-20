//using Photon.Pun;
//using Photon.Realtime;
//using System.Collections;
//using System.Collections.Generic;
//using TMPro;
//using UnityEngine;

//public class ChannelNameRPC : MonoBehaviourPunCallbacks
//{
//    PhotonView pv;

//    int channelName;
//    public string numberKey = "number";

//    // 테스트 뷰어
//    public TextMeshProUGUI channelNameText;

//    void Awake()
//    {
//        channelNameText = GameObject.Find("NextChannelNum Viewer").GetComponent<TextMeshProUGUI>();
//    }

//    void Start()
//    {
//        pv = GetComponent<PhotonView>();
//        InitChannelNumber();
//    }

//    public int GetChannelName()
//    {
//        return channelName;
//    }

//    // 로컬 채널 넘버 설정, 업데이트 된 값을 저장, 채널 명 동기화
//    public void SetNextChannelName(int value)
//    {
//        channelName = value;
//        // 해시 테이블
//        ExitGames.Client.Photon.Hashtable hashNumber = new ExitGames.Client.Photon.Hashtable();
//        hashNumber.Add(numberKey, value);
//        PhotonNetwork.LocalPlayer.SetCustomProperties(hashNumber);

//        channelNameText.text = "nextChannel : " + channelName.ToString();
//    }

//    // 채널 번호 초기화
//    void InitChannelNumber()
//    {
//        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(numberKey, out object value))
//        {
//            channelName = (int)value;
//        }
//        // 최초 접속 시 채널 넘버 0으로 초기화
//        else
//        {
//            channelName = 0;
//            SetNextChannelName(channelName);

//            channelNameText.text = "nextChannel : " + channelName.ToString();
//        }
//        Debug.Log("채널 명 초기화 성공 : " + channelName);
//    }

//    // 채널 명 수정    
//    public void UpdateNextChannelName()
//    {
//        int nextChannelName = GetChannelName() + 1;
        
//        pv.RPC("UpdateNextChannelNameRPC", RpcTarget.AllBufferedViaServer, nextChannelName);
//    }

//    // 채널 명 업데이트 
//    [PunRPC]
//    void UpdateNextChannelNameRPC(int next)
//    {
//        SetNextChannelName(next);
//        Debug.Log("채널명 업데이트 RPC 호출됨");
//    }

    

//    // 로컬 플레이어 속성 업데이트 시 채널 넘버 업데이트
//    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
//    {
//        if (targetPlayer != null &&
//            targetPlayer == PhotonNetwork.LocalPlayer &&
//            changedProps.ContainsKey(numberKey))
//        {
//            channelName = (int)changedProps[numberKey];
//        }
//    }
//}
