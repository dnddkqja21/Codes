using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// 이름 띄우기
/// </summary>

public class NameFloating : MonoBehaviourPunCallbacks
{    
    GameObject nameObj;
    Transform namePos;
    TextMeshPro nameText;
    PhotonView pv;

    IEnumerator Start()
    {
        // 어느 스타트 함수가 먼저 호출될지 모르기 때문에 코루틴으로 대기
        yield return new WaitForSeconds(0.3f);

        pv = GetComponent<PhotonView>();

        namePos = transform.GetChild(0);

        nameObj = Instantiate(GameManager.Instance.nameObjPrefab, transform);
        nameObj.transform.position = namePos.position;
        nameText = nameObj.GetComponent<TextMeshPro>();

        nameText.text = pv.Owner.NickName;
    }

    void Update()
    {
        if(nameObj != null)
        {
            nameObj.transform.position = namePos.position;
            nameObj.transform.rotation = Camera.main.transform.rotation;        
        }
    }
}
