using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ �ϴ� ���� ���� ��忡 ���� �޴����� ����� �ʵ��� �Ѵ�.
/// </summary>

public class TimeoutManager : MonoBehaviour
{
    void Start()
    {
        // ��� ȭ�� ��Ȱ��ȭ
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        // ��׶��� Ÿ�Ӿƿ� �����ð� ���� (�� ����)
        PhotonNetwork.KeepAliveInBackground = 1800f;
    }
}
