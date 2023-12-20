using Photon.Pun;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviourPun
{
    TMP_InputField inputChat;
    TextMeshProUGUI noticeArea;

    [SerializeField]
    Transform content;
    [SerializeField]
    TextMeshProUGUI chatPrefab;
    [SerializeField]
    ScrollRect scrollView;

    float lineHeight = 50;
    Dictionary<string, string> textMap = new Dictionary<string, string>();

    void Start()
    {
        InitMap();
        inputChat = UIManager.Instance.inputChat;
        noticeArea = UIManager.Instance.noticeArea; 

        // 공지는 로컬라이징에서 관리
        //noticeArea.text = "[공지] 순천향 메타버스에 오신 것을 환영합니다!";

        // 모바일 인풋필드 초기화
        inputChat.onEndEdit.AddListener(OnInputEndEdit);
        inputChat.onValueChanged.AddListener(HandleInputValueChanged);
    }

    void InitMap()
    {
        // 차 후 유틸에서 관리할것.
        //key -> 깨지는 중국말
        //value -> 우리말로 수정
        textMap.Add("？", "?");
        textMap.Add("，", ",");
        textMap.Add("！", "!");
        textMap.Add("：", ":");
        textMap.Add("；", ";");
        textMap.Add("＆", "&");
        textMap.Add("＾", "^");
        textMap.Add("～", "~");
        textMap.Add("（", "(");
        textMap.Add("）", ")");
        textMap.Add("＊", "*");
        textMap.Add("＃", "#");
        textMap.Add("％", "%");
        textMap.Add("＿", "_");
        textMap.Add("／", "/");
        textMap.Add("￥", "?");
        textMap.Add("＄", "$");
        textMap.Add("￦", "\\");
        textMap.Add("￡", "?");
        //textMap.Add("�", "?");        
    }

    void HandleInputValueChanged(string newText)
    {
        Debug.Log(newText);
        foreach (KeyValuePair<string, string> kvp in textMap)
        {
            newText = newText.Replace(kvp.Key, kvp.Value);
        }
        inputChat.text = newText;
    }

    void Update() 
    {
        ChattingPC();
    }

    void ChattingPC()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {            
            SendChat(inputChat.text);
            inputChat.text = string.Empty;
            inputChat.placeholder.GetComponent<TextMeshProUGUI>().text = "채팅하기";
        }
    }    

    void OnInputEndEdit(string value)
    {
        SendChat(value);
        inputChat.text = string.Empty;
    }

    void SendChat(string inputMsg)
    {
        if (inputMsg == string.Empty)
            return;

        Dictionary<string, string> sendMsg = new Dictionary<string, string>();
        string nick = PhotonManagerWorld.Instance.player != null ? PhotonManagerWorld.Instance.player.GetPhotonView().Owner.NickName : "TEST : ";
        sendMsg.Add("nick", nick);
        sendMsg.Add("msg", inputMsg + "\n");

        // broadcast
        photonView.RPC("ReceiveChat", RpcTarget.All, sendMsg);
    }
  
    [PunRPC]
    void ReceiveChat(Dictionary<string, string> sendMsg)
    {
        GameObject chat = Instantiate(chatPrefab.gameObject, content);
        TextMeshProUGUI chatTmp = chat.GetComponent<TextMeshProUGUI>();

        // 메시지        
        chatTmp.text = "[" + sendMsg["nick"] + "] : " + sendMsg["msg"] + "\n";

        // 텍스트의 길이에 따른 적절한 높이 계산
        int lineCount = Mathf.CeilToInt(chatTmp.preferredHeight / lineHeight);
        float newHeight = lineCount * lineHeight - 100f;
        chatTmp.rectTransform.sizeDelta = new Vector2(chatTmp.rectTransform.sizeDelta.x, newHeight);       

        // 스크롤뷰 항상 최신 메시지 보이도록 함, 하이라키(채팅 오브젝트)에 콘텐츠 사이즈피터 추가해야 함
        Canvas.ForceUpdateCanvases();
        scrollView.verticalNormalizedPosition = 0.0f;
    }

    public void OnSelectInput()
    {
        inputChat.placeholder.GetComponent<TextMeshProUGUI>().text = "";
    }
    public void OnDeselectInput()
    {
        inputChat.placeholder.GetComponent<TextMeshProUGUI>().text = "채팅하기";
    }
}
