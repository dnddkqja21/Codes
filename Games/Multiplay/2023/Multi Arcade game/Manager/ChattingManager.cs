using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ChattingManager : MonoBehaviourPun
{
    static ChattingManager instance = null;
    public static ChattingManager Instance {  get { return instance; } }

    [SerializeField]
    TMP_InputField inputChat;
    [SerializeField]
    TextMeshProUGUI noticeArea;
    [SerializeField]
    Transform content;
    [SerializeField]
    TextMeshProUGUI chatPrefab;
    [SerializeField]
    ScrollRect scrollView;
    float lineHeight = 50;

    // 비속어 필터 관리
    Dictionary<string, string> textMap = new Dictionary<string, string>();

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        InitTextMap();
        inputChat.onEndEdit.AddListener(OnInputEndEdit);
    }

    void Update()
    {
        ChattingOnPC();
    }

    void ChattingOnPC()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SendChat(inputChat.text);
            inputChat.text = string.Empty;

            string text = LocalizationManager.Instance.LocaleTable("채팅하기");
            inputChat.placeholder.GetComponent<TextMeshProUGUI>().text = text;
        }
    }

    public void SendChat(string inputMsg)
    {
        if (inputMsg == string.Empty)
            return;

        Dictionary<string, string> sendMsg = new Dictionary<string, string>();
        string nick = PhotonManager.Instance.player != null ? PhotonManager.Instance.player.GetPhotonView().Owner.NickName : "TEST : ";
        sendMsg.Add("nick", nick);
        sendMsg.Add("msg", inputMsg + "\n");

        // broadcast
        photonView.RPC("ReceiveChat", RpcTarget.All, sendMsg);
    }

    [PunRPC]
    void ReceiveChat(Dictionary<string, string> sendMsg)
    {
        // 공지 메시지
        // 자를 길이 보다는 길어야 한다.
        if (sendMsg["msg"].Length >= 3)
        {
            if (sendMsg["msg"].Substring(0, 3).Equals("!공지"))
            {
                string notice = LocalizationManager.Instance.LocaleTable("공지");
                noticeArea.text = notice + sendMsg["msg"].Substring(4);
                return;
            }
        }

        GameObject chat = Instantiate(chatPrefab.gameObject, content);
        TextMeshProUGUI chatTmp = chat.GetComponent<TextMeshProUGUI>();

        // 메시지        
        DateTime now = DateTime.Now;       
        int hour = now.Hour;
        int minute = now.Minute;
        
        string time = $"{hour:D2}:{minute:D2}";

        chatTmp.text = " [" + time + "] [" + sendMsg["nick"] + "] : " + sendMsg["msg"] + "\n";

        // 텍스트의 길이에 따른 적절한 높이 계산
        int lineCount = Mathf.CeilToInt(chatTmp.preferredHeight / lineHeight);
        float newHeight = lineCount * lineHeight - 50f;
        chatTmp.rectTransform.sizeDelta = new Vector2(chatTmp.rectTransform.sizeDelta.x, newHeight);

        // 스크롤뷰 항상 최신 메시지 보이도록 함, 하이라키(채팅 오브젝트)에 콘텐츠 사이즈피터 추가해야 함
        Canvas.ForceUpdateCanvases();
        scrollView.verticalNormalizedPosition = 0.0f;
    }

    void OnInputEndEdit(string value)
    {
        string temp = MessageFilter(value);
        SendChat(temp);
        inputChat.text = string.Empty;
    }

    // 메시지 필터링
    string MessageFilter(string value)
    {
        foreach (KeyValuePair<string, string> kvp in textMap)
        {
            value = value.Replace(kvp.Key, kvp.Value);
        }
        return value;
    }    

    public void OnSelectInput()
    {
        inputChat.placeholder.GetComponent<TextMeshProUGUI>().text = string.Empty;
    }
    public void OnDeselectInput()
    {
        string text = LocalizationManager.Instance.LocaleTable("채팅하기");
        inputChat.placeholder.GetComponent<TextMeshProUGUI>().text = text;
    }

    void InitTextMap()
    {
        // 비속어 필터 관리 등등...
        // key -> 수정할 단어
        // value -> 수정될 단어
        textMap.Add("바보", "착한아이");
        textMap.Add("멍청이", "착한아이");
    }
}
