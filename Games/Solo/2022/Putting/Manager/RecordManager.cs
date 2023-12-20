using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordManager : MonoBehaviour
{
    private static RecordManager instance = null;
    public static RecordManager Instance { get { return instance; } }

    [Header("일반 모드 팝업")]
    public GameObject[] normalModePopups;
    [Header("상세 모드 팝업")]
    public GameObject[] seeMoreModePopups;

    // 선택된 인포 팝업 모드
    public int infoPopupMode = 1;

    // 상세 모드 눌렸는지 여부
    public bool isSeeMore = false;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public void OnShowRecord()
    {
        for (int i = 0; i < normalModePopups.Length; i++)
        {
            normalModePopups[i].SetActive(false);
            seeMoreModePopups[i].SetActive(false);
        }

        if (isSeeMore)
        {
            seeMoreModePopups[infoPopupMode - 1].SetActive(true);
        }
        else
        {
            normalModePopups[infoPopupMode - 1].SetActive(true);
        }
    }
}
