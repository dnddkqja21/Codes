using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NickName : MonoBehaviourPunCallbacks
{
    TextMeshPro nickName;
    PhotonView pv;
    void Start()
    {
        nickName = GetComponent<TextMeshPro>();
        pv = GetComponentInParent<PhotonView>();
        nickName.text = pv.Owner.NickName;
    }
    void Update()
    {
        transform.rotation = Camera.main.transform.rotation;
    }
}
