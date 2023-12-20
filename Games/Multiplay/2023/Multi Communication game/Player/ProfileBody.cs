using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using System.Collections;
using ExitGames.Client.Photon;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization;


public class ProfileBody : MonoBehaviourPun,  IOnEventCallback
{
    Material textureFront;
    Material textureBack;
    Material profileColor;

    void Awake()
    {
        profileColor = GetComponent<Renderer>().material;
        textureFront = transform.parent.GetChild(2).transform. GetComponent<Renderer>().material;
        textureBack = transform.parent.GetChild(4).transform.GetComponent<Renderer>().material;
        PhotonNetwork.AddCallbackTarget(this);
    }
   
  
    void Start()
    {
        if (photonView.IsMine)
        {
            BroadcastUserData();
        }
    }

   
    void BroadcastUserData()
    {
        photonView.RPC("AddUserInfo", RpcTarget.AllBuffered, UserData.Instance.avatarData.userSeq);

        // api를 쏘지 않으면 다른 사람의 데이터를 볼 수 없다.
        //photonView.RPC("UpdateUserInfo", RpcTarget.AllBuffered);

        // 나의 것만 적용시키면 당연히 다른 사람은 병아리 출력
        //UpdateUserInfo();
    }
    
    public void ExtiApp()
    {
        Application.Quit();
    }

    // 이미지 컨버터 by 바이너리 
    Texture2D ConvertBase64ToTexture(string base64Data)
    {
    
        byte[] imageData = System.Convert.FromBase64String(base64Data);
        Texture2D texture = new Texture2D(1, 1);

        if (texture.LoadImage(imageData))
        {            
            return texture;
        }
        else
        {
            Debug.LogError("Failed to load texture from base64 data.");
            return null;
        }
    }

    Texture2D GetTextureFromDataUri(string dataUri)
    {
        int commaIndex = dataUri.IndexOf(',');
        if (commaIndex != -1 && dataUri.Length > commaIndex + 1)
        {
            string base64Data = dataUri.Substring(commaIndex + 1);
            return ConvertBase64ToTexture(base64Data);
        }

        Debug.LogError("Invalid data URI format.");
        return null;
    }

    [PunRPC]
    void AddUserInfo(string userSeq)
    {
        Dictionary<string, object> requestData = new Dictionary<string, object>();
        requestData.Add("userSeq", userSeq);

        StartCoroutine(HttpNetwork.Instance.PostSendNetwork(requestData, URL_CONFIG.USER_INFO, (data) => {
            if (data != null)
            {

                StartCoroutine(UpdateProfile((Dictionary<string, object>)data));
            }
        }));
    }

    // test 멀티에선 너도 나도 나일 뿐이다. (모두 나의 사진이 출력 됨)
    //[PunRPC]
    void UpdateUserInfo()
    {
        string proflImageCn = UserData.Instance.avatarData.proflImageCn;
        string proflColor = UserData.Instance.avatarData.proflColor;

        if (proflImageCn != null)
        {
            InitTexture(textureBack, proflImageCn);
            InitTexture(textureFront, proflImageCn);
        }

        if (proflColor != null)
        {
            InitColor(profileColor, proflColor);
        }
        else
        {
            InitDummyTexture(textureBack);
            InitDummyTexture(textureFront);
        }
    }

    IEnumerator UpdateProfile(Dictionary<string, object> data)
    {
        string proflImageCn = (string)data["proflImageCn"];
        string proflColor = (string)data["proflColor"];
        // 텍스쳐 및 UI 적용은 따로 해야 함.
        yield return new WaitForSeconds(0.5f);

        if (proflImageCn != null)
        {
            InitTexture(textureBack, proflImageCn);
            InitTexture(textureFront, proflImageCn);
        }
        else
        {
            InitDummyTexture(textureBack);
            InitDummyTexture(textureFront);
        }
        if (proflColor != null)
        {
            InitColor(profileColor, proflColor);
        }        
    }

    void InitDummyTexture(Material texture)
    {
        Texture2D newTexture = GameManager.Instance.dummyProfile;
        texture.mainTexture = newTexture;
    }

    void InitColor(Material color, string userColor)
    {
        Color newColor;
        color.color = ColorUtility.TryParseHtmlString(userColor, out newColor) ? newColor : Color.white;
    }
    void InitTexture(Material texture, string textCode)
    {
        Texture2D newTexture = GetTextureFromDataUri(textCode);
        texture.mainTexture = newTexture != null ? newTexture : null;        
    }
   
    public void OnEvent(EventData photonEvent)
    {
        byte sameUserCode = 33;
        if (photonEvent.Code == sameUserCode)
        {

            object[] data = (object[])photonEvent.CustomData;
            if (data != null && data.Length > 0)
            {
                string receivedUserSeq = (string)data[0];
                if (UserData.Instance.avatarData.userSeq.Equals(receivedUserSeq))
                {
                    PhotonNetwork.Disconnect();

                    Locale curLang = LocalizationSettings.SelectedLocale;
                    string text = LocalizationSettings.StringDatabase.GetLocalizedString("Table 01", "중복", curLang);
                    PopupManager.Instance.ShowOneButtnPopup(text, ExtiApp);
                }
            }
        }
    }
}
