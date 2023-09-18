using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 게임을 하는 동안 절전 모드에 의해 휴대폰이 잠기지 않도록 한다.
/// </summary>

public class TimeoutManager : MonoBehaviour
{
    void Start()
    {
        // 잠금 화면 비활성화
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        // 백그라운드 타임아웃 유예시간 설정 (분 단위)
        PhotonNetwork.KeepAliveInBackground = 1800f;
    }
}
