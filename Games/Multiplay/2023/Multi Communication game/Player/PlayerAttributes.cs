using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어 속성 정리
/// </summary>

public class PlayerAttributes : MonoBehaviour
{
    public bool bubbleAble;
    public bool bubbleAccept;
    public float speed = 3f;

    public PhotonView photonView;
    public string seq;
    public string lang;
    public bool isGuest;

    void Awake()
    {
        bubbleAble = false;
        bubbleAccept = true;
    }

    void Start()
    {    
        photonView = GetComponentInParent<PhotonView>();

        if (photonView == null)
        {
            Debug.Log("포톤뷰 널");
            return;
        }

        ExitGames.Client.Photon.Hashtable customProperties = photonView.Owner.CustomProperties;

        seq = Util.GetStr(customProperties, "userSeq");
        if (seq.Equals(""))
        {
            seq = UserData.Instance.avatarData.userSeq;
        }
        lang = Util.GetStr(customProperties, "lang");
        if (lang.Equals(""))
        {
            lang = UserData.Instance.avatarData.lang;
        }
        isGuest = UserData.Instance.avatarData.userAuthor == "G" ? true : false;       
    }
}
