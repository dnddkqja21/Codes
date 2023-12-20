using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 중국학연구소 웹 페이지 로드
/// </summary>

public class OpenWebBrowser : MonoBehaviour
{   
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }
        // 로컬 플레이어일 때 UI띄우기
        if (!other.transform.GetComponent<PhotonView>().IsMine)
            return;

        UIManager.Instance.openWebPopup.SetActive(true);
    }

    void OnTriggerExit(Collider other)
    {
        UIManager.Instance.openWebPopup.SetActive(false);
    }

    public void OpenWebPage()
    {
        Application.OpenURL(EnumToData.Instance.BuildingNameToURL(GameManager.Instance.buildingName));
    }
}
