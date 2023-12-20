using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 각 플레이어의 색상을 미니맵에 표시
/// </summary>

public class PlayerColorOnMinimap : MonoBehaviourPun
{
    [SerializeField]
    Color[] colors;
    [SerializeField]
    GameObject arrow;

    Material mat;

    void Start()
    {
        mat = GetComponent<Renderer>().material;
        
        if(photonView.IsMine)
        {
            arrow.SetActive(true);
            MinimapIcon();
            transform.localScale *= 0.1f;
        }
    }

    void MinimapIcon()
    {
        photonView.RPC("SetMinimapIcon", RpcTarget.All, UserData.Instance.avatarData.userSeq);
    }    

    [PunRPC]
    void SetMinimapIcon(string userSeq)
    {
        Dictionary<string, object> requestData = new Dictionary<string, object>();
        requestData.Add("userSeq", userSeq);

        StartCoroutine(HttpNetwork.Instance.PostSendNetwork(requestData, URL_CONFIG.USER_INFO, (data) => {
            if (data != null)
            {

                StartCoroutine(UpdateColor((Dictionary<string, object>)data));
            }
        }));       
    }

    IEnumerator UpdateColor(Dictionary<string, object> data)
    {
        string lang = (string)data["lang"];
        
        yield return new WaitForSeconds(0.5f);
                
        if (mat != null)
        {
            mat.color = lang == "K" ? colors[0] : colors[1];
        }
    }
}
