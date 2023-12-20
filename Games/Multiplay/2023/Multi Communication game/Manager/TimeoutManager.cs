using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeoutManager : MonoBehaviour
{
    void Start()
    {
        // 잠금 화면 비활성화
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        // 백그라운드 타임아웃 유예시간 설정
        PhotonNetwork.KeepAliveInBackground = 1800f;
    }
}
