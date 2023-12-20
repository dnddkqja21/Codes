using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerName : MonoBehaviour
{
    Vector3 nameOffset = new Vector3(0, 2, 0);

    void Start()
    {
        GameObject playerName = Instantiate(UIManagerWorld.Instance.playerName, transform);
        playerName.transform.localPosition += nameOffset;
        playerName.GetComponent<TextMeshPro>().text = GetComponent<PhotonView>().Owner.NickName;
    }
}
