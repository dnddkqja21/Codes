using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 포지션 카피
/// </summary>

public class CopyPosition : MonoBehaviour
{
    // 원하는 축만 제어
    [SerializeField]
    bool x, y, z;

    Transform target;

    void Start()
    {
        PhotonManagerWorld.Instance.PlayerCreated += SetPlayer;        
    }

    void SetPlayer()
    {
        target = PhotonManagerWorld.Instance.player.transform;        
    }

    void Update()
    {
        if(!target) return;

        // 축 중 true인 축의 위치는 타겟의 위치, false면 현재 위치 사용
        transform.position = 
            new Vector3
            (
                (x ? target.position.x : transform.position.x),
                (y ? target.position.y : transform.position.y),
                (z ? target.position.z : transform.position.z)
            );
    }
}
