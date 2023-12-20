using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 저장된 URL 로드
/// </summary>

public class WebviewLoadURL : MonoBehaviour
{   
    public string URL;

    void Awake()
    {    
        if(PhotonManagerLobby.Instance != null )
        {
            URL = PhotonManagerLobby.Instance.URL;
        }
    }

    void Start()
    {
        if (PhotonManagerLobby.Instance != null)
        {            
            LoadURL(URL);
        }
    }

    public void LoadURL(string url)
    {
        WebviewManager.Instance.LoadUrl(true, url);
    }
}
