using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어 추적
/// 카메라가 플레이어를 따라가기 위함
/// </summary>

public class FollowPlayer : MonoBehaviour
{    
    Transform player;

    Vector3 offset = new Vector3(0, 0.5f, 0);

    void Start()
    {
        PhotonManagerWorld.Instance.PlayerCreated += SetPlayer;        
    }

    void SetPlayer()
    {
        player = PhotonManagerWorld.Instance.player.transform;
    }
    
    void LateUpdate()
    {
        if(player != null)
        {
            Vector3 pos = transform.position + offset;
            transform.position = Vector3.Lerp(pos, player.position, 0.4f);
        }
    }
}
