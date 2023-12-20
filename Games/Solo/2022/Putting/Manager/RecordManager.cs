using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordManager : MonoBehaviour
{
    private static RecordManager instance = null;
    public static RecordManager Instance { get { return instance; } }

    [Header("�Ϲ� ��� �˾�")]
    public GameObject[] normalModePopups;
    [Header("�� ��� �˾�")]
    public GameObject[] seeMoreModePopups;

    // ���õ� ���� �˾� ���
    public int infoPopupMode = 1;

    // �� ��� ���ȴ��� ����
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
