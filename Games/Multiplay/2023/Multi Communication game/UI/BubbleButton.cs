using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 버블챗 활성화 유무 버튼
/// </summary>

public class BubbleButton : MonoBehaviour
{
    bool isBubble = true;

    public delegate void SetButtonEventHandler();
    public event SetButtonEventHandler SetButton;

    void Awake()
    {
        isBubble = PlayerPrefs.GetInt("IsBubbleEnabled", 1) == 1;
    }

    void Start()
    {
        PhotonManagerWorld.Instance.PlayerCreated += SetPlayer;
    }

    public void ActiveBubbleChat()
    {
        isBubble = !isBubble;
        SetPlayer();
        PlayerPrefs.SetInt("IsBubbleEnabled", isBubble ? 1 : 0);
    }

    void OnSetButton()
    {
        SetButton?.Invoke();
    }

    void SetPlayer()
    {
        PhotonManagerWorld.Instance.player.GetComponent<PlayerAttributes>().bubbleAccept = isBubble;

        ExitGames.Client.Photon.Hashtable customProperties = PhotonManagerWorld.Instance.player.GetComponent<PhotonView>().Owner.CustomProperties;
        customProperties["bubbleAccept"] = isBubble ? "true" : "false";
        PhotonManagerWorld.Instance.player.GetComponent<PhotonView>().Owner.SetCustomProperties(customProperties);
        OnSetButton();
    }
}
